using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TasinmazApi.Business.Abstract;
using TasinmazApi.DataAccess;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Business.Concrete
{
    public class AuthService : IAuthService 
    {
        private readonly ApplicationDbContext _context;  
        private readonly IConfiguration _configuration; 

        
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<string> RegisterAsync(Kullanici kullanici)
        {
            bool emailVarMi = await _context.Kullanicilar.AnyAsync(k => k.Email == kullanici.Email);
            if (emailVarMi)
            {
                return "Bu email zaten kullanılıyor!";
            }

          

            if (!IsValidEmail(kullanici.Email)) 
            {
                return " zaten Geçersiz e posta formatı. ";
            }

            
            if (!IsValidPassword(kullanici.Sifre))
            {
                return " zaten Şifre en az 8 karakter, 1 harf, 1 rakam ve 1 özel karakter içermelidir!";
            }
          

            kullanici.Sifre = HashPassword(kullanici.Sifre);
            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();  

            return "Kullanıcı başarıyla kaydedildi ✅";
        }

        public async Task<string> LoginAsync(KullaniciLoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Sifre))
            {
                return "Email veya şifre boş olamaz!";
            }

            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Email == dto.Email);

            if (kullanici == null)
            {
                return "Kullanıcı bulunamadı!";
            }

            string hashedInput;

            if (string.IsNullOrEmpty(kullanici.Salt))
            {
                
                hashedInput = HashPassword(dto.Sifre);
            }
            else
            {
               
                hashedInput = HashPassword(dto.Sifre + kullanici.Salt);
            }

            if (kullanici.Sifre != hashedInput)
            {
                return "Şifre yanlış!";
            }

            return GenerateJwtToken(kullanici);
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string GenerateJwtToken(Kullanici kullanici)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciId.ToString()),
                new Claim(ClaimTypes.Email, kullanici.Email),
                new Claim(ClaimTypes.Role, kullanici.Rol.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expires"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasLetter = password.Any(char.IsLetter);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasLetter && hasDigit && hasSpecialChar;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                 return false;
            }
        }
    }
}
