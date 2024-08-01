namespace DesignAirDemo.Models.Entities
{
    public class ArchivedItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

    }
}
