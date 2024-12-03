using System;

namespace Generic.Abp.Extensions.Tokens;

public class TokenData<T>
{
    public string Token { get; set; } = default!; // 加密后的令牌
    public string Key { get; set; } = default!; // 加密用的密钥
    public Guid Id { get; set; } = default!; // 唯一标识
    public DateTime ExpireTime { get; set; } = default!; // 过期时间
    public T? Payload { get; set; } = default; // 自定义附加数据
}