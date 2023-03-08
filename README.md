# ContactList
Backend application with REST API architecture created using ASP.NET Core.
## Table of contents
* [Classes and Methods](#class-methods)
* [Libraries](#libraries)
* [Setup](#setup)

## Class-methods
I divided project into separate folders which contains simillar functions.
<details><summary>Controllers - they handle HTTP requests and generate HTTP responses</summary>
<p>

For example ContactController is responsible for Contacts in our application.
``` C#
namespace ContactListApi.Controllers
{
    [Route("api/contact")]
    [Authorize]
    [ApiController]
```
First we declare that our route to data will be "https:/localhost:7011/api/contact.
Then we declare that we want our data to be protected from unwanted api users. It can be seen when api user provide us with JWT.
Then we declare that our errors will be handled by custom Middleware.
``` C#
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Contact>> GetAll([FromQuery]ContactQuery query)
        {
            var contactsDtos = _contactService.GetAll(query);

            return Ok(contactsDtos);
        }
```
HttpGet action let everyone see our list of contacts. To display it it references to ContactQuery which allow user to filter Data he gets.
For example : https://localhost:7011/api/contact?searchPhrase=a&pageSize=5&pageNumber=2&sortBy=FirstName&sortDirection=ASC <br>
Returns : 5 records sorted by First name Ascending.
</p>
</details>

<details><summary>Entities - they define how our data will be stored in database.</summary>
<p>
  
``` C#
  //Wymuszenie unikatowości adresu email 
    [Index(nameof(Email), IsUnique = true)]
    public class Contact
    {
        //Primary key
        [Key]
        public int Id { get; set; }
        //Fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        //Foreign Key to Category
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int? SubCategoryId { get; set; }
        public virtual SubCategory? SubCategory { get; set; }

    }
```
  </p>
</details>

<details><summary>Middleware - operations that work during our runtime and monitor it.</summary>
  
  ``` C#
    
  namespace ContactListApi.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                context.Response.WriteAsync(notFoundException.Message);
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                context.Response.WriteAsync(badRequestException.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
  
  ```
  
  For example ErrorHandlingMiddleware.cs define how we handle diffrent Error scenarios and what type of data api user should get in case of problem.
<p>
  
  </p>
 </details>
  
 <details><summary>Models - custom models that help us validate data before posting it to Database or prevent unwanted data leak to App user.</summary>
   
   Inside models there are also Validators for example :

``` C#

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
                     string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\""';
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

```
It confirms that Password is correct and Email is unique in AppUser table.
                       
  </details>

   <details><summary>Services - they hold logic responsible for HTTP requests required by controllers.</summary>
    
```C#
     
      public void Delete(int id)
        {
            _logger.LogWarning($"Contact with id: {id} DELETE action invoked");

            var contact = _dbContext
               .Contacts
               .FirstOrDefault(c => c.Id == id);

            if (contact == null) { throw new NotFoundException("Contact not found."); }

            _dbContext.Contacts.Remove(contact);
            _dbContext.SaveChanges();

        }
     
```

public void Delete first checks if required record exsist before deleting it.

   </details>

## Libraries
Project is created with:
* Microsoft Entity Framework Core - https://learn.microsoft.com/pl-pl/ef/core/
* NLog - https://nlog-project.org
* Fluent Validation - https://docs.fluentvalidation.net/en/latest/
* Swagger - https://swagger.io
* AutoMapper - https://automapper.org

## Setup
* Prepare database ( I used MS SQL) 
* Change  "Default Connection" : ... in file appsettings.json to Your Database connection string.
* Compile Solution - During compilation The database will seed with data You can find in Data/Seeder.cs
* Now You can contact with Api using for example Postman.
* You can also preview working of application under https/localhost:7011/swagger
* If u want to test token generation You can post <br>
https://localhost:7011/api/account/login
with Json body filled like this :  
```
{
    "email" : "exampleUser@example.com",
    "password" : "example@1234",
    "confirmPassword" :"example@1234"
}



```
