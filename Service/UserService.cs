using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.AppData;
using Todo.Model;
using Todo.Request;
using Todo.Response;

namespace Todo.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly AppDBContext _dbContext;

        public UserService(UserManager<IdentityUser> _userManager, ILogger<AuthService> _logger, AppDBContext _dbContext)
        {
            this._userManager = _userManager;
            this._logger = _logger;
            this._dbContext = _dbContext;
        }

        public async Task<(bool isCreated, string message)> CreateToDo(ToDoPostRequest request)
        {
            try
            {
                var userFound = await _userManager
                                   .FindByEmailAsync(request.UserEmail);
                _logger.LogInformation($" userFound: {userFound}");

                if (userFound is null)
                {
                    return (false, "User is not found.");
                }

                var toDoItem = new ToDo
                {
                    Title = request.Title,
                    IsCompleted = request.IsCompleted,
                    UserId = userFound.Id,
                    User = (User)userFound
                };

                await _dbContext.ToDoItems.AddAsync(toDoItem);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"ToDo item created for userFound: {userFound.Email}");
                return (true, "Todo was created");

            } catch (Exception e){
                return (false, "Erorr: " + e);
            }
        }

        public class ToDoItemsResult
        {
            public ItemsResponse Response { get; set; } = new ItemsResponse();
            public string Message { get; set; } = string.Empty;
            public bool Success { get; set; } = false;
        }

        public async Task<ToDoItemsResult> GetAllToDoItemsAsync(string email)
        {
            try
            {
                var userWithToDoItems = await _dbContext.Users
                    .Include(u => u.ToDoItems)
                    .FirstOrDefaultAsync(u => u.Email.Equals(email));

                if (userWithToDoItems == null)
                {
                    return new ToDoItemsResult { Success = false, Message = "User not found." };
                }

                ItemsResponse response = new ItemsResponse();

                userWithToDoItems.ToDoItems.ForEach(toDoItem => {
                    response.ItemDatas.Add(new ItemData()
                    {
                        Id = toDoItem.Id,
                        IsCompleted = toDoItem.IsCompleted,
                        Title = toDoItem.Title,
                    });
                }
                );

                return new ToDoItemsResult { Success = true, Response = response, Message = "Success" };
            }
            catch (Exception e)
            {
                return new ToDoItemsResult { Success = false, Message = $"Error: {e.Message}" };
            }
        }

        public class ToDoItemResult
        {
            public ItemResponse Response { get; set; } = new ItemResponse();
            public string Message { get; set; } = string.Empty;
            public bool Success { get; set; } = false;
        }

        public async Task<ToDoItemResult> GetToDoItemById(int id, string email)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.ToDoItems)
                    .FirstOrDefaultAsync(u => u.Email.Equals(email));

                if (user == null)
                {
                    return new ToDoItemResult
                    {
                        Success = false,
                        Message = "User not found.",
                        Response = new ItemResponse()
                    };
                }

                var toDoItem = user.ToDoItems.FirstOrDefault(i => i.Id == id);
                if (toDoItem == null)
                {
                    return new ToDoItemResult
                    {
                        Success = false,
                        Message = "ToDo item not found."
                    };
                }

                ItemResponse response = new()
                {
                    Id = toDoItem.Id,
                    Title = toDoItem.Title,
                    IsCompleted = toDoItem.IsCompleted
                };

                _logger.LogInformation($"ToDo item found for user: {email}");

                return new ToDoItemResult
                {
                    Success = true,
                    Message = "Success",
                    Response = response
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Error retrieving ToDo item: {e.Message}");
                return new ToDoItemResult
                {
                    Success = false,
                    Message = $"Error retrieving ToDo item: {e.Message}"
                };
            }
        }

        public async Task<(bool Success, string Message)> UpdateToDo(UpdateToDoRequest request)
        {
            try
            {
                var userWithToDoItems = await _dbContext.Users
                    .Include(u => u.ToDoItems)
                    .FirstOrDefaultAsync(u => u.Email.Equals(request.UserEmail));

                if (userWithToDoItems == null)
                {
                    return (false, "User not found.");
                }

                var toDoItem = userWithToDoItems.ToDoItems.FirstOrDefault(i => i.Id == request.ToDoId);
                if (toDoItem == null)
                {
                    return (false, $"ToDo item with ID {request.ToDoId} not found.");
                }

                toDoItem.Title = request.Title;
                toDoItem.IsCompleted = request.IsCompleted;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"ToDo item with ID {toDoItem.Id} updated for user: {userWithToDoItems.Email}");

                return (true, "ToDo item is updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error updating ToDo item: {e.Message}");
                return (false,$"Error updating ToDo item: {e.Message}");
            }
        }
        public async Task<(bool Success, string Message)> DeleteToDoItem(int id, string email)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.ToDoItems)
                    .FirstOrDefaultAsync(u => u.Email.Equals(email));

                if (user == null)
                {
                    return (false,"User not found.");
                }

                var toDoItem = user.ToDoItems.FirstOrDefault(i => i.Id == id);
                if (toDoItem == null)
                {
                    return (false, "User not found.");
                }

                _dbContext.ToDoItems.Remove(toDoItem);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"ToDo item with ID {id} deleted for user: {email}");

                return (true, $"ToDo item with ID {id} has been successfully deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error deleting ToDo item: {e.Message}");
                return (false, $"Error deleting ToDo item: {e.Message}");
            }
        }
    }
}
