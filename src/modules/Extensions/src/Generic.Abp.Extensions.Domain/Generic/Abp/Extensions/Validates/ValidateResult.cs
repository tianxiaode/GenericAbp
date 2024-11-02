namespace Generic.Abp.Extensions.Validates;

public class ValidateResult
{
    public bool Success { get; set; } = true;
    public string? Value { get; set; } = default!;
}