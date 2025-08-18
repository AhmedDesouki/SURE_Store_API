using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SURE_Store_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SURE_Store_API.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public JwtService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;    // Store configuration reference
            _userManager = userManager;        // Store user manager reference
        }
        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            // Get the user's roles from the user manager
            var roles = await _userManager.GetRolesAsync(user);

            // Create a list of claims (user information) to include in the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),        // User ID claim
                new Claim(ClaimTypes.Email, user.Email!),             // Email claim
                new Claim(ClaimTypes.Name, user.FullName),            // Full name claim
                new Claim("FullName", user.FullName)                  // Custom full name claim
            };
            // Add each user role as a claim for authorization purposes
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));  // Role claim
            }

            // Create the security key from the JWT secret key in configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            // Create signing credentials using HMAC SHA256 algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token with all necessary information
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],         // Token issuer (this application)
                audience: _configuration["Jwt:Audience"],     // Token audience (client applications)
                claims: claims,                               // User claims (ID, email, name, roles)
                expires: DateTime.UtcNow.AddMinutes(30),      // Token expiration (30 minutes from now)
                signingCredentials: creds                     // Signing credentials for token validation
            );

            // Convert the token to a string and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            // Create a byte array to hold the random token data
            var randomNumber = new byte[64];

            // Use cryptographically secure random number generator
            using var rng = RandomNumberGenerator.Create();

            // Fill the byte array with random bytes
            rng.GetBytes(randomNumber);

            // Convert the random bytes to a base64 string and return
            return Convert.ToBase64String(randomNumber);
        }
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            // Create token validation parameters for expired token validation
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,     // Don't validate audience for expired tokens
                ValidateIssuer = false,       // Don't validate issuer for expired tokens
                ValidateIssuerSigningKey = true,  // Always validate the signing key
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),  // Signing key
                ValidateLifetime = false      // Don't validate lifetime (token is expired)
            };
            // Create JWT token handler for validation
            var tokenHandler = new JwtSecurityTokenHandler();

            // Validate the token and extract the principal
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            // Additional validation: ensure the token uses the correct algorithm
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;  // Return null if algorithm doesn't match
            }

            // Return the user principal if all validations pass
            return principal;
        } 
    }
}
