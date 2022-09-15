using Generic.Abp.TextTemplate.Localization;
using Volo.Abp.TextTemplating;

namespace Generic.Abp.TextTemplate
{
    public class GenericTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                new TemplateDefinition(
                    "EmailLayout",
                    isLayout: true
                ).WithVirtualFilePath(
                    "/EmailLayout/EmailLayout.tpl", //template content path
                    isInlineLocalized: true
                )
            );

            context.Add(
                new TemplateDefinition(
                    name: "PasswordReset",
                    typeof(GenericTextTemplateResource),
                    defaultCultureName: "en",
                    layout: "EmailLayout"
                ).WithVirtualFilePath(
                    "/PasswordReset/Templates",  //template content path
                    true
                )
            );


        }
    }
}
