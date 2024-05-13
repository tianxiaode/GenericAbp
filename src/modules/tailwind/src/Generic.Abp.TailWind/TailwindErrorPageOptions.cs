namespace Generic.Abp.Tailwind;

public class TailwindErrorPageOptions
{
    public readonly IDictionary<string, string> ErrorViewUrls;

    public TailwindErrorPageOptions()
    {
        ErrorViewUrls = new Dictionary<string, string>();
    }
}