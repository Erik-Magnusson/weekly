

namespace Flux.Dispatchables
{
    public class Dispatchable<T> : IDispatchable
    {
        public ActionType ActionType { get; set; }

        public T Payload { get; set; }

        public Dispatchable(ActionType actionType, T value)
        {
            ActionType = actionType;
            Payload = value;
        }
    }
}
