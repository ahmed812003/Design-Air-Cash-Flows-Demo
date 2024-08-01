using DesignAirDemo.Data;
using DesignAirDemo.Models.DTOModels.ContractorDto;
using DesignAirDemo.Models.DTOModels.SupplierDto;
using DesignAirDemo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesignAirDemo.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly AppDbContext _db;

        public SuppliersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<IActionResult> All()
        {
            var suppliers = await _db.Suppliers.ToListAsync();

            var suppliersDto = suppliers.Select(s => new GetSupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalDebitBalance = s.TotalDebitBalance,
                TotalCreditBalance = s.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, s.TotalDebitBalance - s.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, s.TotalCreditBalance - s.TotalDebitBalance)

            }).ToList();

            return View(suppliersDto);
        }

        [HttpGet()]
        public async Task<IActionResult> Search(string Name)
        {
            var suppliers = await _db.Suppliers.Where(c => c.Name.Contains(Name)).ToListAsync();

            var suppliersDto = suppliers.Select(s => new GetSupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalDebitBalance = s.TotalDebitBalance,
                TotalCreditBalance = s.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, s.TotalDebitBalance - s.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, s.TotalCreditBalance - s.TotalDebitBalance)

            }).ToList();

            return PartialView("_SearchSuppliers", suppliersDto);
        }


        [HttpGet()]
        public async Task<IActionResult> GetSuppliersOfProject(string id)
        {
            var project = await _db.Projects.Include(p => p.Suppliers).FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound();

            var suppliersDto = project.Suppliers.Select(s => new GetSupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalDebitBalance = s.TotalDebitBalance,
                TotalCreditBalance = s.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, s.TotalDebitBalance - s.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, s.TotalCreditBalance - s.TotalDebitBalance),
                ProjectId = project.Id,

            }).ToList();

            return View(suppliersDto);
        }

        [HttpGet()]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddSupplierDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var supplier = new Supplier
                {
                    Name = model.Name,
                };
                var dashboardData = await _db.DashboardData.FirstOrDefaultAsync();
                await _db.Suppliers.AddAsync(supplier);
                dashboardData.Suppliers += 1;
                await _db.SaveChangesAsync();
               
                return RedirectToAction("All");

            }
        }
    }
}
