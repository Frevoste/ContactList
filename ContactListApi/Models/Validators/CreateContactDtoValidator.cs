using ContactListApi.Data;
using FluentValidation;

namespace ContactListApi.Models.Validators
{
    public class CreateContactDtoValidator : AbstractValidator<CreateContactDto>
    {
        public CreateContactDtoValidator(ApplicationDbContext dbContext)
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
        }
    }
}
