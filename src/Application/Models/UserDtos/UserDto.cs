using Domain.Entities;

using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.UserDtos
{
    public class UserDto
    {
        public string Name { get; set; }

        public string LastName { get; set; }    
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public static UserDto ToDto(User user)
        {
            UserDto userDto = new();
            userDto.Name = user.Name;
            userDto.LastName = user.LastName;
            userDto.Email = user.Email;
            userDto.Password = user.Password;
            userDto.Role = user.Role;
            userDto.Carts = user.Carts;

            return userDto;

        }

    }
}