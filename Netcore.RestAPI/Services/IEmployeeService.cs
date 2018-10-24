using Netcore.RestAPI.Models;
using Netcore.RestAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netcore.RestAPI.Repository
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployee(string username);
        Task Create(Employee employee);
        Task<bool> Update(Employee employee);
        Task<bool> Delete(string username);
        Task<EmployeeViewModel> Authenticate(string username, string password);
    }
}
