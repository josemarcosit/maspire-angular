using maspire_angular.Infrastructure.Persistence;
using maspire_angular.Shared.Abstractions;
using maspire_angular.Shared.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Cryptography;
using System.Text;

namespace maspire_angular.Features.Auth
{
    public class AuthService : IAuthService
    {
        private readonly MaspireDbContext _context;        
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(MaspireDbContext context,
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<AuthResult> RegisterAsync(string email, string fullName, string password)
        {            
            var existingUser = await GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = _localizer["UserAlreadyExists"],
                };
            }
            
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = _localizer["PasswordMinLength"]
                };
            }

            try
            {
                var user = new User
                {
                    Email = email,
                    FullName = fullName,
                    PasswordHash = HashPassword(password),
                    Role = "User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new AuthResult
                {
                    Success = true,
                    Message = _localizer["UserRegisteredSuccess"],
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        Role = user.Role
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = String.Format(_localizer["UserRegisteredSuccess"], ex.Message)
                };
            }
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);

                if (user == null || !VerifyPassword(password, user.PasswordHash) || !user.IsActive)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = _localizer["InvalidCredentials"]
                    };
                }

                // Atualizar último login
                user.LastLogin = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new AuthResult
                {
                    Success = true,
                    Message = _localizer["LoginSuccess"],
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        Role = user.Role
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = String.Format(_localizer["LoginError"], ex.Message)
                };
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash);
        }
    }
}
