namespace Data
{
    public class Queries<T> : IQueries<T>
    {

        public Task<IList<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}