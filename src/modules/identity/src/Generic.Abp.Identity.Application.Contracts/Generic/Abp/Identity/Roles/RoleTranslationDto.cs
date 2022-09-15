namespace Generic.Abp.Identity.Roles;

public class RoleTranslationDto
{
    public string Language { get; set; }
    public string Name { get; set; }

    public RoleTranslationDto(string language, string name)
    {
        Language = language;
        Name = name;
    }
}
