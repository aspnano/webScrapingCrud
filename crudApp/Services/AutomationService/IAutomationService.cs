using crudApp.Services.AutomationService.DTOs;

namespace crudApp.Services.AutomationService
{
    public interface IAutomationService
    {
        Task<int> RunAutomation(AutomationParameters parameters);

    }
}
