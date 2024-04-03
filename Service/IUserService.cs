using Todo.Request;
using Todo.Response;
using static Todo.Service.UserService;

namespace Todo.Service
{
    public interface IUserService
    {
        public Task<(bool isCreated, string message)> CreateToDo(ToDoPostRequest request);
        public Task<ToDoItemsResult> GetAllToDoItemsAsync(string email);
        public Task<ToDoItemResult> GetToDoItemById(int id, string email);
        public Task<(bool Success, string Message)> UpdateToDo(UpdateToDoRequest request);
        public Task<(bool Success, string Message)> DeleteToDoItem(int id, string email);
    }
}
