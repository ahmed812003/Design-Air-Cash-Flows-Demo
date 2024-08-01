using DesignAirDemo.Data;
using DesignAirDemo.Models.DTOModels.ClientDtos;
using DesignAirDemo.Models.DTOModels.ContractorDto;
using DesignAirDemo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesignAirDemo.Controllers
{
    public class ContractorsController : Controller
    {
        private readonly AppDbContext _db;

        public ContractorsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<IActionResult> All()
        {
            var contractors = await _db.Contractors.ToListAsync();

            var contractorsDto = contractors.Select(s => new GetContractorDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalDebitBalance = s.TotalDebitBalance,
                TotalCreditBalance = s.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, s.TotalDebitBalance - s.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, s.TotalCreditBalance - s.TotalDebitBalance)

            }).ToList();

            return View(contractorsDto);
        }

        [HttpGet()]
        public async Task<IActionResult> Search(string Name)
        {
            var contractors = await _db.Contractors.Where(c => c.Name.Contains(Name)).ToListAsync();

            var contractorsDto = contractors.Select(s => new GetContractorDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalDebitBalance = s.TotalDebitBalance,
                TotalCreditBalance = s.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, s.TotalDebitBalance - s.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, s.TotalCreditBalance - s.TotalDebitBalance)

            }).ToList();

            return PartialView("_SearchContractors", contractorsDto);
        }


        [HttpGet()]
        public async Task<IActionResult> GetContractorsOfProject(string id)
        {
            var project = await _db.Projects.Include(p => p.Contractors).FirstOrDefaultAsync(p => p.Id == id);
            
            if (project == null)
                return NotFound();

            var contractorsDto = project.Contractors.Select(s => new GetContractorDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalDebitBalance = s.TotalDebitBalance,
                TotalCreditBalance = s.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, s.TotalDebitBalance - s.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, s.TotalCreditBalance - s.TotalDebitBalance),
                ProjectId = project.Id,

            }).ToList();

            return View(contractorsDto);
        }


        [HttpGet()]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddContractorDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var contractor = new Contractor
                {
                    Name = model.Name,
                };
                var dashboardData = await _db.DashboardData.FirstOrDefaultAsync();
                await _db.Contractors.AddAsync(contractor);
                dashboardData.Contractors += 1;
                await _db.SaveChangesAsync();
                return RedirectToAction("All");

            }
        }
    }
}
