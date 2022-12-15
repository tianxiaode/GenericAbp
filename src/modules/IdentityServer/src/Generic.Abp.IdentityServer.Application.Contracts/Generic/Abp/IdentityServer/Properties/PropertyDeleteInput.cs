using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.IdentityServer.Properties
{
    public class PropertyDeleteInput
    {
        [Required]
        [DisplayName("Properties:Key")]
        public string Key { get; set; }

    }
}
