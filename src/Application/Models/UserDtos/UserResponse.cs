using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;

namespace Application.Models.UserDtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
        public DateTime UserRegistrationDate { get; set; }
        public DateTime? UserDeletionDate { get; set; }
        //public ICollection<CartResponse> Carts { get; set; } = new List<CartResponse>();

        public static UserResponse ToDto(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                //UserRegistrationDate = user.UserRegistrationDate,
                //UserDeletionDate = user.UserDeletionDate,
                //Carts = CartResponse.ToDtoList(user.Carts ?? new List<Cart>()).ToList()
            };
        }

        public static ICollection<UserResponse> ToDtoList(IEnumerable<User> users)
        {
            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                //Carts = CartResponse.ToDtoList(user.Carts ?? new List<Cart>()).ToList()
            }).ToList();
        }
    }
}