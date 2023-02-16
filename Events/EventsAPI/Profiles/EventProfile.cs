using AutoMapper;
using EventsAPI.Dtos;
using EventsAPI.Models;

namespace EventsAPI.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventReadDto>();
            CreateMap<EventCreateDto, Event>();
            CreateMap<EventUpdateDto, Event>();
            CreateMap<Event, EventUpdateDto>();
            CreateMap<DeviceItemDto, EventReadDto>();
            CreateMap<Device, EventReadDto>();
            CreateMap<Contracts.Contracts.DeviceCreated, Device>();
            CreateMap<Contracts.Contracts.DeviceUpdated, Device>();
            CreateMap<Contracts.Contracts.DeviceDeleted, Device>();
        }
    }
}
