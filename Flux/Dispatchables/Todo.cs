namespace Flux.Dispatchables
{
    public class Todo : IDispatchable
    {
        public ActionType ActionType { get; set; }
        public Guid Id { get; set; }
        public TodoCategory Category { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public int NrTotal { get; set; }
        public int NrDone { get; set; }
        public TodoUnit Unit { get; set; }


    }

    public enum TodoCategory
    {
        DAILY,
        WEEKLY,
        MONTHLY,
        QUARTERLY,
        BIYEARLY,
        YEARLY
    }

    public enum TodoUnit
    {
        HOURS,
        DAYS,
        TIMES,
               
    }
}
