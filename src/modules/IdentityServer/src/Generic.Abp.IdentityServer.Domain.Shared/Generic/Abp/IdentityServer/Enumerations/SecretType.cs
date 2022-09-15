using Generic.Abp.Enumeration;

namespace Generic.Abp.IdentityServer.Enumerations;

public class SecretType: Enumeration<SecretType>
{
    public static readonly SecretType X509CertificateName =
        new(0, IdentityServerConstants.SecretTypes.X509CertificateName, order: 0);
    public static readonly SecretType JsonWebKey = new(1, IdentityServerConstants.SecretTypes.JsonWebKey, order: 1);

    public static readonly SecretType SharedSecret =
        new(2, IdentityServerConstants.SecretTypes.SharedSecret, isDefault: true, order: 2);

    public static readonly SecretType X509CertificateBase64 =
        new(3, IdentityServerConstants.SecretTypes.X509CertificateBase64, order: 3);

    public static readonly SecretType X509CertificateThumbprint =
        new(4, IdentityServerConstants.SecretTypes.X509CertificateThumbprint, order: 4);
    public SecretType(byte value, string name, string[] permission = null, bool isDefault = false, bool isPrivate = false, int order = 0) : base(value, name, permission, isDefault, isPrivate, order)
    {
        ResourceName = "AbpIdentityServerResource";
    }
}