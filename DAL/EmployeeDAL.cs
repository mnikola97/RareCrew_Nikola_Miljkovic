using DAL.Models;
using System.Net.Http.Json;
using System.Resources;

namespace DAL
{
    public class EmployeeDAL:IEmployeeDAL
    {   
        public EmployeeDAL() { }
        public async Task<List<Employee>> GetData() {

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Resources.EmployeeApiEndpoint);
                response.EnsureSuccessStatusCode();
                if (response.Content is null)
                    throw new Exception("Employee API je vration null");
                
                else
                {
                   return await response.Content.ReadFromJsonAsync<List<Employee>>();
                }
                
            }
        }
    }
}
               
