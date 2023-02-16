using AutoMapper;
using DevicesAPI.Dtos;
using DevicesAPI.Models;
using static Contracts.Contracts;

namespace DevicesAPI.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceReadDto>();
            CreateMap<DeviceCreateDto, Device>();
            CreateMap<DeviceUpdateDto, Device>();
            CreateMap<Device, DeviceUpdateDto>();
            CreateMap<Device, DeviceCreated>();
            CreateMap<Device, DeviceUpdated>();
        }
    }
}
