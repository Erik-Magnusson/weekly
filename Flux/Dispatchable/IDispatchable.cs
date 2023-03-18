namespace Flux.Dispatchable
{
    public interface IDispatchable<T> where T : Enum
    {
        T ActionType { get; set; }
    }
}
