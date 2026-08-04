using Microsoft.AspNetCore.Mvc;
using VidirDashboard.Application.Services;

namespace VidirDashboard.Web.Controllers;

[ApiController]
[Route("api/machines")]
public class MachinesController : ControllerBase
{
    private readonly MachineQueryService _queryService;

    public MachinesController(MachineQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _queryService.GetAllMachineStatusesAsync());

    [HttpGet("{id:int}/status")]
    public async Task<IActionResult> GetStatus(int id)
    {
        var status = await _queryService.GetMachineStatusAsync(id);
        return status is null ? NotFound() : Ok(status);
    }
}