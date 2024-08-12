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
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class WorkflowVersionAsyncController : ControllerBase
    {
        private readonly WorkflowVersionServiceAsync<WorkflowVersionViewModel, WorkflowVersion> _workflowVersionService;
        public WorkflowVersionAsyncController(WorkflowVersionServiceAsync<WorkflowVersionViewModel, WorkflowVersion> workflowVersionServiceAsync)
        {
            _workflowVersionService = workflowVersionServiceAsync;
        }

        //get all
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _workflowVersionService.GetAll();
            return Ok(items);
        }

        
        //get one
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _workflowVersionService.GetOne(id);
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
        public async Task<IActionResult> Create([FromBody] WorkflowVersionViewModel workflowVersion)
        {
            if (workflowVersion == null)
                return BadRequest();

            var id = await _workflowVersionService.Add(workflowVersion);
            return Created($"api/WorkflowVersion/{id}", id);  //HTTP201 Resource created
        }

        //update
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorkflowVersionViewModel workflowVersion)
        {
            if (workflowVersion == null || workflowVersion.Id != id)
                return BadRequest();

            int retVal = await _workflowVersionService.Update(workflowVersion);
            if (retVal == 0)
                return StatusCode(304);  //Not Modified
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
            else
                return Accepted(workflowVersion);
        }

        //delete
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int retVal = await _workflowVersionService.Remove(id);
            if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }

    }}
