using ContactListApi.Models;

namespace ContactListApi.Services
{
    public interface IAppUserService
    {
        void RegisterAppUser(RegisterAppUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
}