using DesignAirDemo.Data;
using DesignAirDemo.Models.DTOModels.Archive;
using DesignAirDemo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesignAirDemo.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly AppDbContext _db;
        private readonly string _tempPath;
        public ArchiveController(AppDbContext db)
        {
            _db = db;
            _tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ArchiveFiles");
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var archivedItems = await _db.ArchivedItems.ToListAsync();
            return View(archivedItems);
        }

        [HttpGet()]
        public async Task<IActionResult> Search(string Name)
        {
            var archivedItems = await _db.ArchivedItems.Where(ai => ai.Description.Contains(Name)).ToListAsync();

            return PartialView("_SearchArchive", archivedItems);
        }

        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            var archiveItem = await _db.ArchivedItems.FindAsync(id);
            if (archiveItem == null) return NotFound();

            string fileName = archiveItem.FilePath;

            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be empty.");
            }

            var filePath = Path.Combine(_tempPath, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", archiveItem.Description + Path.GetExtension(archiveItem.FilePath));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToArchiveDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var archiveItem = new ArchivedItem
            {
                Description = model.Description,
            };

            var filePath = Path.Combine(_tempPath, archiveItem.Id + Path.GetExtension(model.File.FileName));
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            archiveItem.FilePath = filePath;

            var dashboardData = await _db.DashboardData.FirstOrDefaultAsync();
            await _db.ArchivedItems.AddAsync(archiveItem);
            
            if (Path.GetExtension(model.File.FileName) == ".pdf")
                dashboardData.Pdfs += 1;
            else
                dashboardData.Images += 1;

            await _db.SaveChangesAsync();
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            var archiveItem =await _db.ArchivedItems.FindAsync(id);
            if (archiveItem == null) return NotFound();

            System.IO.File.Delete(archiveItem.FilePath);

            var dashboardData = await _db.DashboardData.FirstOrDefaultAsync();
            if (Path.GetExtension(archiveItem.FilePath) == ".pdf")
                dashboardData.Pdfs -= 1;
            else
                dashboardData.Images -= 1;

            _db.ArchivedItems.Remove(archiveItem);
            await _db.SaveChangesAsync();
            return Ok("item Deleted Successfully");
        }

    }
}
