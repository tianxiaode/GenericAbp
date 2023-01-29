using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.TagHelpers;

public interface IHasItems<out T> where T : class
{
    public IEnumerable<T> Items { get; }
}