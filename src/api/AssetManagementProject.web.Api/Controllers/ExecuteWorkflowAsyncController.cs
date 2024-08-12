using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagementProject.web.Domain;
using WorkflowCore.Interface;
using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Services.DefinitionStorage;
using Newtonsoft.Json;
using System.Dynamic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Xamun.Workflows.Action.Model;
using Xamun.Workflows.Action.HTTP;
using Xamun.Workflows.Action.WorkflowResult;
using AssetManagementProject.web.Entity;
using AssetManagementProject.web.Domain.Service;

namespace AssetManagementProject.web.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExecuteWorkflowAsyncController : ControllerBase
    {
        //private readonly ISendHttpRequestServiceAsync _sendHttpRequestServiceAsync;
        private readonly IServiceProvider _serviceProvider;
        private readonly WorkflowVersionService<WorkflowVersionViewModel, WorkflowVersion> _workflowVersionService;

        public ExecuteWorkflowAsyncController(IServiceProvider serviceProvider, WorkflowVersionService<WorkflowVersionViewModel, WorkflowVersion> workflowVersionService)
        {
            //_sendHttpRequestServiceAsync = sendHttpRequestServiceAsync;
            _serviceProvider = serviceProvider;
            _workflowVersionService = workflowVersionService;
        }


        //[AllowAnonymous]
        //[HttpPost("ExecuteWorkflow")]
        //public async Task<IActionResult> ExecuteWorkflow([FromBody] SendHttpRequestViewModel req)
        //{
        //    try
        //    {
        //        var response = await _sendHttpRequestServiceAsync.SendHttpRequest(req);
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        [Authorize]
        [HttpPost("ProcessWorkflow")]
        public async Task<IActionResult> ProcessWorkflow([FromBody] ProcessWorkflowViewModel processWorkflow)
        {
            return await HandleWorkflow(processWorkflow);
        }
        [Authorize]
        [HttpPost("ProcessWorkflow/{replaceToken}")]
        public async Task<IActionResult> ProcessWorkflow([FromBody] ProcessWorkflowViewModel processWorkflow, string replaceToken)
        {
            processWorkflow.WorkflowObjectParamJson = processWorkflow.WorkflowObjectParamJson.Replace("{{REPLACE_ACCESS_TOKEN}}", replaceToken);
            return await HandleWorkflow(processWorkflow);
        }
        private async Task<IActionResult> HandleWorkflow(ProcessWorkflowViewModel processWorkflow)
        {
            var workflowVersion = GetUpdateVersionId();

            var workflowDefinition = UpdateJsonWorkflowDefinition(workflowVersion, processWorkflow.WorkflowDefinitionJson);

            var host = _serviceProvider.GetService<IWorkflowHost>();
            var loader = _serviceProvider.GetService<IDefinitionLoader>();
            var def = loader.LoadDefinition(workflowDefinition, Deserializers.Json);

            dynamic param = JsonConvert.DeserializeObject<ExpandoObject>(processWorkflow.WorkflowObjectParamJson);

            int stepId = 1;
            var workflowId = string.Empty;
            foreach (var steps in def.Steps.ToList())
            {
                if (stepId == 1)
                {
                    host.StartAsync(CancellationToken.None).Wait();
                    workflowId = await host.StartWorkflow(def.Id, param);
                    host.StopAsync(CancellationToken.None).Wait();
                }
                else
                {
                    host.StartAsync(CancellationToken.None).Wait();
                    await host.ResumeWorkflow(workflowId);
                    host.StopAsync(CancellationToken.None).Wait();

                }
                stepId++;
            }

            ResultState state = ResultState.GetState();
            var resObj = new RequestResponseViewModel
            {
                RequestType = state.RequestType,
                workflowResult = state.workflowResult
            };

            await host.TerminateWorkflow(workflowId);
            return Ok(resObj);
        }

        private string UpdateJsonWorkflowDefinition(int workflowVersion, string workflowDefinitionJson)
        {
            var workflowJson = workflowDefinitionJson;
            var unscapeJson = Regex.Unescape(workflowJson);
            var deserializeJsonList = JsonConvert.DeserializeObject<ExpandoObject>(unscapeJson);
            var map = (IDictionary<string, object>)deserializeJsonList;
            if (map.ContainsKey("Version"))
            {
                map["Version"] = workflowVersion;
            }

            var serializeJson = JsonConvert.SerializeObject(deserializeJsonList);
            workflowDefinitionJson = serializeJson;

            return workflowDefinitionJson;

        }

        private int GetUpdateVersionId()
        {

            var workflowList = _workflowVersionService.GetAll();

            int workflowVersion = 1;
            if (workflowList.Count() <= 0)
            {
                WorkflowVersionViewModel workflowmodel = new WorkflowVersionViewModel();
                workflowmodel.Version = workflowVersion;

                _workflowVersionService.Add(workflowmodel);
            }
            else
            {
                var workflow = workflowList.LastOrDefault();
                workflow.Version++;
                workflowVersion = workflow.Version;

                var newWorkflowVersion = new WorkflowVersionViewModel();
                newWorkflowVersion.Version = workflowVersion;

                _workflowVersionService.Add(newWorkflowVersion);
            }

            return workflowVersion;
        }


    }
}
