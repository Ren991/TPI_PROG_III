using Application.Interfaces;
using Application.Models;
using Application.Models.AuthDtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models.UserDtos;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            var users = _userRepository.Get();
            return users;
        }

        public UserDto AddNewUser(UserCreateRequest userDto)
        {
            var existingUser = _userRepository.GetByEmail(userDto.Email);
            if (existingUser != null)
            {
                throw new Exception("Email ya registrado. Por favor intente nuevamente");
            }
            return UserDto.ToDto(_userRepository.Create(UserCreateRequest.ToEntity(userDto)));
        }

        public UserDto AddNewAdminUser(UserAdminCreateRequest userDto)
        {
            var existingUser = _userRepository.GetByEmail(userDto.Email);
            if (existingUser != null)
            {
                throw new Exception("Email ya registrado. Por favor intente nuevamente");
            }
            return UserDto.ToDto(_userRepository.Create(UserAdminCreateRequest.ToEntity(userDto)));
        }


        public UserDto GetUserByEmail(string email)
        {
            return UserDto.ToDto(_userRepository.GetByEmail(email));
        }

        public UserLoginRequest GetUserToAuthenticate(string email)
        {
            try
            {
                UserDto entity = GetUserByEmail(email);

                if (entity == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }

                UserLoginRequest entityToAuthenticate = new();
                entityToAuthenticate.Email = entity.Email;
                entityToAuthenticate.Password = entity.Password;
                entityToAuthenticate.Role = entity.Role;

                return entityToAuthenticate;

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }


        public void UpdateUser(int id, string password)
        {
            User? user = _userRepository.Get(id);
            if (user == null)
            {
                throw new Exception("No se encontró el usuario");
            }
            user.Password = password;
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            User? user = _userRepository.Get(id);
            if (user == null)
            {
                throw new Exception("No se encontró el usuario");
            }
            _userRepository.Delete(user);
        }
    }
}
