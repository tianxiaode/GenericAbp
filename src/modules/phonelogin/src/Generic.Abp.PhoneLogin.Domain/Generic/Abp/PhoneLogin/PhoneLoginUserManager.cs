using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Generic.Abp.PhoneLogin
{
    public class PhoneLoginUserManager : IdentityUserManager
    {
        public PhoneLoginUserManager(IdentityUserStore store, IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher, IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<IdentityUserManager> logger,
            ICancellationTokenProvider cancellationTokenProvider,
            IOrganizationUnitRepository organizationUnitRepository, ISettingProvider settingProvider,
            IDistributedEventBus distributedEventBus, IIdentityLinkUserRepository identityLinkUserRepository,
            IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache) : base(store, roleRepository, userRepository,
            optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services,
            logger, cancellationTokenProvider, organizationUnitRepository, settingProvider, distributedEventBus,
            identityLinkUserRepository, dynamicClaimCache)
        {
        }


        [UnitOfWork]
        public virtual async Task<IdentityUser> FindByPhoneAsync(string phoneNumber)
        {
            var list = await UserRepository.GetListAsync(phoneNumber: phoneNumber);
            return list.FirstOrDefault();
        }
    }
}