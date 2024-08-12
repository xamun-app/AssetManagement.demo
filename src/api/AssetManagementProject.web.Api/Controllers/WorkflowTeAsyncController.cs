using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AssetManagementProject.web.Domain.Service;
using AssetManagementProject.web.Domain;
using AssetManagementProject.web.Entity;
using AssetManagementProject.web.Api.Configuration;

namespace AssetManagementProject.web.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class WorkflowTeAsyncController: ControllerBase
    {

        private readonly WorkflowTeServiceAsync<WorkflowTeViewModel, WorkflowTe> _workflowteServiceAsync;
        private readonly IServiceProvider _serviceProvider;
        public WorkflowTeAsyncController(WorkflowTeServiceAsync<WorkflowTeViewModel, WorkflowTe> workflowteServiceAsync, IServiceProvider serviceProvider)
        {
            _workflowteServiceAsync = workflowteServiceAsync;
            _serviceProvider = serviceProvider;
        }


        //get all
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _workflowteServiceAsync.GetAll();
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
            var page = await _workflowteServiceAsync.GetPage(pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
            return Ok(page);
        }

        //get one
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _workflowteServiceAsync.GetOne(id);
            if (item == null)
            {
                Log.Error("GetById({ ID}) NOT FOUND", id);
                return NotFound();
            }

            return Ok(item);
        }

        //add
        [Authorize(Policy = AuthorizationRolesConstants.IDAScript)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkflowTeViewModel workflowte)
        {
            if (workflowte == null)
                return BadRequest();

            var id = await _workflowteServiceAsync.Add(workflowte);
            return Created($"api/WorkflowTe/{id}", id);  //HTTP201 Resource created
        }

        //update
        [Authorize(Policy = AuthorizationRolesConstants.IDAScript)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorkflowTeViewModel workflowte)
        {
            if (workflowte == null || workflowte.Id != id)
                return BadRequest();

	    var retVal = await _workflowteServiceAsync.Update(workflowte);
            if (retVal == 0)
				return StatusCode(304);  //Not Modified
            else if (retVal == - 1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
            else
                return Accepted(workflowte);
        }


        //delete
        [Authorize(Policy = AuthorizationRolesConstants.IDAScript)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
	    var retVal = await _workflowteServiceAsync.Remove(id);
	    if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }

        [Authorize(Policy = AuthorizationRolesConstants.IDAScript)]
        [HttpDelete()]
        public async Task<IActionResult> DeleteAll()
        {
	    var retVal = await _workflowteServiceAsync.DeleteAll();
	    if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }

        [Authorize(Policy = AuthorizationRolesConstants.IDAScript)]
        [HttpDelete("ApplicationWorkflow/{appWorkflowId}")]
        public async Task<IActionResult> DeleteAllByAppWorkflowId(int appWorkflowId)
        {
	        var retVal = await _workflowteServiceAsync.DeleteAllByAppWorkflowId(appWorkflowId);

            if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");
	         
            return NoContent();   	     //No Content 204
        }
        [Authorize]
        [HttpGet("GetWorkflowByAppWorkflowId/{appWorkflowId}")]
        public async Task<IActionResult> GetWorkflowByAppWorkflowId(int appWorkflowId)
        {
            var workflowList = await _workflowteServiceAsync.Get(x => x.ApplicationWorkflowId.Equals(appWorkflowId) && x.IsDeleted.Equals(false));

            return Ok(workflowList);
        }

        [AllowAnonymous]
        [HttpGet("GetWorkflowByName/{workflowName}")]
        public async Task<IActionResult> GetWorkflowByName(string workflowName)
        {
            var workflow = await _workflowteServiceAsync.Get(x => x.Name.ToLower().Equals(workflowName.ToLower()));
            return Ok(workflow);
        }

        [AllowAnonymous]
        [HttpGet("GetWorkflowVersion")]
        public async Task<IActionResult> VersionId()
        {
            var workflowJsonList = await _workflowteServiceAsync.GetAll();
            int newVersionId = 0;
            if (workflowJsonList.Count() <= 0)
            {
                newVersionId++;
                return Ok(newVersionId);
            }

            foreach(var workflow in workflowJsonList)
            {
                var workflowJson = workflow.WorkflowCoreJson;
                var unscapeJson = Regex.Unescape(workflowJson);
                var deserializeJsonList = JsonConvert.DeserializeObject<ExpandoObject>(unscapeJson);

                foreach (var deserializeJson in deserializeJsonList)
                {
                    if (deserializeJson.Key.Equals("Version"))
                    {
                        int versionId = Convert.ToInt32(deserializeJson.Value);
                        if(versionId > newVersionId)
                        {
                            newVersionId = ++versionId;
                        }
                    }

                }
            }
            
            return Ok(newVersionId);
        }

        [Authorize]
        [HttpPost("PostWorkflowByList")]
        public async Task<IActionResult> PostWorkflowBylist([FromBody] ListWorkflowTeViewModel listOfWorkflowTeViewModel)
        {
            var listOfIds = new List<int>();

            //Check if null
            if(listOfWorkflowTeViewModel == null)
                return BadRequest();

            //Iterate insertion
            foreach(var workflow in listOfWorkflowTeViewModel.WorkflowTeViewModels)
            {
                var id = await _workflowteServiceAsync.Add(workflow);
                listOfIds.Add(id);
            }
            return Ok(new { Ids = listOfIds }); //HTTP 200 Workflows Created
        }

        [AllowAnonymous]
        [HttpGet("GetWorkflowByNameAndModuleName/{workflowName}/{moduleName}")]
        public async Task<IActionResult> GetWorkflowByModuleNameAndWorkflowName(string workflowName, string moduleName)
        {
            var workflowList = await _workflowteServiceAsync.Get(workflow => workflow.Name.ToLower().Equals(workflowName.ToLower()) && workflow.ModuleName.ToLower().Equals(moduleName.ToLower()) && workflow.IsDeleted == false);
            var workflow = workflowList.FirstOrDefault();
            if (workflow == null)
            {
                Log.Error("GetWorkflowByNameAndModuleName({ workflowName}, {moduleName}) NOT FOUND", workflowName, moduleName);
                return NotFound("Workflow not found");
            }
            return Ok(workflow);
        }

        [Authorize]
        [HttpPut("PutWorkflowByList")]
        public async Task<IActionResult> PutWorkflowByList([FromBody] ListWorkflowTeViewModel listOfWorkflowTeViewModel)
        {
            foreach(var workflow in listOfWorkflowTeViewModel.WorkflowTeViewModels)
            {
                var retVal = await _workflowteServiceAsync.Update(workflow);
                if (retVal == 0)
                    return StatusCode(304);  //Not Modified
                else if (retVal == -1)
                    return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
            }
            return Accepted(listOfWorkflowTeViewModel);
        }
    }
}
