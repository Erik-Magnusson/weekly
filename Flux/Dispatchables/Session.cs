namespace Flux.Dispatchables
{
    public class Session : IDispatchable
    {
        public ActionType ActionType { get; set;}
        public required string Username { get; set; }
        public required Guid UserId { get; set; }
    }
}
