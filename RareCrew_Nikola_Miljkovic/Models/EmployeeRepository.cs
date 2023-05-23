using System.Drawing;
namespace RareCrew_Nikola_Miljkovic.Models
{
    public class EmployeeRepository
    {
        // Simulated data source
        private List<Employee> employees;

        public EmployeeRepository()
        {
            employees = new List<Employee>();
        }

        public async Task<List<EmployeeData>> GetData()
        {

            List<EmployeeData> employeesData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Resources.EmployeeApiEndpoint);
                response.EnsureSuccessStatusCode();
                if (response.Content is null)
                    throw new Exception("Employee API je vration null");

                else
                {
                    employees = await response.Content.ReadFromJsonAsync<List<Employee>>();
                }
            }
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
