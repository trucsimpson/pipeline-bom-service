using BOMService.Application.BOMs.Commands;
using FluentValidation;

namespace BOMService.Application.BOMs.Validators
{
    public class CreateBOMValidator : AbstractValidator<CreateBOMCommand>
    {
        public CreateBOMValidator()
        {
            RuleFor(x => x.Payload.CommunityId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.Payload.HouseId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
