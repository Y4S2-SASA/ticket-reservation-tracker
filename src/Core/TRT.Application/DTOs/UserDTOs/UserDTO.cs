using TRT.Domain.Enums;

namespace TRT.Application.DTOs.UserDTOs
{
    public class UserDTO
    {
       
        public string NIC { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }

    }
}
