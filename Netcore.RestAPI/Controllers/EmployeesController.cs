using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Netcore.RestAPI.Models;
using Netcore.RestAPI.Repository;

namespace Netcore.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET api/employees
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await _employeeRepository.GetAllEmployeesAsync());
        }

        // GET api/employees/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var employee = await _employeeRepository.GetEmployee(name);
            if (employee == null)
                return new NotFoundResult();
            return new ObjectResult(employee);
        }

        // POST api/employees
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            await _employeeRepository.Create(employee);
            return new OkObjectResult(employee);
        }

        // PUT api/employees/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody] Employee employee)
        {
            var employeeFromDb = await _employeeRepository.GetEmployee(name);
            if (employeeFromDb == null)
                return new NotFoundResult();
            employee.Id = employeeFromDb.Id;
            await _employeeRepository.Update(employee);
            return new OkObjectResult(employee);
        }

        // DELETE api/employees/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var employeeFromDb = await _employeeRepository.GetEmployee(name);
            if (employeeFromDb == null)
                return new NotFoundResult();
            await _employeeRepository.Delete(name);
            return new OkResult();
        }
    }
}
