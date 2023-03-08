using ContactListApi.Entities;
using Microsoft.AspNetCore.Identity;

namespace ContactListApi.Data
{
    public class Seeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        public Seeder(ApplicationDbContext dbContext, IPasswordHasher<AppUser> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Categories.Any())
                {
                    var categories = GetCategories();
                    _dbContext.Categories.AddRange(categories);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.SubCategories.Any())
                {
                    var subCategories = GetSubCategories();
                    _dbContext.SubCategories.AddRange(subCategories);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Contacts.Any())
                {
                    var contacts = GetContacts();
                    _dbContext.Contacts.AddRange(contacts);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.AppUsers.Any())
                {
                    var appUsers = GetAppUsers();
                    _dbContext.AppUsers.AddRange(appUsers);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<AppUser> GetAppUsers()
        {

            var appUsers = new List<AppUser>()
            {
                new AppUser()
                {
                    Email = "exampleUser@example.com",

                }

            };
            appUsers[0].PasswordHash = _passwordHasher.HashPassword(appUsers[0], "example@1234");
            return appUsers;
        }
        private IEnumerable<SubCategory> GetSubCategories()
        {
            var subCategories = new List<SubCategory>()
            {
                new SubCategory()
                { Name = "Worker" },
                 new SubCategory()
                { Name = "Boss" },
                new SubCategory()
                { Name = "Intern" },
                new SubCategory()
                { Name = "Client" },
                new SubCategory()
                { Name = "Family" },
                new SubCategory()
                { Name = "Friends" },
             };
            return subCategories;
        }

        private IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>()
            {
                new Category()
                { Name = "Job" },
                new Category()
                { Name = "Private" },
                new Category()
                { Name = "Other" }
            };
            return categories;
        }
       
        private IEnumerable<Contact> GetContacts()
        {
            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    FirstName = "Grzegorz",
                    LastName = "Brzęczyszczykiewicz",
                    Email = "Grzegorz.Brzęczyszczykiewicz@gmail.com",
                    PhoneNumber = "+48111222333",
                    BirthDate = new DateTime(1998, 10, 20),
                },
                new Contact()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@example.com",
                    PhoneNumber = "555-123-456",
                    BirthDate = new DateTime(1990, 1, 1)
                },
                new Contact()
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "janedoe@example.com",
                    PhoneNumber = "555-234-567",
                    BirthDate = new DateTime(1991, 2, 2)
                },
                new Contact()
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alicesmith@example.com",
                    PhoneNumber = "555-345-678",
                    BirthDate = new DateTime(1992, 3, 3)
                },
                new Contact()
                {
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bobsmith@example.com",
                    PhoneNumber = "555-456-789",
                    BirthDate = new DateTime(1993, 4, 4)
                },
                new Contact()
                {
                    FirstName = "Charlie",
                    LastName = "Brown",
                    Email = "charliebrown@example.com",
                    PhoneNumber = "555-567-890",
                    BirthDate = new DateTime(1994, 5, 5)
                },
                new Contact()
                {
                    FirstName = "Lucy",
                    LastName = "Brown",
                    Email = "lucybrown@example.com",
                    PhoneNumber = "555-678-901",
                    BirthDate = new DateTime(1995, 6, 6)
                },
                new Contact()
                {
                    FirstName = "David",
                    LastName = "Johnson",
                    Email = "davidjohnson@example.com",
                    PhoneNumber = "555-789-012",
                    BirthDate = new DateTime(1996, 7, 7)
                },
                new Contact()
                {
                    FirstName = "Emily",
                    LastName = "Johnson",
                    Email = "emilyjohnson@example.com",
                    PhoneNumber = "555-890-123",
                    BirthDate = new DateTime(1997, 8, 8)
                },
                new Contact()
                {
                    FirstName = "Frank",
                    LastName = "Williams",
                    Email = "frankwilliams@example.com",
                    PhoneNumber = "555-901-234",
                    BirthDate = new DateTime(1998, 9, 9)
                },
                new Contact()
                {
                    FirstName = "Grace",
                    LastName = "Williams",
                    Email = "gracewilliams@example.com",
                    PhoneNumber = "555-012-345",
                    BirthDate = new DateTime(1999, 10, 10)
                }
            };


            return contacts;
        }
    }
}
