using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Entities;

namespace System
{
    public static class UserExtention
    {
        public static User ToEntity(this UserDTO userDTO, User? user = null)
        {
            if (user is null) user = new User();

            user.NIC = userDTO.NIC;
            user.Name = userDTO.Name;
            user.Email = userDTO.Email;
            user.UserName = userDTO.Email;
            user.Role = userDTO.Role;
            user.IsActive = true;

            return user;
        }
    }
}
