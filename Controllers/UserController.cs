using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.AppData;
using Todo.Model;
using Todo.Request;
using Todo.Response;
using Todo.Service;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthService> _logger;
        private readonly AppDBContext _dbContext;

        public UserController(IUserService _userService, ILogger<AuthService> _logger, AppDBContext _dbContext)
        {
            this._userService = _userService;
            this._logger = _logger;
            this._dbContext = _dbContext;
        }

        [HttpPost("createToDo")]
        public async Task<IActionResult> CreateToDo(ToDoPostRequest request)
        {
            if (request == null)
            {
                return BadRequest("Bad Request");
            }

            var result = await _userService.CreateToDo(request);

            if (result.isCreated)
            {
                return Ok(result.message);
            }
            return BadRequest(result.message);
        }

        [HttpGet("getAllToDoItems/{email}")]
        public async Task<ActionResult<ItemsResponse>> GetAllToDoItemsAsync(string email)
        {
            var result = await _userService.GetAllToDoItemsAsync(email);
            if (result.Success)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getItemById/{id}/{email}")]
        public async Task<ActionResult<ItemResponse>> GetToDoItemById(int id, string email)
        {
            var result = await _userService.GetToDoItemById(id, email);
            if (result.Success )
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Message);
        }


        [HttpPut("updateToDo")]
        public async Task<IActionResult> UpdateToDo(UpdateToDoRequest request)
        {
            var result = await _userService.UpdateToDo(request);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("deleteToDoItem/{id}/{email}")]
        public async Task<IActionResult> DeleteToDoItem(int id, string email)
        {
            var result = await _userService.DeleteToDoItem( id, email);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
