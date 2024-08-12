using System.Collections.Generic;
using System.Linq;
using AssetManagementProject.web.Api.Configuration;
using AssetManagementProject.web.Domain;
using AssetManagementProject.web.Domain.Service;
using AssetManagementProject.web.Entity;
using AssetManagementProject.web.Entity.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AssetManagementProject.web.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService<AccountViewModel, Account> _accountService;
        public AccountController(AccountService<AccountViewModel, Account> accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<AccountViewModel> GetAll()
        {
            //Log.Information("Log: Log.Information");
            //Log.Warning("Log: Log.Warning");
            //Log.Error("Log: Log.Error");
            //Log.Fatal("Log: Log.Fatal");
            var test = _accountService.DoNothing();
            var items = _accountService.GetAll();
            return items;
        }


        //get all
        [Authorize]
        [HttpGet("GetByPage")]
        public PageResult<AccountViewModel> GetByPage(int pageIndex = 0, int pageSize = 10,
            string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null)
        {
            //Log.Information("Log: Log.Information");
            //Log.Warning("Log: Log.Warning");
            //Log.Error("Log: Log.Error");
            //Log.Fatal("Log: Log.Fatal");
            var test = _accountService.DoNothing();
            var items = _accountService.GetPage(pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
            return items;
        }

        //get by predicate example
        //get all active by name
        [Authorize]
        [HttpGet("GetActiveByName/{name}")]
        public IActionResult GetActiveByName(string name)
        {
            var items = _accountService.Get(a => a.IsActive && a.Name == name);
            return Ok(items);
        }

        //get one
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _accountService.GetOne(id);
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
        public IActionResult Create([FromBody] AccountViewModel account)
        {
            if (account == null)
                return BadRequest();

            var id = _accountService.Add(account);
            return Created($"api/Account/{id}", id);  //HTTP201 Resource created
        }

        //update
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AccountViewModel account)
        {
            if (account == null || account.Id != id)
                return BadRequest();

            int retVal = _accountService.Update(account);
            if (retVal == 0)
                return StatusCode(304);  //Not Modified
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
            else
                return Accepted(account);
        }

        //delete 
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int retVal = _accountService.Remove(id);
            if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }
    }
}


