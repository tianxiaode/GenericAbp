using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.Widgets;

public interface IWidgetManager : ITransientDependency
{
    Task<bool> IsGrantedAsync(Type widgetComponentType);

    Task<bool> IsGrantedAsync(string name);
}
