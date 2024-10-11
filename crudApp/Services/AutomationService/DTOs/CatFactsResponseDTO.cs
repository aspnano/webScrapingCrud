namespace crudApp.Services.AutomationService.DTOs
{
    public class CatFactsResponseDTO
    {
        public int Current_Page { get; set; }
        public List<CatFactDTO> Data { get; set; }
    }

    public class CatFactDTO
    {
        public string Fact { get; set; }
        public int Length { get; set; }
    }
}
