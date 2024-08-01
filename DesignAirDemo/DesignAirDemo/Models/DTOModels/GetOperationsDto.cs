namespace DesignAirDemo.Models.DTOModels
{
    public class GetOperationsDto
    {
        public string UserId { get; set; }

        public string ProjectId {  get; set; }

        public string Type { get; set; }

        public string UserName { get; set; }

        public string ProjectName { get; set; }

        public decimal TotalDebitBalance {  get; set; }

        public decimal TotalCreditBalance { get; set; }

        public decimal Different {  get; set; }

        public List<GetOperationDto> getOperationDto { get; set; } = new List<GetOperationDto>();
    }
}
