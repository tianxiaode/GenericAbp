using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.Account.Web.ProfileManagement;

public interface IProfileManagementPageContributor
{
    Task ConfigureAsync(ProfileManagementPageCreationContext context);
}