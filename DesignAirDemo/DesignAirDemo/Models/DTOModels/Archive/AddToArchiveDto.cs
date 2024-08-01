namespace DesignAirDemo.Models.DTOModels.Archive
{
    public class AddToArchiveDto
    {
        public string Description { get; set; }

        public IFormFile File { get; set; }
    }
}
