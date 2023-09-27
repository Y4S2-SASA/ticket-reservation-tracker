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
            user.FistName = userDTO.FistName;
            user.LastName = userDTO.LastName;
            user.MobileNumber = userDTO.MobileNumber;
            user.Email = userDTO.Email;
            user.UserName = userDTO.Email;
            user.Status = TRT.Domain.Enums.Status.Activated;
            user.Role = userDTO.Role;

            return user;
        }
    }
}
