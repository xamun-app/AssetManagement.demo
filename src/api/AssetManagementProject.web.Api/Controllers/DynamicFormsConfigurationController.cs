using AssetManagementProject.web.Api.Configuration;
using AssetManagementProject.web.Domain;
using AssetManagementProject.web.Domain.Service;
using AssetManagementProject.web.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementProject.web.Api.Controllers
{
    /// <summary>
    ///    
    /// A DynamicFormsConfiguration controller
    ///
    /// MANUAL UPDATES REQUIRED!
    /// Update API version and uncomment route version declaration if required 
    ///       
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DynamicFormsConfigurationController : ControllerBase
    {
        private readonly DynamicFormsConfigurationService<DynamicFormsConfigurationViewModel, DynamicFormsConfiguration> _dynamicformsconfigurationService;
        public DynamicFormsConfigurationController(DynamicFormsConfigurationService<DynamicFormsConfigurationViewModel, DynamicFormsConfiguration> dynamicformsconfigurationService)
        {
            _dynamicformsconfigurationService = dynamicformsconfigurationService;
        }



        //get all
        [Authorize]
        [HttpGet]
        public IEnumerable<DynamicFormsConfigurationViewModel> GetAll()
        {
            //Serilog log examples 
            //Log.Information("Log: Log.Information");
            //Log.Warning("Log: Log.Warning");
            //Log.Error("Log: Log.Error");
            //Log.Fatal("Log: Log.Fatal");
            var items = _dynamicformsconfigurationService.GetAll();
            return items;
        }

        [Authorize]
        [HttpGet("GetByPage")]

        public PageResult<DynamicFormsConfigurationViewModel> GetByPage(int pageIndex = 0, int pageSize = 10,
             string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null)
        {
            //Log.Information("Log: Log.Information");
            //Log.Warning("Log: Log.Warning");
            //Log.Error("Log: Log.Error");
            //Log.Fatal("Log: Log.Fatal");

            var items = _dynamicformsconfigurationService.GetPage(pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
            return items;

        }


        //get one
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _dynamicformsconfigurationService.GetOne(id);
            if (item == null)
            {
                Log.Error("GetById({ ID}) NOT FOUND", id);
                return NotFound();
            }

            return Ok(item);
        }

        //add
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpPost]
        public IActionResult Create([FromBody] DynamicFormsConfigurationViewModel dynamicformsconfiguration)
        {
            if (dynamicformsconfiguration == null)
                return BadRequest();

            var id = _dynamicformsconfigurationService.Add(dynamicformsconfiguration);
            return Created($"api/DynamicFormsConfiguration/{id}", id);  //HTTP201 Resource created
        }

        //update
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DynamicFormsConfigurationViewModel dynamicformsconfiguration)
        {
            if (dynamicformsconfiguration == null || dynamicformsconfiguration.Id != id)
                return BadRequest();

            var retVal = _dynamicformsconfigurationService.Update(dynamicformsconfiguration);
            if (retVal == 0)
                return StatusCode(304);  //Not Modified
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
            else
                return Accepted(dynamicformsconfiguration);
        }

        //delete 
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var retVal = _dynamicformsconfigurationService.Remove(id);
            if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }

    }
}
