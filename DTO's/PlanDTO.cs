namespace RedacaoAPI.DTO_s
{
    public class PlanCreateDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int MaxCorrections { get; set; }
    }

    public class PlanUpdateDTO
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? MaxCorrections { get; set; }
    }
}
