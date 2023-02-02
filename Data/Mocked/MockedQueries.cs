using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.Mocked
{
    public class MockedQueries<T> : IQueries<T>
    {
        public IList<T> GetAll()
        {
            new List<Todo>()
            {
                new Todo()
                {
                    ActionType = ActionType.ADD,
                    Id = Guid.NewGuid(),
                    Category = TodoCategory.WEEKLY,
                    Text = "Excercise",
                    Icon = "fitness_center",
                    NrTotal = 5,
                    NrDone = 0,
                    Unit = TodoUnit.TIMES,
                },
                new Todo()
                {
                    ActionType = ActionType.ADD,
                    Id = Guid.NewGuid(),
                    Category = TodoCategory.WEEKLY,
                    Text = "Clean",
                    Icon = "cleaning_services",
                    NrTotal = 1,
                    NrDone = 0,
                    Unit = TodoUnit.TIMES,
                },
                new Todo()
                {
                    ActionType = ActionType.ADD,
                    Id = Guid.NewGuid(),
                    Category = TodoCategory.WEEKLY,
                    Text = "Groceries",
                    Icon = "local_grocery_store",
                    NrTotal = 1,
                    NrDone = 0,
                    Unit = TodoUnit.TIMES,
                },
                new Todo()
                {
                    ActionType = ActionType.ADD,
                    Id = Guid.NewGuid(),
                    Category = TodoCategory.WEEKLY,
                    Text = "Social",
                    Icon = "group",
                    NrTotal = 2,
                    NrDone = 0,
                    Unit = TodoUnit.TIMES,
                },
                new Todo()
                {
                    ActionType = ActionType.ADD,
                    Id = Guid.NewGuid(),
                    Category = TodoCategory.WEEKLY,
                    Text = "Good sleep",
                    Icon = "bed",
                    NrTotal = 5,
                    NrDone = 0,
                    Unit = TodoUnit.DAYS,
                },


            };
        }

        public T GetOne(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
