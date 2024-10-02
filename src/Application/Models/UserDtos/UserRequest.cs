using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.UserDtos
{ 
    public class UserCreateRequest
    {
        public string Name { get; set; }

        public string LastName { get; set; }    

        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

      
        public static User ToEntity(UserCreateRequest userDto)
        {
            User user = new User();
            user.Name = userDto.Name;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.Role = userDto.Role;
            Cart cart = new Cart();
            user.Carts.Add(cart);

            return user;

        }
    }
}