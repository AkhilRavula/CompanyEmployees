using AutoMapper;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contract;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;
        private readonly UserManager<User> usermanager;
        private readonly IOptions<JwtConfiguration> configuration;
        private readonly JwtConfiguration jwtConfiguration;
        private User? _user;

        public AuthenticationService(ILoggerManager logger,IMapper
            mapper,UserManager<User> usermanager, IOptions<JwtConfiguration> configuration)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.usermanager = usermanager;
            this.configuration = configuration;
            jwtConfiguration = configuration.Value;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials(); 
            var claims = await GetClaims(); 
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        { 
           // var jwtSettings = configuration.GetSection("JwtSettings"); 

            var tokenOptions = new JwtSecurityToken(issuer: jwtConfiguration.ValidIssuer,
                audience: jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfiguration.expires)),
                signingCredentials: signingCredentials); 

            return tokenOptions;
        }
        private SigningCredentials GetSigningCredentials()
        { 
            var key = Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey);
            var secret = new SymmetricSecurityKey(key); 
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256); 
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim> 
            { 
            new Claim(ClaimTypes.Name, _user.UserName) }; 

            var roles = await usermanager.GetRolesAsync(_user);
            foreach (var role in roles)
            { 
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = mapper.Map<User>(userForRegistrationDto);

            var result = await usermanager.CreateAsync(user,userForRegistrationDto.Password);

            if(result.Succeeded)
            {
                await usermanager.AddToRolesAsync(user, userForRegistrationDto.Roles);
            }

            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto)
        {
            _user = await usermanager.FindByNameAsync(userForAuthenticationDto.UserName);

            var result = (_user != null && await usermanager.CheckPasswordAsync(_user, userForAuthenticationDto.Password));
            
            if(!result)
            {
                logger.LogWarn($"{nameof(ValidateUser)}: Invalid Username Password");
            }

            return result;
        
        }

    }
}
