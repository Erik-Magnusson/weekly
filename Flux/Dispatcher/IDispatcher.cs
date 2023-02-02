using Flux.Dispatchables;
namespace Flux.Dispatcher
{
    public interface IDispatcher
    {
        Action<IDispatchable> Action { get; set; }
        void Dispatch(IDispatchable payload);
    }
}
