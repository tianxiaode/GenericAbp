using System.Threading.Tasks;

namespace Generic.Abp.Account.Emailing;

public interface IAccountEmailer
{
    Task SendPasswordResetLinkAsync(
        IdentityUser user,
        string resetToken,
        string appName,
        string returnUrl = null,
        string returnUrlHash = null
    );
}