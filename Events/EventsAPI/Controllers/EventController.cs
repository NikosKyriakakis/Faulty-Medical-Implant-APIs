using AutoMapper;
using EventsAPI.Dtos;
using EventsAPI.Models;
using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly IRepository<Event, Guid> _eventRepository;
        private readonly IRepository<Device, Guid> _deviceRepository;
        private readonly IMapper _mapper;

        public EventController(IRepository<Event, Guid> eventRepository, IRepository<Device, Guid> deviceRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
        }

        [HttpGet("{deviceId:guid}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EventReadDto>>> GetByDeviceAsync(Guid deviceId)
        {
            if (deviceId == Guid.Empty)
                return BadRequest();

            var eventModels = await _eventRepository.GetAllAsync(item => item.DeviceId == deviceId);
            var deviceIds = eventModels.Select(item => item.DeviceId);
            var deviceModels = await _deviceRepository.GetAllAsync(item => deviceIds.Contains(item.Id));

            var eventReadDtos = eventModels.Select(eventModel =>
            {
                Device device;

                try 
                {
                    device = deviceModels.Single(deviceModel => deviceModel.Id == eventModel.DeviceId);
                }
                catch (InvalidOperationException)
                {
                    return new EventReadDto();
                }

                var eventReadDto = _mapper.Map<EventReadDto>(device);
                
                eventReadDto.EventId = eventModel.Id;
                eventReadDto.Reason = eventModel.Reason;
                eventReadDto.Type = eventModel.Type;

                return eventReadDto;
            });

            return Ok(eventReadDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> PostAsync(EventCreateDto eventCreateDto)
        {
            var eventModel = _mapper.Map<Event>(eventCreateDto);

            await _eventRepository.PostAsync(eventModel);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var eventModels = await _eventRepository.GetAllAsync(item => item.DeviceId == id);

            foreach (var eventModel in eventModels)
                await _eventRepository.DeleteAsync(eventModel.Id);

            return NoContent();
        }
    }
}
