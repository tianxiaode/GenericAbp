using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Extensions.Tokens;

public class TokenManager : ITransientDependency
{
    public TokenData<T> GenerateTokenData<T>(Guid id, DateTime expireTime, T payload = default!)
    {
        // 动态生成 AES 密钥
        var aesKey = Guid.NewGuid().ToString("N")[..32];

        // 构造 TokenData
        var tokenData = new TokenData<T>
        {
            Id = id,
            ExpireTime = expireTime,
            Payload = payload,
        };

        // 序列化并加密
        tokenData.Token = GenerateAesToken(tokenData, aesKey);
        tokenData.Key = aesKey;

        return tokenData;
    }

    public (bool IsValid, TokenData<T>? Data) ValidateTokenData<T>(string token, string key)
    {
        try
        {
            return ValidateAesToken<T>(token, key);
        }
        catch
        {
            return (false, null); // 解密或验证失败
        }
    }

    private string GenerateAesToken<T>(TokenData<T> tokenData, string key)
    {
        var payload = JsonSerializer.Serialize(tokenData);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cs))
        {
            writer.Write(payload);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    private (bool IsValid, TokenData<T>? Data) ValidateAesToken<T>(string token, string key)
    {
        var encryptedBytes = Convert.FromBase64String(token);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);

        var iv = encryptedBytes.Take(16).ToArray();
        var cipherText = encryptedBytes.Skip(16).ToArray();

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipherText);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);
        var payload = reader.ReadToEnd();
        var tokenData = JsonSerializer.Deserialize<TokenData<T>>(payload);
        if (tokenData == null)
        {
            return (false, null);
        }

        return tokenData.ExpireTime < DateTime.UtcNow
            ? (false, null)
            : // 令牌已过期
            (true, tokenData);
    }
}