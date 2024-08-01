using DesignAirDemo.Data;
using DesignAirDemo.Models.DTOModels.ClientDtos;
using DesignAirDemo.Models.DTOModels.PojectDto;
using DesignAirDemo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesignAirDemo.Controllers
{
    public class ClientsController : Controller
    {
        
        private readonly AppDbContext _db;

        public ClientsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<IActionResult> All()
        {
            var clients = await _db.Clients.ToListAsync();

            var clientsDto = clients.Select(s => new GetClientDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalDebitBalance = s.TotalDebitBalance,
                TotalCreditBalance = s.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, s.TotalDebitBalance - s.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, s.TotalCreditBalance - s.TotalDebitBalance)

            }).ToList();

            return View(clientsDto);
        }

        [HttpGet()]
        public async Task<IActionResult> Search(string Name)
        {
            var clients = await _db.Clients.Where(c => c.Name.Contains(Name)).ToListAsync();

            var clientsDto = clients.Select(s => new GetClientDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalDebitBalance = s.TotalDebitBalance,
                TotalCreditBalance = s.TotalCreditBalance,
                CurrentDebitBalance = Math.Max(0, s.TotalDebitBalance - s.TotalCreditBalance),
                CurrentCreditBalance = Math.Max(0, s.TotalCreditBalance - s.TotalDebitBalance)

            }).ToList();

            return PartialView("_SearchClients", clientsDto);
        }


        [HttpGet()]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddClientDto model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var client = new Client
                {
                    Name = model.Name,
                };

                var dashboardData = await _db.DashboardData.FirstOrDefaultAsync();
                await _db.Clients.AddAsync(client);
                dashboardData.Clients += 1;
                await _db.SaveChangesAsync();
                return RedirectToAction("All");
            }
        }


    }
}
