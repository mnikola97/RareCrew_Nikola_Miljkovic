namespace BusinessLogic
{
    public interface IEmployeeBL
    {

        Task<List<Models.EmployeeData>> GetData();
    }
}