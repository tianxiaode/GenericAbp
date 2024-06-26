﻿using Volo.Abp.Modularity;

namespace Generic.Abp.MyProjectName;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class MyProjectNameApplicationTestBase<TStartupModule> : MyProjectNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
