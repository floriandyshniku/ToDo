namespace Todo.Request
{
    public class UpdateToDoRequest
    {
        public string UserEmail { get; set; } = string.Empty;
        public int ToDoId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
    }
}
