using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mVCDemoDbContext;
        public EmployeesController(MVCDemoDbContext mVCDemoDbContext)
        {
            this.mVCDemoDbContext = mVCDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mVCDemoDbContext.Employess.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department
            };

            await mVCDemoDbContext.Employess.AddAsync(employee);
            await mVCDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mVCDemoDbContext.Employess.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null) 
            {
                var viewModel = new UpdateEmployeeViewmodel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };

                return await Task.Run(() => View("View",viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewmodel model)
        {
            var employee = await mVCDemoDbContext.Employess.FindAsync(model.Id);

            if(employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await mVCDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }
            
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewmodel model)
        {
            var employee = await mVCDemoDbContext.Employess.FindAsync(model.Id);

            if(employee != null)
            {
                mVCDemoDbContext.Employess.Remove(employee);
                await mVCDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }
    }
}
