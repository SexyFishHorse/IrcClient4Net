namespace SexyFishHorse.Irc.Client.Configuration
{
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    public class IrcClientModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromThisAssembly()
                .SelectAllClasses()
                .BindDefaultInterface());
        }
    }
}
