using System.ComponentModel.DataAnnotations;

namespace Todo.Response
{
    public class ItemsResponse
    {

        public List<ItemData> ItemDatas = new List<ItemData>();

    }
    public class ItemData
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
    }
}
