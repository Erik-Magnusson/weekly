
namespace Data.Models
{
    public class Week
    {
        public int WeekNr 
        { 
            get
            {
                return _weekNr;
            }
            set
            {
                if (value <= 0)
                {
                    _weekNr = 52;
                    Year--;
                    return;
                }
                if (value > 52)
                {
                    _weekNr = 1;
                    Year++;
                    return;
                }
                _weekNr = value;

            }
        }
        public int Year { get; set; }
        private int _weekNr;

  
    }
}
