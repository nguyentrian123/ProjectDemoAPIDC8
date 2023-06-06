using AutoMapper;
using Contracts.Logger;
using Contracts.Repository;
using Entities.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TestWebAPI.Extension;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {

       
        private readonly ICompanyRepository _companyRepository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository,
                                ILoggerManager logger, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _companyRepository.GetAllCompanies(trackChanges: false);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companiesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _companyRepository.GetCompany(id, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                 throw new KeyNotFoundException($"Company with id:{id}  doesn't exist in the database."); 
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
            }
        }

    }
}
