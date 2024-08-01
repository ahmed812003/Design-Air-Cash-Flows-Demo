namespace DesignAirDemo.Models.Entities
{
    public class Project
    {
        public Project()
        {
            Contractors = new List<Contractor>();
            Suppliers = new List<Supplier>();
            Operations = new List<Operation>(); 
        }


        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public decimal TotalDebitBalance { get; set; }

        public decimal TotalCreditBalance { get; set; }

        public string ClientId { get; set; } = string.Empty;

        public virtual Client Client { get; set; }

        public virtual List<Contractor> Contractors { get; set; }

        public virtual List<Supplier> Suppliers { get; set; }

        public virtual List<Operation> Operations { get; set; } 
    }
}
