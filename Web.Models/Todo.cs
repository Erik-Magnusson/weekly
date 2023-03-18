
namespace Web.Models
{
    public class Todo : ApiEntityBase
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public int NrTotal { get; set; }
        public int NrDone { get; set; }
        public TodoUnit Unit { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }

        public enum TodoUnit
        {
            HOURS,
            DAYS,
            TIMES,

        }
    }
}

