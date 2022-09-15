﻿using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Generic.Abp.Themes.Shared.PageToolbars
{
    public class PageToolbarContributionContext
    {
        [NotNull]
        public string PageName { get; }

        [NotNull]
        public IServiceProvider ServiceProvider { get; }

        [NotNull]
        public PageToolbarItemList Items { get; }

        public PageToolbarContributionContext(
            [NotNull] string pageName,
            [NotNull] IServiceProvider serviceProvider)
        {
            PageName = Check.NotNull(pageName, nameof(pageName));
            ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));

            Items = new PageToolbarItemList();
        }
    }
}
