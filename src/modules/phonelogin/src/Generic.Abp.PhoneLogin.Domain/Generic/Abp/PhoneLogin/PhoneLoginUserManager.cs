using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Generic.Abp.PhoneLogin
{
    public class PhoneLoginUserManager : IdentityUserManager
    {
        public PhoneLoginUserManager(
            IdentityUserStore store,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<PhoneLoginUserValidator> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<IdentityUserManager> logger,
            ICancellationTokenProvider cancellationTokenProvider,
            IOrganizationUnitRepository organizationUnitRepository,
            ISettingProvider settingProvider)
            : base(
                store,
                roleRepository,
                userRepository,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger, cancellationTokenProvider, organizationUnitRepository, settingProvider)
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
