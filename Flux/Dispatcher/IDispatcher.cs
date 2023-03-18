using Flux.Dispatchable;
namespace Flux.Dispatcher
{
    public interface IDispatcher<T> where T : Enum
    {
        Action<IDispatchable<T>> Action { get; set; }
        void Dispatch(IDispatchable<T> payload);
    }
}
