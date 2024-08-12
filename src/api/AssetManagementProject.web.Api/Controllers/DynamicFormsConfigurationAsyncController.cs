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
    public class DynamicFormsConfigurationAsyncController : ControllerBase
    {
        private readonly DynamicFormsConfigurationServiceAsync<DynamicFormsConfigurationViewModel, DynamicFormsConfiguration> _dynamicformsconfigurationServiceAsync;
        public DynamicFormsConfigurationAsyncController(DynamicFormsConfigurationServiceAsync<DynamicFormsConfigurationViewModel, DynamicFormsConfiguration> dynamicformsconfigurationServiceAsync)
        {
            _dynamicformsconfigurationServiceAsync = dynamicformsconfigurationServiceAsync;
        }


        //get all
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _dynamicformsconfigurationServiceAsync.GetAll();
            return Ok(items);
        }

        [Authorize]
        [HttpGet("GetByPage")]
        public async Task<IActionResult> GetByPage(int pageIndex = 0, int pageSize = 10,
             string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null)
        {
            //Log.Information("Log: Log.Information");
            //Log.Warning("Log: Log.Warning");
            //Log.Error("Log: Log.Error");
            //Log.Fatal("Log: Log.Fatal");
            var page = await _dynamicformsconfigurationServiceAsync.GetPage(pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
            return Ok(page);
        }

        //get one
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _dynamicformsconfigurationServiceAsync.GetOne(id);
            if (item == null)
            {
                Log.Error("GetById({ ID}) NOT FOUND", id);
                return NotFound();
            }

            return Ok(item);
        }

        //get by name
        [Authorize]
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var formConfigs = await _dynamicformsconfigurationServiceAsync.Get(a => a.Name == name &&
                a.IsActive == true &&
                a.IsDeleted == false);
            if (formConfigs.Count() == 0)
            {
                Log.Error("GetByName({ NAME }) NOT FOUND", name);
                return NotFound();
            }

            var formConfig = formConfigs.OrderByDescending(a => a.Version).FirstOrDefault();

            return Ok(formConfig);
        }

        //get by name and version
        [Authorize]
        [HttpGet("GetByNameAndVersion/{name}/{version}")]
        public async Task<IActionResult> GetByNameAndVersion(string name, double version)
        {
            var formConfig = await _dynamicformsconfigurationServiceAsync.Get(a => a.Name == name &&
                a.IsActive == true &&
                a.IsDeleted == false &&
                a.Version == version);
            if (formConfig == null)
            {
                Log.Error("GetByNameAndVersion({ NAME }, { VERSION}) NOT FOUND", name, version);
                return NotFound();
            }

            return Ok(formConfig);
        }

        //add
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DynamicFormsConfigurationViewModel dynamicformsconfiguration)
        {
            if (dynamicformsconfiguration == null)
                return BadRequest();

            var id = await _dynamicformsconfigurationServiceAsync.Add(dynamicformsconfiguration);
            return Created($"api/DynamicFormsConfiguration/{id}", id);  //HTTP201 Resource created
        }

        //update
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DynamicFormsConfigurationViewModel dynamicformsconfiguration)
        {
            if (dynamicformsconfiguration == null || dynamicformsconfiguration.Id != id)
                return BadRequest();

            var retVal = await _dynamicformsconfigurationServiceAsync.Update(dynamicformsconfiguration);
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
        public async Task<IActionResult> Delete(int id)
        {
            var retVal = await _dynamicformsconfigurationServiceAsync.Remove(id);
            if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }
    }
}
