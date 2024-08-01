namespace DesignAirDemo.Models.DTOModels.PojectDto
{
    public class GetProjectDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal TotalDebitBalance { get; set; }

        public decimal TotalCreditBalance { get; set; }

        public decimal CurrentDebitBalance { get; set; }

        public decimal CurrentCreditBalance { get; set; }

        public string ClientId { get; set; }
    }
}
