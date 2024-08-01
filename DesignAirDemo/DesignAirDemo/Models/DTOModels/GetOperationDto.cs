namespace DesignAirDemo.Models.DTOModels
{
    public class GetOperationDto
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateTime { get; set; }

        public bool Sign { get; set; }

        public string FilePath { get; set; }
    }
}
