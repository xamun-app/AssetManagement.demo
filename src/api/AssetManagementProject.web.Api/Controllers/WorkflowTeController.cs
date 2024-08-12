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
    public class WorkflowTeController : ControllerBase
    {
        private readonly WorkflowTeService<WorkflowTeViewModel, WorkflowTe> _workflowteService;
        public WorkflowTeController(WorkflowTeService<WorkflowTeViewModel, WorkflowTe> workflowteService)
        {
            _workflowteService = workflowteService;
        }

        //get all
        [Authorize]
        [HttpGet]
        public IEnumerable<WorkflowTeViewModel> GetAll()
        {
            var items = _workflowteService.GetAll();
            return items;
        }

        
        //get one
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _workflowteService.GetOne(id);
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
        public IActionResult Create([FromBody] WorkflowTeViewModel workflowTe)
        {
            if (workflowTe == null)
                return BadRequest();

            var id = _workflowteService.Add(workflowTe);
            return Created($"api/WorkflowTe/{id}", id);  //HTTP201 Resource created
        }

        //update
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] WorkflowTeViewModel workflowTe)
        {
            if (workflowTe == null || workflowTe.Id != id)
                return BadRequest();

            int retVal = _workflowteService.Update(workflowTe);
            if (retVal == 0)
                return StatusCode(304);  //Not Modified
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
            else
                return Accepted(workflowTe);
        }

        //delete
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int retVal = _workflowteService.Remove(id);
            if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }

    }}
