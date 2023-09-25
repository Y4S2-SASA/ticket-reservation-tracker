namespace TRT.Application.DTOs.AuthenticationDTOs
{
    public class UserAuthenticationResponseDTO
    {
        internal UserAuthenticationResponseDTO(bool isLoginSuccess, string token, string displayName, string userId, string message, string role)
        {
            IsLoginSuccess = isLoginSuccess;
            Token = token;
            DisplayName = displayName;
            UserId = userId;
            Message = message;
            Role = role;
        }

        public bool IsLoginSuccess { get; set; }
        public string Token { get; set; }
        public string DisplayName { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }


        public static UserAuthenticationResponseDTO NotSuccess(string errorMessage)
        {
            return new UserAuthenticationResponseDTO(false, string.Empty, string.Empty, string.Empty, errorMessage, string.Empty);
        }

        public static UserAuthenticationResponseDTO Success(string token, string displayName, string userId, string role)
        {
            return new UserAuthenticationResponseDTO(true, token, displayName, userId, "User login is successfully.", role);
        }
    }
}
