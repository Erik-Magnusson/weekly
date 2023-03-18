using Web.Models;
using Flux.Store;

namespace Web.Client.Stores
{
    public interface ITemplateStore : IStore
    {
        IList<Template> Templates { get; }

    }
}
