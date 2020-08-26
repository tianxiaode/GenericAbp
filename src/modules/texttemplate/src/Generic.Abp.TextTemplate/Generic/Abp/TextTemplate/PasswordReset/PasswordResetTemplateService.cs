using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace Generic.Abp.TextTemplate.PasswordReset
{
    public class PasswordResetTemplateService : ITransientDependency
    {
        private readonly ITemplateRenderer _templateRenderer;

        public PasswordResetTemplateService(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        public async Task<string> RunAsync(PasswordResetModel model)
        {
            var result = await _templateRenderer.RenderAsync(
                "PasswordReset", //the template name
                model
            );

            return result;
        }

    }
}
