using Application.Interfaces;
using Application.Models.UserDtos;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ICollection<UserResponse> GetAllUsers()
        {
            var users = UserResponse.ToDtoList(_userRepository.ListAsync().Result ?? throw new KeyNotFoundException("No se encontraron usuarios"));
            return users;
        }

        public UserResponse GetUserById(int id)
        {
            UserResponse userDto = UserResponse.ToDto(_userRepository.GetByIdAsync(id).Result ?? throw new KeyNotFoundException("No se encontró el usuario"));
            return userDto;
        }

        public User? GetUserByUserName(string userName)
        {
            return _userRepository.GetUserByUserName(userName);
        }

        public UserResponse CreateUser(UserCreateRequest dto)
        {
            return UserResponse.ToDto(_userRepository.AddAsync(UserCreateRequest.ToEntity(dto)).Result);
        }

        public void UpdateUser(int id, UserCreateRequest dto)
        {

            var existingUser = _userRepository.GetByIdAsync(id).Result ?? throw new KeyNotFoundException("No se encontró el usuario");

            if (_userRepository.GetUserByUserName(dto.UserName) != null)
                throw new InvalidOperationException("El nombre de usuario ya está en uso.");

            existingUser.Name = dto.Name;
            existingUser.Email = dto.Email;
            existingUser.Password = dto.Password;
            existingUser.Role = dto.Role;

            _userRepository.UpdateAsync(existingUser).Wait();
        }

        public void DeleteUser(int id)
        {
            var userDto = _userRepository.GetByIdAsync(id).Result ?? throw new KeyNotFoundException("No se encontró el usuario");
            _userRepository.DeleteAsync(userDto);
        }
    }
}