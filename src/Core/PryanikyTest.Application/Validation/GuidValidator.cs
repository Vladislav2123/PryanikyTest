using FluentValidation;

namespace PryanikyTest.Application.Validation;

/// <summary>
/// Custom validator for Guid`s.
/// </summary>
public class GuidValidator : AbstractValidator<Guid>
{
	public GuidValidator()
	{
		RuleFor(guid => guid)
			.NotNull()
			.NotEmpty();
	}
}