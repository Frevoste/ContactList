using ContactListApi.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ContactListApi.Models.Validators
{
    public class UpdateContactDtoValidator : AbstractValidator<UpdateContactDto>
    {
        public UpdateContactDtoValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Email)
              .NotEmpty()
              .EmailAddress()
              .Custom((value, context) =>
              {
                  var emailInUse = dbContext.Contacts.Any(c => c.Email == value);
                  if (emailInUse)
                  {
                      context.AddFailure("Email", "That email is taken");
                  }
              });
            RuleFor(x => x.Category)
               .Custom((value, context) =>
               {
                   var categoryExist = dbContext.Categories.Any(c => c.Name == value);
                   if (!categoryExist) { context.AddFailure("Category", "That category is not valid."); }
               });
            RuleFor(x => x.PasswordHash)
                .MinimumLength(8)
                .Custom((value, context) =>
                {
                    if (!value.Any(char.IsUpper))
                    {
                        context.AddFailure("Password", "Password does not contain upper letter.");
                    }
                    if (!value.Any(char.IsLower))
                    {
                        context.AddFailure("Password", "Password does not contain lower letter.");
                    }
                    if (!value.Any(char.IsDigit))
                    {
                        context.AddFailure("Password", "Password does not contain number.");
                    }
                    string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
                    char[] specialChArray = specialCh.ToCharArray();
                    foreach (char ch in specialChArray)
                    {
                        if (value.Contains(ch))
                        {
                            break;
                        }
                        if (specialChArray.Last() == ch)
                        {
                            context.AddFailure("Password", "Password does not contain special character.");
                        }
                    }
                });
        }
    }
}
