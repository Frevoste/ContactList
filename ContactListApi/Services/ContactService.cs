using AutoMapper;
using ContactListApi.Data;
using ContactListApi.Entities;
using ContactListApi.Exceptions;
using ContactListApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ContactListApi.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public ContactService(ApplicationDbContext dbContext, IMapper mapper, ILogger<ContactService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public ContactDto GetById(int id)
        {
            var contact = _dbContext
                .Contacts
                .Include(c => c.Category)
                .Include(c => c.SubCategory)
                .FirstOrDefault(c => c.Id == id);

            if (contact == null) throw new NotFoundException("Contact not found.");

            //Przemapowanie danych których nie chcemy przekazać użytkownikowi Api
            var result = _mapper.Map<ContactDto>(contact);
            return result;
        }

        public PagedResults<ContactDto> GetAll(ContactQuery query)
        {
            if (query.SearchPhrase != null)
            {
                query.SearchPhrase = query.SearchPhrase.ToLower();
            }

            var baseQuery = _dbContext
                .Contacts
                .Include(c => c.Category)
                .Include(c => c.SubCategory)
                .Where(c => query.SearchPhrase == null || (c.FirstName.ToLower().Contains(query.SearchPhrase) || c.LastName.ToLower().Contains(query.SearchPhrase) || c.Category.Name.ToLower().Contains(query.SearchPhrase) || c.SubCategory.Name.ToLower().Contains(query.SearchPhrase) || c.Email.ToLower().Contains(query.SearchPhrase)));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Contact, object>>>
                {
                    {nameof(Contact.FirstName), r => r.FirstName },
                    {nameof(Contact.LastName), r => r.LastName },
                    {nameof(Contact.Email), r => r.Email },
                    {nameof(Contact.BirthDate), r => r.BirthDate },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == Data.Enum.SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }




            var contacts = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();
            var contactsDtos = _mapper.Map<List<ContactDto>>(contacts);
            var result = new PagedResults<ContactDto>(contactsDtos, totalItemsCount, query.PageSize, query.PageNumber);
            //Przemapowanie danych których nie chcemy przekazać użytkownikowi Api


            return result;
        }

        public int Create(CreateContactDto dto)
        {
            List<string> avaiableJobSubCategories = new List<string>() { "Worker", "Boss", "Client", "Intern" };
            List<string> avaiablePrivateSubCategories = new List<string>() { "Family", "Friend", "Client", "Intern" };

            var contact = _mapper.Map<Contact>(dto);

            if (dto.Category.ToLower() == "job")
            {
                if (avaiableJobSubCategories.Any(x => x == dto.SubCategory))
                {
                    contact.Category = _dbContext.Categories.FirstOrDefault(x => x.Name == "Job");
                    contact.SubCategory = _dbContext.SubCategories.FirstOrDefault(x => x.Name == dto.SubCategory);
                }
                else
                    throw new BadRequestException("Subcategory does not match category.");
            }
            else if (dto.Category.ToLower() == "private")
            {
                if (avaiablePrivateSubCategories.Any(x => x == dto.SubCategory))
                {
                    contact.Category = _dbContext.Categories.FirstOrDefault(x => x.Name == "Private");
                    contact.SubCategory = _dbContext.SubCategories.FirstOrDefault(x => x.Name == dto.SubCategory);
                }
                else
                    throw new BadRequestException("Subcategory does not match category.");
            }
            else if (dto.Category.ToLower() == "other")
                contact.Category = _dbContext.Categories.FirstOrDefault(x => x.Name == "Other");
            else
                throw new BadRequestException("Wrong category.");

            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();


            return contact.Id;
        }

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

        public void Update(int id, UpdateContactDto dto)
        {
            List<string> avaiableJobSubCategories = new List<string>() { "Worker", "Boss", "Client", "Intern" };
            List<string> avaiablePrivateSubCategories = new List<string>() { "Family", "Friend" };

            var contact = _dbContext
                .Contacts
                .FirstOrDefault(c => c.Id == id);

            if (contact == null) { throw new NotFoundException("Contact not found."); }

            contact.FirstName = dto.FirstName;
            contact.LastName = dto.LastName;
            contact.Email = dto.Email;
            contact.PhoneNumber = dto.PhoneNumber;
            contact.BirthDate = dto.BirthDate;

            if (dto.Category.ToLower() == "job")
            {
                if (avaiableJobSubCategories.Any(x => x == dto.SubCategory))
                {
                    contact.Category = _dbContext.Categories.FirstOrDefault(x => x.Name == "Job");
                    contact.SubCategory = _dbContext.SubCategories.FirstOrDefault(x => x.Name == dto.SubCategory);
                }
                else
                    throw new BadRequestException("Subcategory does not match category.");
            }
            else if (dto.Category.ToLower() == "private")
            {
                if (avaiablePrivateSubCategories.Any(x => x == dto.SubCategory))
                {
                    contact.Category = _dbContext.Categories.FirstOrDefault(x => x.Name == "Private");
                    contact.SubCategory = _dbContext.SubCategories.FirstOrDefault(x => x.Name == dto.SubCategory);
                }
                else
                    throw new BadRequestException("Subcategory does not match category.");
            }
            else if (dto.Category.ToLower() == "other")
            {
                contact.Category = _dbContext.Categories.FirstOrDefault(x => x.Name == "Other");
                contact.SubCategory = new SubCategory() { Name = dto.SubCategory };
            }
            else if (dto.Category != null)
                throw new BadRequestException("Wrong category.");


            _dbContext.SaveChanges();

        }
    }
}
