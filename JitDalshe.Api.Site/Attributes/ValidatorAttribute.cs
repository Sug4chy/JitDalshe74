using FluentValidation;

namespace JitDalshe.Api.Site.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ValidatorAttribute<TValidator> : Attribute where TValidator : IValidator;