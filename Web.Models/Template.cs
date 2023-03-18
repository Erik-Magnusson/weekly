
namespace Web.Models
{
    public class Template
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public int NrTotal { get; set; }
        public Todo.TodoUnit Unit { get; set; }
    }
}
