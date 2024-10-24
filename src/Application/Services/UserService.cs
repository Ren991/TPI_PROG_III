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
using Domain.Exceptions;

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
                throw new BadRequestException("Email already registered. Please try again.");
            }
            return UserDto.ToDto(_userRepository.Create(UserCreateRequest.ToEntity(userDto)));
        }

        public UserDto AddNewAdminUser(UserAdminCreateRequest userDto)
        {
            var existingUser = _userRepository.GetByEmail(userDto.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("Email already registered. Please try again.");
            }
            return UserDto.ToDto(_userRepository.Create(UserAdminCreateRequest.ToEntity(userDto)));
        }


        public UserDto GetUserByEmail(string email)
        {
            return UserDto.ToDto(_userRepository.GetByEmail(email));
        }

        public UserLoginRequest GetUserToAuthenticate(string email)
        {
            
                UserDto entity = GetUserByEmail(email);

                if (entity == null)
                {
                    throw new NotFoundException("User not found.");
                }

                UserLoginRequest entityToAuthenticate = new();
                entityToAuthenticate.Email = entity.Email;
                entityToAuthenticate.Password = entity.Password;
                entityToAuthenticate.Role = entity.Role;

                return entityToAuthenticate;

            
            
        }


        public void UpdateUser(int id, string password)
        {
            User? user = _userRepository.Get(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            user.Password = password;
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            User? user = _userRepository.Get(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            _userRepository.Delete(user);
        }
    }
}
