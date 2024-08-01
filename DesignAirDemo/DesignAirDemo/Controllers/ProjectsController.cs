using DesignAirDemo.Data;
using DesignAirDemo.Models.DTOModels;
using DesignAirDemo.Models.DTOModels.PojectDto;
using DesignAirDemo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DesignAirDemo.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _db;

        public ProjectsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<IActionResult> All()
        {
            var projects = await _db.Projects.ToListAsync();
            var projectsDto = projects.Select(p => new GetProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                TotalDebitBalance = p.TotalDebitBalance,
                TotalCreditBalance = p.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, p.TotalDebitBalance - p.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, p.TotalCreditBalance - p.TotalDebitBalance),
                ClientId = p.ClientId
            }).ToList();

            return View(projectsDto);
        }

        [HttpGet()]
        public async Task<IActionResult> Search(string Name)
        {
            var projects = await _db.Projects.Where(p => p.Name.Contains(Name)).ToListAsync();
            var projectsDto = projects.Select(p => new GetProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                TotalDebitBalance = p.TotalDebitBalance,
                TotalCreditBalance = p.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, p.TotalDebitBalance - p.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, p.TotalCreditBalance - p.TotalDebitBalance),
                ClientId = p.ClientId
            }).ToList();

            return PartialView("_SearchProjects" , projectsDto);
        }

        [HttpGet()]
        public async Task<IActionResult> Add()
        {
            await createClientSelectList();
            return View();
        }

        [HttpGet()]
        public async Task<IActionResult> AddToProject(string id, string type)
        {
            if (type == "Suppliers")
            {
                var supplier = await _db.Suppliers.FindAsync(id);
                if (supplier == null)
                    return NotFound();
            }
            else if (type == "Contractors")
            {
                var contractor = await _db.Contractors.FindAsync(id);
                if (contractor == null)
                    return NotFound();
            }

            var addToProjectDto = new AddToProjectDto
            {
                Id = id,
                Type = type,
            };
            await createAddToProjectSelectList(id, type);
            return View(addToProjectDto);
        }

        [HttpPost()]
        public async Task<IActionResult> Add(AddProjectDto modelDto)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            var project = new Project
            {
                Name = modelDto.Name,
                ClientId = modelDto.ClientId,
            };
            var dashboardData = await _db.DashboardData.FirstOrDefaultAsync();
            await _db.Projects.AddAsync(project);
            dashboardData.Projects += 1;
            await _db.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpPost()]
        public async Task<IActionResult> AddToProject(AddToProjectDto modelDto)
        {
            if(!ModelState.IsValid)
                return View(ModelState);

            var project = await _db.Projects.FindAsync(modelDto.ProjectId);
            if (project == null)
                return NotFound();

            if (modelDto.Type == "Suppliers")
            {
                var supplier = await _db.Suppliers.FindAsync(modelDto.Id);
                if (supplier == null)
                    return NotFound();
                supplier.Projects.Add(project);
            }
            else if (modelDto.Type == "Contractors")
            {
                var contractor = await _db.Contractors.FindAsync(modelDto.Id);
                if (contractor == null)
                    return NotFound();
                contractor.Projects.Add(project);
            }

            await _db.SaveChangesAsync();
            return View(modelDto);
        }

        private async Task createAddToProjectSelectList(string id, string type)
        {
            if (type == "Contractors")
            {
                var contractor = await _db.Contractors.Include(c => c.Projects).FirstOrDefaultAsync(c => c.Id == id);
                if (contractor == null)
                    return;
                var projects = _db.Projects.Where(p => !contractor.Projects.Contains(p));
                SelectList listItems = new SelectList(projects, "Id", "Name", 1);
                ViewBag.ProjectList = listItems;
            }
            else
            {
                var supplier = await _db.Suppliers.Include(c => c.Projects).FirstOrDefaultAsync(c => c.Id == id);
                if (supplier == null)
                    return;
                var projects = _db.Projects.Where(p => !supplier.Projects.Contains(p));
                SelectList listItems = new SelectList(projects, "Id", "Name", 1);
                ViewBag.ProjectList = listItems;
            }  
        }

        private async Task createClientSelectList(int? start = 1)
        {
            var clients = await _db.Clients.ToListAsync();
            SelectList listItems = new SelectList(clients, "Id", "Name", 1);
            ViewBag.ClientList = listItems;
        }
    }
}
