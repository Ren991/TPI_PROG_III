using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Domain.Entities;
using Application.Models.AuthDtos;
using Domain.Enums;
using Domain.Exceptions;

namespace Infrastructure.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationServiceOptions _options;
        public AuthenticationService(IUserRepository userRepository, IOptions<AuthenticationServiceOptions> options)
        {
            _userRepository = userRepository;
            _options = options.Value;
        }

        private User? ValidateUser(UserLoginRequest authenticationRequest)
        {
            if (string.IsNullOrEmpty(authenticationRequest.Email) || string.IsNullOrEmpty(authenticationRequest.Password))
                return null;

            var user = _userRepository.GetByEmail(authenticationRequest.Email);

            if (user == null) return null;

            if (user == null) return null;

            if (authenticationRequest.Email == user.Email && user.Password == authenticationRequest.Password) return user;

            return null;

        }

        public string AuthenticateAsync(UserLoginRequest authenticationRequest)
        {
                //Paso 1: Validamos las credenciales
                var user = ValidateUser(authenticationRequest); //Lo primero que hacemos es llamar a una función que valide los parámetros que enviamos.

                if (user == null)
                {
                    throw new NotAllowedException("Unauthenticated user.");
                    
                }


                //Paso 2: Crear el token
                var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey));

                var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

                //Los claims son datos en clave->valor que nos permite guardar data del usuario.
                var claimsForToken = new List<Claim>();
                claimsForToken.Add(new Claim("sub", user.Id.ToString()));
                claimsForToken.Add(new Claim("given_name", user.Name));
                claimsForToken.Add(new Claim("family_name", user.LastName));
                claimsForToken.Add(new Claim("role", user.Role.ToString()));


                var jwtSecurityToken = new JwtSecurityToken(
                  _options.Issuer,
                  _options.Audience,
                  claimsForToken,
                  DateTime.UtcNow,
                  DateTime.UtcNow.AddHours(1),
                  credentials);

                var tokenToReturn = new JwtSecurityTokenHandler()
                    .WriteToken(jwtSecurityToken);

                return tokenToReturn.ToString();

                      
            
        }

        private UserLoginRequest GetUserToAuthenticate(string email, string password)
        {
            User entity = _userRepository.GetByEmail(email);

            if (entity == null)
            {
                return null;
                
            }

            UserLoginRequest entityToAuthenticate = new();
            entityToAuthenticate.Email = entity.Email;
            entityToAuthenticate.Role = entity.Role;
            entityToAuthenticate.Password = password;

            return entityToAuthenticate;
        }

        public string Autenticar(UserLoginRequest loginRequest)
        {
            UserLoginRequest authenticationRequest = GetUserToAuthenticate(loginRequest.Email, loginRequest.Password);

            //Paso 1: Validamos las credenciales
            var user = ValidateUser(authenticationRequest); //Lo primero que hacemos es llamar a una función que valide los parámetros que enviamos.

            if (user == null)
            {
                return null;

            }


            //Paso 2: Crear el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey)); //Traemos la SecretKey del Json. agregar antes: using Microsoft.IdentityModel.Tokens;

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            //Los claims son datos en clave->valor que nos permite guardar data del usuario.
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString())); //"sub" es una key estándar que significa unique user identifier, es decir, si mandamos el id del usuario por convención lo hacemos con la key "sub".
            claimsForToken.Add(new Claim("given_name", user.Email)); //Lo mismo para given_name y family_name, son las convenciones para nombre y apellido. Ustedes pueden usar lo que quieran, pero si alguien que no conoce la app
            claimsForToken.Add(new Claim("role", user.Role.ToString()));

            var jwtSecurityToken = new JwtSecurityToken( //agregar using System.IdentityModel.Tokens.Jwt; Acá es donde se crea el token con toda la data que le pasamos antes.
              _options.Issuer,
              _options.Audience,
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              credentials);

            var tokenToReturn = new JwtSecurityTokenHandler() //Pasamos el token a string
                .WriteToken(jwtSecurityToken);

            return tokenToReturn.ToString();
        }
        public class AuthenticationServiceOptions
        {
            public const string AuthenticationService = "AuthenticacionService";

            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string SecretForKey { get; set; }
        }

    }
    

}
