namespace DesignAirDemo.Models.Entities
{
    public class Operation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Type { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public decimal Amount { get; set; }

        public bool Sign { get; set; }

        public string FilePath {  get; set; }

        public string? ProjectId { get; set; }
        public virtual Project Project { get; set; }


        public string? ClientId { get; set; }
        public virtual Client Client { get; set; }


        public string? ContractorId { get; set; }
        public virtual Contractor Contractor { get; set; }


        public string? SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }


    }
}
