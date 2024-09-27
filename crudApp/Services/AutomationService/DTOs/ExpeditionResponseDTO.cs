using System.Text.Json.Serialization;

namespace crudApp.Services.AutomationService.DTOs
{
    public class ExpeditionResponseDTO
    {
        public ExpeditionDataDto Data { get; set; }
        public List<ExpeditionItemDto> Result { get; set; }
    }
    public class ExpeditionDataDto
    {
        public string WpJsonFilterExpeditions { get; set; }
        public string ExpeditionPostId { get; set; }
        public string Season { get; set; }
        public string ShipPostId { get; set; }
    }

    public class ExpeditionItemDto
    {
        public string ProductName { get; set; }
        public string StartDate { get; set; }      
        public string StartPrice { get; set; }
    }
}
