using DesignAirDemo.Data;
using DesignAirDemo.Models;
using DesignAirDemo.Models.DTOModels;
using DesignAirDemo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace DesignAirDemo.Controllers
{
    public class OperationsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly string _tempPath;

        public OperationsController(AppDbContext db)
        {
            _db = db;
            _tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
        }

        [HttpGet()]
        public async Task<IActionResult> GetOperations([FromQuery] AskForOperations model)
        {
            var project = await _db.Projects.Include(p => p.Operations).SingleOrDefaultAsync(p => p.Id == model.ProjectId);
            if(project == null) 
                return NotFound();

            if(model.Type == "Contractors")
            {
                var contractor = await _db.Contractors.FindAsync(model.UserId);
                if (contractor == null)
                    return NotFound();
               
                var getOperationsDto = project.Operations.Where(o => o.ContractorId == model.UserId).Select(op => new GetOperationDto
                {
                    Id = op.Id,
                    Description = op.Description,
                    Type = op.Type,
                    Amount = op.Amount,
                    Sign = op.Sign,
                    DateTime = op.Date,
                    FilePath = op.FilePath
                }).ToList();

                var operationsDto = new GetOperationsDto
                {
                    UserId = model.UserId,
                    ProjectId = project.Id,
                    Type = model.Type,
                    UserName = contractor.Name,
                    ProjectName = project.Name,
                    TotalCreditBalance = getOperationsDto.Where(op => op.Sign).Sum(op => op.Amount),
                    TotalDebitBalance = getOperationsDto.Where(op => !op.Sign).Sum(op => op.Amount) ,
                    getOperationDto = getOperationsDto,
                };
                operationsDto.Different = operationsDto.TotalCreditBalance - operationsDto.TotalDebitBalance;
                return View(operationsDto);
            }
            else if(model.Type == "Suppliers")
            {
                var supplier = await _db.Suppliers.FindAsync(model.UserId);
                if (supplier == null)
                    return NotFound();

                var getOperationsDto = project.Operations.Where(o => o.SupplierId == model.UserId).Select(op => new GetOperationDto
                {
                    Id = op.Id,
                    Description = op.Description,
                    Type = op.Type,
                    Amount = op.Amount,
                    Sign = op.Sign,
                    DateTime = op.Date,
                    FilePath = op.FilePath
                }).ToList();

                var operationsDto = new GetOperationsDto
                {
                    UserId = model.UserId,
                    ProjectId = project.Id,
                    Type = model.Type,
                    UserName = supplier.Name,
                    ProjectName = project.Name,
                    TotalCreditBalance = getOperationsDto.Where(op => op.Sign).Sum(op => op.Amount),
                    TotalDebitBalance = getOperationsDto.Where(op => !op.Sign).Sum(op => op.Amount),
                    getOperationDto = getOperationsDto
                };
                operationsDto.Different = operationsDto.TotalCreditBalance - operationsDto.TotalDebitBalance;
                return View(operationsDto);
            }
            else
            {
                var client = await _db.Clients.FindAsync(model.UserId);
                if (client == null)
                    return NotFound();

                var getOperationsDto = project.Operations.Where(o => o.ClientId == model.UserId).Select(op => new GetOperationDto
                {
                    Id = op.Id,
                    Description = op.Description,
                    Type = op.Type,
                    Amount = op.Amount,
                    Sign = op.Sign,
                    DateTime = op.Date,
                    FilePath = op.FilePath
                }).ToList();

                var operationsDto = new GetOperationsDto
                {
                    UserId = model.UserId,
                    ProjectId = project.Id,
                    Type = model.Type,
                    UserName = client.Name,
                    ProjectName = project.Name,
                    TotalCreditBalance = getOperationsDto.Where(op => op.Sign).Sum(op => op.Amount),
                    TotalDebitBalance = getOperationsDto.Where(op => !op.Sign).Sum(op => op.Amount),
                    getOperationDto = getOperationsDto
                };
                operationsDto.Different = operationsDto.TotalCreditBalance - operationsDto.TotalDebitBalance;
                return View(operationsDto);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> AddOperation([FromQuery]AskForOperations model)
        {
            var project = await _db.Projects.FindAsync(model.ProjectId);
            if (project == null) return NotFound();

            if (model.Type == "Contractors")
            {
                var contractor = await _db.Contractors.FindAsync(model.UserId);
                if (contractor == null)
                    return NotFound();
            }
            else if (model.Type == "Suppliers")
            {
                var supplier = await _db.Suppliers.FindAsync(model.UserId);
                if (supplier == null)
                    return NotFound();
            }
            else
            {
                var client = await _db.Clients.FindAsync(model.UserId);
                if (client == null)
                    return NotFound();
            }

            var addOperationDto = new AddOperationDto
            {
                Id = model.UserId,
                Type = model.Type,
                ProjectId = model.ProjectId,
            };

            await createOperationTypesSelectList();
            return View(addOperationDto);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadImage(string id)
        {
            var operation = await _db.Operations.FindAsync(id);
            if (operation == null) return NotFound();

            string fileName = operation.FilePath;

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
            return File(memory, "application/octet-stream", operation.Description + Path.GetExtension(operation.FilePath));
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOperation(AddOperationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = await _db.Projects.FindAsync(model.ProjectId);
            if (project == null)
                return NotFound();

            var operationType = await _db.OperationTypes.FindAsync(model.OperationTypeId);
            if (operationType == null)
                return NotFound();

            var operation = new Operation
            {
                Type = operationType.Name,
                Sign = operationType.Sign,
                Description = model.Description,
                Amount = model.Amount,
                ProjectId = model.ProjectId,
            };


            var filePath = Path.Combine(_tempPath, operation.Id + Path.GetExtension(model.Image.FileName));
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            operation.FilePath = filePath;

            if (model.Type == "Contractors")
            {
                var contractor = await _db.Contractors.FindAsync(model.Id);
                if (contractor == null)
                    return NotFound();
                operation.ContractorId = model.Id;
                if(operationType.Sign)
                    contractor.TotalCreditBalance += model.Amount;
                else
                    contractor.TotalDebitBalance += model.Amount;
            }
            else if (model.Type == "Suppliers")
            {
                var supplier = await _db.Suppliers.FindAsync(model.Id);
                if (supplier == null)
                    return NotFound();
                operation.SupplierId = model.Id;
                if (operationType.Sign)
                    supplier.TotalCreditBalance += model.Amount;
                else
                    supplier.TotalDebitBalance += model.Amount;
            }
            else
            {
                var client = await _db.Clients.FindAsync(model.Id);
                if (client == null)
                    return NotFound();
                operation.ClientId = model.Id;
                if (operationType.Sign)
                    client.TotalCreditBalance += model.Amount;
                else
                    client.TotalDebitBalance += model.Amount;
            }

            if (operationType.Sign)
            {
                project.TotalCreditBalance += model.Amount;
            }
            else
            {
                project.TotalDebitBalance += model.Amount;
            }
            var dashboardData = await _db.DashboardData.FirstOrDefaultAsync();
            await _db.Operations.AddAsync(operation);
            dashboardData.Operations += 1;
            await _db.SaveChangesAsync();
            await createOperationTypesSelectList();
            return View(model);
            
        }

        public async Task<IActionResult> Delete([FromQuery]DeleteOperationDto model)
        {
            var operation = await _db.Operations.Include(o => o.Project).SingleOrDefaultAsync(o => o.Id == model.OperationId);
            if(operation == null)
                return NotFound();

            if(model.Type == "Contractors")
            {
                var contractor = await _db.Contractors.FindAsync(model.UserId);
                if(contractor == null) return NotFound();

                if (operation.Sign)
                {
                    operation.Project.TotalCreditBalance -= operation.Amount;
                    contractor.TotalCreditBalance -= operation.Amount;
                }
                else
                {
                    operation.Project.TotalDebitBalance -= operation.Amount;
                    contractor.TotalDebitBalance -= operation.Amount;
                }

            }
            else if(model.Type == "Suppliers")
            {
                var supplier = await _db.Suppliers.FindAsync(model.UserId);
                if(supplier == null) return NotFound();

                if (operation.Sign)
                {
                    operation.Project.TotalCreditBalance -= operation.Amount;
                    supplier.TotalCreditBalance -= operation.Amount;
                }
                else
                {
                    operation.Project.TotalDebitBalance -= operation.Amount;
                    supplier.TotalDebitBalance -= operation.Amount;
                }

            }
            else
            {
                var client = await _db.Clients.FindAsync(model.UserId);
                if(client == null) return NotFound();

                if (operation.Sign)
                {
                    operation.Project.TotalCreditBalance -= operation.Amount;
                    client.TotalCreditBalance -= operation.Amount;
                }
                else
                {
                    operation.Project.TotalDebitBalance -= operation.Amount;
                    client.TotalDebitBalance -= operation.Amount;
                }

            }
            
            System.IO.File.Delete(operation.FilePath);
            var dashboardData = await _db.DashboardData.FirstOrDefaultAsync();
            dashboardData.Operations -= 1;
            _db.Remove(operation);
            await _db.SaveChangesAsync();

            return Ok("operation Deleted Succesffully");
        }

        public async Task createOperationTypesSelectList()
        {
            var operations = await _db.OperationTypes.ToListAsync();
            SelectList listItems = new SelectList(operations, "Id", "Name", 1);
            ViewBag.OperationList = listItems;
        }
    }
}


