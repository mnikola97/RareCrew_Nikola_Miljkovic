using Microsoft.AspNetCore.Mvc;
using System.Resources;
using RareCrew_Nikola_Miljkovic.Models;

namespace RareCrew_Nikola_Miljkovic.Controllers
{
    public class EmployeeController : Controller
    {

        private EmployeeRepository employeeRepository;

        public EmployeeController()
        {
            employeeRepository = new EmployeeRepository();
        }

        public async Task<ActionResult> Index()
        {
            var employees= await employeeRepository.GetData();
            return View(employees);
        }
    }
}
