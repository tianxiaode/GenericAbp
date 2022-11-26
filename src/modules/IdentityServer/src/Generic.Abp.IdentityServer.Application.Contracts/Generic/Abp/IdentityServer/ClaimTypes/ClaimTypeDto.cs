

namespace Generic.Abp.IdentityServer.ClaimTypes
{
    public class ClaimTypeDto
    {
        public string Name { get; set; }

        public ClaimTypeDto(string name) { 
            Name = name;
        }
    }
}
