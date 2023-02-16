using AutoMapper;
using EventsAPI.Models;
using GenericRepository;
using MassTransit;

namespace EventsAPI.Consumers;

public class DeviceCreatedConsumer : IConsumer<Contracts.Contracts.DeviceCreated>
{
    private readonly IRepository<Device, Guid> _repository;
    private readonly IMapper _mapper;

    public DeviceCreatedConsumer(IRepository<Device, Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<Contracts.Contracts.DeviceCreated> context)
    {
        var message = context.Message;
        var model = await _repository.GetByIdAsync(message.Id);
        if (model != null)
            return;

        model = _mapper.Map<Device>(message);

        await _repository.PostAsync(model);
    }
}