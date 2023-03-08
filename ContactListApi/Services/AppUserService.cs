using ContactListApi.Data;
using ContactListApi.Entities;
using ContactListApi.Exceptions;
using ContactListApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactListApi.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AppUserService(ApplicationDbContext context,IPasswordHasher<AppUser> passwordHasher,AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public void RegisterAppUser(RegisterAppUserDto dto)
        {
            
            var newAppUser = new AppUser()
            {
                Email = dto.Email,
            };

            newAppUser.PasswordHash = _passwordHasher.HashPassword(newAppUser, dto.Password);
            _context.AppUsers.Add(newAppUser);
            _context.SaveChanges();
        }
        public string GenerateJwt(LoginDto dto)
        {
            var appUser = _context.AppUsers.FirstOrDefault(a => a.Email == dto.Email);
            if (appUser == null)
            {
                throw new BadRequestException("Wrong email or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Wrong email or password.");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, appUser.Email.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);

        }
    }
}
