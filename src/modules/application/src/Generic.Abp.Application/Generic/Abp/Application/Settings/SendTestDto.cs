using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.Application.Settings;

[Serializable]
public class SendTestDto
{
    public SendTestDto(string to, string subject, string body)
    {
        To = to;
        Subject = subject;
        Body = body;
    }

    [Required]
    public string To { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    public string Body { get; set; }
}