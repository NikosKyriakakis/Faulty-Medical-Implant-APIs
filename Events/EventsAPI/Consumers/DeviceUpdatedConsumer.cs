using AutoMapper;
using EventsAPI.Models;
using GenericRepository;
using MassTransit;

namespace EventsAPI.Consumers;

public class DeviceUpdatedConsumer : IConsumer<Contracts.Contracts.DeviceUpdated>
{
    private readonly IRepository<Device, Guid> _repository;
    private readonly IMapper _mapper;

    public DeviceUpdatedConsumer(IRepository<Device, Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<Contracts.Contracts.DeviceUpdated> context)
    {
        var deviceUpdated = context.Message;
        var existingModel = await _repository.GetByIdAsync(deviceUpdated.Id);
        var deviceModel = _mapper.Map<Device>(deviceUpdated);

        if (existingModel == null)
            await _repository.PostAsync(deviceModel);
        else
            await _repository.UpdateAsync(deviceModel);
    }
}