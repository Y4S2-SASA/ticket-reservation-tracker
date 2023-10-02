using TRT.Application.DTOs.Common;

namespace TRT.Application.DTOs.UserDTOs
{
    public class UserMasterDataDTO
    {
        public UserMasterDataDTO()
        {
            Roles = new List<DropDownDTO>();
            Status = new List<DropDownDTO>();
        }

        public List<DropDownDTO> Roles { get; set; }
        public List<DropDownDTO> Status { get; set; }

    }
}
