namespace DesignAirDemo.Models.DTOModels
{
    public class AddOperationDto
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
        
        public decimal Amount { get; set; }
        
        public string ProjectId { get; set; }
        
        public int OperationTypeId { get; set; }

        public IFormFile Image {  get; set; }
    }
}
