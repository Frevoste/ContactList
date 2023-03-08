using ContactListApi.Models;

namespace ContactListApi.Services
{
    public interface IContactService
    {
        int Create(CreateContactDto dto);
        PagedResults<ContactDto> GetAll(ContactQuery query);
        ContactDto GetById(int id);
        void Delete(int id);
        void Update(int id,UpdateContactDto dto);
    }
}