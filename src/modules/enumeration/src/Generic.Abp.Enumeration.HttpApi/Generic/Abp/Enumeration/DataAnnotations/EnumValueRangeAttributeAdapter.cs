using Generic.Abp.Enumeration.Validation;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Volo.Abp;

namespace Generic.Abp.Enumeration.DataAnnotations
{
    public class EnumValueRangeAttributeAdapter:AttributeAdapterBase<EnumValueRangeAttribute>
    {
        public EnumValueRangeAttributeAdapter(EnumValueRangeAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            Check.NotNull(context, nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            Check.NotNull(validationContext, nameof(validationContext));

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.DataType
            );
        }
    }
}