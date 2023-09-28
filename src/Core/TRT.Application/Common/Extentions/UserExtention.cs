using TRT.Application.Common.Helpers;
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
            user.FirstName = userDTO.FistName;
            user.LastName = userDTO.LastName;
            user.MobileNumber = userDTO.MobileNumber;
            user.Email = userDTO.Email;
            user.UserName = userDTO.Email;
            user.Status = TRT.Domain.Enums.Status.Activated;
            user.Role = userDTO.Role;

            return user;
        }

        public static UserDetailDTO ToDto(this User user, UserDetailDTO? userDetailDTO = null)
        {
            if (userDetailDTO is null) userDetailDTO = new UserDetailDTO();

            userDetailDTO.NIC = user.NIC;
            userDetailDTO.FullName = $"{user.FirstName} {user.LastName}";
            userDetailDTO.FirstName = user.FirstName;
            userDetailDTO.LastName = userDetailDTO.LastName;
            userDetailDTO.MobileNumber = user.MobileNumber;
            userDetailDTO.Role = EnumHelper.GetEnumDescription(user.Role);
            userDetailDTO.UserName = user.UserName;
            userDetailDTO.Email = user.Email;
            
            return userDetailDTO;
        }
    }
}
