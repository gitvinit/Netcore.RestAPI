using Netcore.RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netcore.RestAPI.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployee(string name);
        Task Create(Employee employee);
        Task<bool> Update(Employee employee);
        Task<bool> Delete(string name);
    }
}
