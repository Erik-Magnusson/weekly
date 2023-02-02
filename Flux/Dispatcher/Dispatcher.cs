using Flux.Dispatchables;

namespace Flux.Dispatcher
{
    public class Dispatcher : IDispatcher
    {
        public Action<IDispatchable> Action { get; set; }
        public void Dispatch(IDispatchable payload)
        {
            Action?.Invoke(payload);
        }

    }
}
