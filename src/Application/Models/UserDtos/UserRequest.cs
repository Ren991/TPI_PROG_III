using Domain.Entities;
using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.UserDtos
{
    public class UserCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public Role Role { get; set; }

        public static User ToEntity(UserCreateRequest dto)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role,
            };
        }

        public static bool validateDto(UserCreateRequest dto)
        {
            if (dto.Name == default ||
                dto.Email == default ||
                dto.Password == default)
                return false;

            return true;
        }
    }
}