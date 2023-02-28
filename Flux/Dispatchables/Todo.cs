using Data;
namespace Flux.Dispatchables
{
    public class Todo : DataEntityBase, IDispatchable
    {
        
        public ActionType ActionType { get; set; }  
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public int NrTotal { get; set; }
        public int NrDone { get; set; }
        public TodoUnit Unit { get; set; }

        public Todo(Todo todo)
        {
            this.Id = null;
            this.ActionType = todo.ActionType;
            this.UserId = todo.UserId;
            this.Text = todo.Text;
            this.NrTotal = todo.NrTotal;
            this.NrDone = todo.NrDone;
            this.Unit = todo.Unit;
        }

        public Todo() { }
    }
    public enum TodoUnit
    {
        HOURS,
        DAYS,
        TIMES,
               
    }
}
