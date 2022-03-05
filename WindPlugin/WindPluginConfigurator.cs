using Bindito.Core;
using TimberbornAPI.EntityActionSystem;

namespace WindPlugin
{
    /// <summary>
    /// This class is a configurator that binds our custom classes
    /// </summary>
    public class WindPluginConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.MultiBind<IEntityAction>().To<WindPoweredGeneratorEntityAction>().AsSingleton();
        }
    }
}
