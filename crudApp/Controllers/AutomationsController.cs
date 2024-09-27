using crudApp.Services.AutomationService;
using crudApp.Services.AutomationService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace crudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutomationsController : ControllerBase
    {
        private readonly IAutomationService _automationService;

        public AutomationsController(IAutomationService automationService)
        {
            _automationService = automationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AutomationParameters parameters)
        {
            try
            {
                var result = await _automationService.RunAutomation(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
