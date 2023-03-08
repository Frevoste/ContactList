using ContactListApi.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;

namespace ContactListApi.Models.Validators
{
    public class ContactQueryValidator : AbstractValidator<ContactQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = { nameof(Contact.FirstName), nameof(Contact.LastName), nameof(Contact.Email), nameof(Contact.BirthDate) };
        public ContactQueryValidator()
        {
            
            RuleFor(c => c.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(c => c.PageSize)
                .Custom((value, context) =>
                {
                    if(!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure("PageSize",$"PageSize must be in [{string.Join(", ", allowedPageSizes)}]");
                    }
                });
            RuleFor(c => c.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional. or must be in [{string.Join(",",allowedSortByColumnNames)}");
        }
    }
}
