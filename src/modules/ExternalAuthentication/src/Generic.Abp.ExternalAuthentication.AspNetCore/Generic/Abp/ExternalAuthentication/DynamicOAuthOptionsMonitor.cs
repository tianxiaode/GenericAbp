using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;

namespace Generic.Abp.ExternalAuthentication;

public class DynamicOAuthOptionsMonitor<TOptions>(
    ExternalAuthenticationSettingManager settingManager,
    IOptionsMonitor<TOptions> defaultMonitor,
    string schemeName)
    : IOptionsMonitor<TOptions>
    where TOptions : OAuthOptions, new()
{
    private readonly ExternalAuthenticationSettingManager _settingManager =
        settingManager ?? throw new ArgumentNullException(nameof(settingManager));

    private readonly IOptionsMonitor<TOptions> _defaultMonitor =
        defaultMonitor ?? throw new ArgumentNullException(nameof(defaultMonitor));

    private readonly string _schemeName = schemeName ?? throw new ArgumentNullException(nameof(schemeName));

    public TOptions CurrentValue => Get(null);

    public TOptions Get(string? name)
    {
        // 从默认配置中克隆一个新实例
        var options = Clone(_defaultMonitor.Get(name));

        // 获取动态配置值
        var providerValue = _settingManager.GetProviderAsync(_schemeName).GetAwaiter().GetResult();
        if (!providerValue.Enabled)
        {
            return options;
        }

        options.ClientId = providerValue.ClientId;
        options.ClientSecret = providerValue.ClientSecret;

        return options;
    }

    public IDisposable? OnChange(Action<TOptions, string?> listener)
    {
        // 转发 OnChange 事件
        return _defaultMonitor.OnChange(listener);
    }

    private static TOptions Clone(TOptions source)
    {
        // 返回浅拷贝，避免修改原始对象
        var options = new TOptions
        {
            CallbackPath = source.CallbackPath,
            ClientId = source.ClientId,
            ClientSecret = source.ClientSecret,
            Events = source.Events,
            SaveTokens = source.SaveTokens,
            ClaimsIssuer = source.ClaimsIssuer,
            Backchannel = source.Backchannel,
            BackchannelTimeout = source.BackchannelTimeout,
            ForwardSignOut = source.ForwardSignOut,
            ForwardDefaultSelector = source.ForwardDefaultSelector,
            ForwardChallenge = source.ForwardChallenge,
            ForwardAuthenticate = source.ForwardAuthenticate,
            ForwardDefault = source.ForwardDefault,
            ForwardForbid = source.ForwardForbid,
            UserInformationEndpoint = source.UserInformationEndpoint,
            AuthorizationEndpoint = source.AuthorizationEndpoint,
            TokenEndpoint = source.TokenEndpoint
        };

        // 确保 Scope 复制正确
        options.Scope.Clear(); // 清空目标集合，防止重复
        foreach (var scope in source.Scope)
        {
            options.Scope.Add(scope);
        }

        return options;
    }
}