﻿using Application.Interfaces;
using Application.Models;
using Application.Models;
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
            return _userRepository.Get();
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

        public UserDto GetUserByEmail(string email)
        {
            return UserDto.ToDto(_userRepository.GetByEmail(email));
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