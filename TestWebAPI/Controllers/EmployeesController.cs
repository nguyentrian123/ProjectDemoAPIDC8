using AutoMapper;
using Contracts.Logger;
using Contracts.Repository;
using Entities.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace TestWebAPI.Controllers
{
    [Route("api/companies/{companyId}/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public EmployeesController(IEmployeeRepository employeeRepository,
                                ICompanyRepository companyRepository,
                                ILoggerManager logger, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var company = _companyRepository.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                throw new KeyNotFoundException($"Company with id:{companyId}  doesn't exist in the database.");
            }
            var employeesFromDb = _employeeRepository.GetEmployees(companyId,trackChanges: false);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return Ok(employeesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = _companyRepository.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                throw new KeyNotFoundException($"Company with id:{companyId}  doesn't exist in the database."); ;
            }
            var employeeDb = _employeeRepository.GetEmployee(companyId, id, trackChanges:false);
            if (employeeDb == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                throw new KeyNotFoundException($"Employee with id:{id}  doesn't exist in the database.");
            }
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return Ok(employee);
        }

    }
}
