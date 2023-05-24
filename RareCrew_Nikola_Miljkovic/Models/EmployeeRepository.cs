using Microsoft.AspNetCore.Hosting.Server;
using System.Drawing;
using System.Drawing.Printing;
using System.Web;
using System.Web.UI.DataVisualization.Charting;

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
            GenerateChart(employeesData);
            return employeesData.OrderByDescending(x => x.WorkHours).ToList();
        }
        public void GenerateChart(List<EmployeeData> employeeData)
        {
            var chart = new Chart();

            var chartArea = new ChartArea();
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas.Add(chartArea);


            var series = new Series();
            series.ChartType = SeriesChartType.Pie;
            var employeeHoursSum = employeeData.Sum(x => x.WorkHours);
            foreach (var employee in employeeData)
            {
                series.Points.AddXY(employee.EmployeeName==null?"No Name":employee.EmployeeName, Math.Round(employee.WorkHours / employeeHoursSum * 100,2));
            }
            series.Label = "#PERCENT{P2}\n#VALX";

            chart.Series.Add(series);
            chart.SaveImage("chart.png", ChartImageFormat.Png);
        }

    }
}
