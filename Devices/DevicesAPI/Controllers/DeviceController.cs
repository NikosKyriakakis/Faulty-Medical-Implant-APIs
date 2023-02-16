using AutoMapper;
using DevicesAPI.Dtos;
using DevicesAPI.Models;
using GenericRepository;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DevicesAPI.Controllers;

[ApiController]
[Route("api/devices")]
public class DeviceController : ControllerBase
{
    private readonly IRepository<Device, Guid> _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public DeviceController(IRepository<Device, Guid> repository, IPublishEndpoint publishEndPoint, IMapper mapper)
    {
        _repository = repository;
        _publishEndpoint = publishEndPoint;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<DeviceReadDto>>> GetAllAsync()
    {
        var deviceItems = await _repository.GetAllAsync();

        return Ok(_mapper.Map<IEnumerable<DeviceReadDto>>(deviceItems));
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<DeviceReadDto>> GetByIdAsync(Guid id)
    {
        var deviceItem = await _repository.GetByIdAsync(id);
        if (deviceItem == null)
            return NotFound();

        return _mapper.Map<DeviceReadDto>(deviceItem);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<DeviceReadDto>> PostAsync(DeviceCreateDto deviceCreateDto)
    {
        var deviceItem = _mapper.Map<Device>(deviceCreateDto);
        await _repository.PostAsync(deviceItem);

        var deviceCreated = _mapper.Map<Contracts.Contracts.DeviceCreated>(deviceItem);
        await _publishEndpoint.Publish(deviceCreated);

        var deviceReadDto = _mapper.Map<DeviceReadDto>(deviceItem);

        return CreatedAtAction(nameof(GetByIdAsync), new { deviceItem.Id }, deviceReadDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> PutAsync(Guid id, DeviceUpdateDto deviceUpdateDto)
    {
        var deviceModel = await _repository.GetByIdAsync(id);
        if (deviceModel == null)
            return NotFound();

        _mapper.Map(deviceUpdateDto, deviceModel);
        await _repository.UpdateAsync(deviceModel);

        var deviceUpdated = _mapper.Map<Contracts.Contracts.DeviceUpdated>(deviceModel);
        await _publishEndpoint.Publish(deviceUpdated);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> PatchAsync(Guid id, JsonPatchDocument<DeviceUpdateDto> patchDocument)
    {
        var deviceModel = await _repository.GetByIdAsync(id);
        if (deviceModel == null)
            return NotFound();

        var deviceUpdateDto = _mapper.Map<DeviceUpdateDto>(deviceModel);
        patchDocument.ApplyTo(deviceUpdateDto, ModelState);

        if (!TryValidateModel(deviceUpdateDto))
            return ValidationProblem(ModelState);

        _mapper.Map(deviceUpdateDto, deviceModel);
        await _repository.UpdateAsync(deviceModel);

        var deviceUpdated = _mapper.Map<Contracts.Contracts.DeviceUpdated>(deviceModel);
        await _publishEndpoint.Publish(deviceUpdated);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var deviceModel = await _repository.GetByIdAsync(id);
        if (deviceModel == null)
            return NotFound();

        await _repository.DeleteAsync(id);

        var deviceDeleted = new Contracts.Contracts.DeviceDeleted
        {
            Id = id
        };
        await _publishEndpoint.Publish(deviceDeleted);

        return NoContent();
    }
}