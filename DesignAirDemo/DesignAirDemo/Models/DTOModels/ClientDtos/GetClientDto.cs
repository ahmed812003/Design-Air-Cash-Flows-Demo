namespace DesignAirDemo.Models.DTOModels.ClientDtos
{
    public class GetClientDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal TotalDebitBalance { get; set; }

        public decimal TotalCreditBalance { get; set; }

        public decimal CurrentDebitBalance { get; set; }

        public decimal CurrentCreditBalance { get; set; }
    }
}
