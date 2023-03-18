

namespace Flux.Dispatchable
{
    public class Dispatchable<T,U> : IDispatchable<T> where T : Enum
    {
        public T ActionType { get; set; }

        public U Payload { get; set; }

        public Dispatchable(T actionType, U value)
        {
            ActionType = actionType;
            Payload = value;
        }
    }
}
