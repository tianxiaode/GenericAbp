using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Themes.Shared.Components
{
    public class DefaultBrandingProvider : IBrandingProvider, ITransientDependency
    {
        public virtual string AppName => "MyApplication";

        public virtual string LogoUrl => null;

        public virtual string LogoReverseUrl => null;
    }
}
