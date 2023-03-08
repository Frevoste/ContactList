using ContactListApi.Data;
using ContactListApi.Exceptions;
using FluentValidation;

namespace ContactListApi.Models.Validators
{
    public class RegisterAppUserDtoValidator : AbstractValidator<RegisterAppUserDto>
    {
        public RegisterAppUserDtoValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.AppUsers.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                }); 
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .Custom((value,context) =>
                 {
                     if(!value.Any(char.IsUpper))
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

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
                
        }
    }
}


