namespace Todo.Request
{
    public class ToDoPostRequest
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
    }
}
