using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Volo.Abp.Data;

namespace Generic.Abp.Extensions.Tokens;

public static class TokenExtensions
{
    private const string TokensPropertyName = "tokens";

    public static void SetToken<TEntity, T>(this TEntity entity, TokenData<T> token)
        where TEntity : IHasExtraProperties
    {
        var tokens = entity.GetTokens<TEntity, T>();
        if (tokens.Any(m => m.Token == token.Token))
        {
            return;
        }

        tokens.Add(token);
        entity.SetProperty(TokensPropertyName, tokens);
    }

    public static TokenData<T>? GetToken<TEntity, T>(this TEntity entity, string token)
        where TEntity : IHasExtraProperties
    {
        var tokens = entity.GetTokens<TEntity, T>();
        return tokens.FirstOrDefault(m => m.Token == token);
    }


    public static List<TokenData<T>> GetTokens<TEntity, T>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        var tokensString = entity.GetProperty(TokensPropertyName, "[]") ?? "[]";
        var tokens = JsonSerializer.Deserialize<List<TokenData<T>>>(tokensString) ?? [];
        return tokens;
    }
}