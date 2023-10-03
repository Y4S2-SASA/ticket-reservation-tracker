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
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.MobileNumber = userDTO.MobileNumber;
            user.Email = userDTO.Email;
            user.UserName = userDTO.UserName;
            user.Role = userDTO.Role;

            return user;
        }

        public static UserDetailDTO ToDetailDto(this User user, UserDetailDTO? userDetailDTO = null)
        {
            if (userDetailDTO is null) userDetailDTO = new UserDetailDTO();

            userDetailDTO.NIC = user.NIC;
            userDetailDTO.FullName = $"{user.FirstName} {user.LastName}";
            userDetailDTO.FirstName = user.FirstName;
            userDetailDTO.LastName = user.LastName;
            userDetailDTO.MobileNumber = user.MobileNumber ?? string.Empty;
            userDetailDTO.Role = EnumHelper.GetEnumDescription(user.Role);
            userDetailDTO.UserName = user.UserName;
            userDetailDTO.Email = user.Email ?? string.Empty;
            userDetailDTO.Status = EnumHelper.GetEnumDescription(user.Status);
            return userDetailDTO;
        }

        public static UserDTO ToDto(this User user, UserDTO? userDTO = null)
        {
            if (userDTO is null) userDTO = new UserDTO();

            userDTO.NIC = user.NIC;
            userDTO.FirstName = user.FirstName;
            userDTO.LastName = user.LastName;
            userDTO.MobileNumber = user.MobileNumber;
            userDTO.Email = user.Email;
            userDTO.UserName = user.Email;
            userDTO.Role = user.Role;

            return userDTO;
        }
    }
}
