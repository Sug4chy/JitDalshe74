using FluentValidation;

namespace JitDalshe.Api.Attributes;

[AttributeUsage(AttributeTargets.Struct |  AttributeTargets.Class)]
public class ValidatorAttribute<TValidator> : Attribute where TValidator : IValidator;