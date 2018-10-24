using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Netcore.RestAPI.Models;
using Netcore.RestAPI.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Netcore.RestAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET api/employees
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await _employeeService.GetAllEmployeesAsync());
        }

        // GET api/employees/name
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string username)
        {
            var employee = await _employeeService.GetEmployee(username);
            if (employee == null)
                return new NotFoundResult();
            return new ObjectResult(employee);
        }

        // POST api/employees
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            await _employeeService.Create(employee);
            return new OkObjectResult(employee);
        }

        // PUT api/employees/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody] Employee employee)
        {
            var employeeFromDb = await _employeeService.GetEmployee(name);
            if (employeeFromDb == null)
                return new NotFoundResult();
            employee.Id = employeeFromDb.Id;
            await _employeeService.Update(employee);
            return new OkObjectResult(employee);
        }

        // DELETE api/employees/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var employeeFromDb = await _employeeService.GetEmployee(name);
            if (employeeFromDb == null)
                return new NotFoundResult();
            await _employeeService.Delete(name);
            return new OkResult();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]Employee employee)
        {
            var user = await _employeeService.Authenticate(employee.Username, employee.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }


    }
}
