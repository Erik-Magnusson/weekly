using Flux.Dispatchable;

namespace Flux.Dispatcher
{
    public class Dispatcher<T> : IDispatcher<T> where T : Enum
    {
        public Action<IDispatchable<T>> Action { get; set; }
        public void Dispatch(IDispatchable<T> payload)
        {
            Action?.Invoke(payload);
        }

    }
}
