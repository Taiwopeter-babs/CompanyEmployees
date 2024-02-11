﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiController]
    [Route("api/v0/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompaniesController(IServiceManager service) =>
            _service = service;

        [HttpGet]
        public IActionResult GetCompanies()
        {
            try
            {
                var companies = _service.
                          CompanyService.
                          GetAllCompanies(trackChanges: false);

                return Ok(companies);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}