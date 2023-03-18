using Web.Models;


namespace Flux.Stores
{
    public interface ITemplateStore : IStore
    {
        IList<Template> Templates { get; }

    }
}
