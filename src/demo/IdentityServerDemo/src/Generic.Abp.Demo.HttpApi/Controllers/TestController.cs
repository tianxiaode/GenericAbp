using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Generic.Abp.Demo.Controllers
{
    [Area("Demo")]
    [ControllerName("Demo")]
    [Route("api/test")]
    public class TestController : DemoController
    {
        public TestController(IdentityUserManager identityUserManager)
        {
            IdentityUserManager = identityUserManager;
        }
        protected IdentityUserManager IdentityUserManager { get; }

    }
}
