using DAL;
using System.Reflection;

namespace BusinessLogic
{
    public class EmployeeBL : IEmployeeBL
    {
        IEmployeeDAL employeeDAL;
        public EmployeeBL(IEmployeeDAL employeeDAL)
        {
            this.employeeDAL = employeeDAL;
        }

        async Task<List<Models.EmployeeData>> IEmployeeBL.GetData()
        {
            List<DAL.Models.Employee> employees;
            List<Models.EmployeeData> employeesData;
            employees = await employeeDAL.GetData();

            employees.ForEach(x => x.WorkHours = (x.EndTimeUtc - x.StarTimeUtc).TotalHours);

            var groupData = employees.GroupBy(e => e.EmployeeName).Select(g => new Models.EmployeeData
            {
                EmployeeName = g.Key,
                WorkHours = g.Sum(s => s.WorkHours)
            });

            employeesData = groupData.ToList();
            return employeesData.OrderByDescending(x => x.WorkHours).ToList();
        }
    }
}