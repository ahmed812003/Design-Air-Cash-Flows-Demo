namespace DesignAirDemo.Models.Entities
{
    public class Supplier
    {
        public Supplier()
        {
            Projects = new List<Project>();
            Operations = new List<Operation>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public decimal TotalDebitBalance { get; set; }

        public decimal TotalCreditBalance { get; set; }

        public List<Project> Projects { get; set; }

        public virtual List<Operation> Operations { get; set; }
    }
}
