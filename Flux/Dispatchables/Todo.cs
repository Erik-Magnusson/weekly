using Data;
namespace Flux.Dispatchables
{
    public class Todo : DataEntityBase, IDispatchable
    {
        public ActionType ActionType { get; set; }  
        public string Text { get; set; }
        public string Icon { get; set; }
        public int NrTotal { get; set; }
        public int NrDone { get; set; }
        public TodoUnit Unit { get; set; }
    }
    public enum TodoUnit
    {
        HOURS,
        DAYS,
        TIMES,
               
    }
}
