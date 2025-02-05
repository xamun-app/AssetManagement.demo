﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class UserAsyncController : ControllerBase
    {
        private readonly UserServiceAsync<UserViewModel, User> _userServiceAsync;
        public UserAsyncController(UserServiceAsync<UserViewModel, User> userServiceAsync)
        {
            _userServiceAsync = userServiceAsync;
        }


        //get all
        [Authorize]
        [HttpGet]
        //[Attributes.DDosAttackProtected]
        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            var items = await _userServiceAsync.GetAll();
            return items;
        }

        //get by predicate example
        //get all active by username
        [Authorize]
        [HttpGet("GetActiveByFirstName/{firstname}")]
        public async Task<IActionResult> GetActiveByFirstName(string firstname)
        {
            var items = await _userServiceAsync.Get(a => a.IsActive && a.FirstName == firstname);
            return Ok(items);
        }

        //get one
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _userServiceAsync.GetOne(id);
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
        public async Task<IActionResult> Create([FromBody] UserViewModel user)
        {
            if (user == null)
                return BadRequest();

            var id = await _userServiceAsync.Add(user);
            return Created($"api/User/{id}", id);  //HTTP201 Resource created
        }

        //update
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserViewModel user)
        {
            if (user == null || user.Id != id)
                return BadRequest();

            int retVal = await _userServiceAsync.Update(user);
            if (retVal == 0)
                return StatusCode(304);  //Not Modified
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
            else
                return Accepted(user);
        }

        //delete
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int retVal = await _userServiceAsync.Remove(id);
            if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }

        //use READ stored procedure example
        [Authorize]
        [HttpGet("GetUsersByName/{firstname}/{lastname}")]
        public async Task<IActionResult> GetUsersByName(string firstname, string lastname)
        {
            var items = await _userServiceAsync.GetUsersByName(firstname, lastname);
            return Ok(items);
        }

        //use CDUD stored procedure example
        [Authorize(Policy = AuthorizationRolesConstants.Administrator)]
        [HttpGet("UpdateEmailbyUsername/{username}/{email}")]
        public async Task<IActionResult> UpdateEmailbyUsername(string username, string email)
        {
            int id = await _userServiceAsync.UpdateEmailByUsername(username, email);
            return Ok(id);
        }


    }
}


