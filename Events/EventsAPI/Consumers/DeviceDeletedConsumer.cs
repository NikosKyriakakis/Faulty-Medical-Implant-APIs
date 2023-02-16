using AutoMapper;
using EventsAPI.Models;
using GenericRepository;
using MassTransit;

namespace EventsAPI.Consumers;

public class DeviceDeletedConsumer : IConsumer<Contracts.Contracts.DeviceDeleted>
{
    private readonly IRepository<Device, Guid> _repository;
    private readonly IMapper _mapper;

    public DeviceDeletedConsumer(IRepository<Device, Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<Contracts.Contracts.DeviceDeleted> context)
    {
        var deviceDeleted = context.Message;
        var existingModel = await _repository.GetByIdAsync(deviceDeleted.Id);

        if (existingModel == null)
            return;
            
        await _repository.DeleteAsync(deviceDeleted.Id);
    }
}