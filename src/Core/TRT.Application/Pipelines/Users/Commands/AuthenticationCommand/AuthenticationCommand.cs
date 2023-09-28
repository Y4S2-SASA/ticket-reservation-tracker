using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.AuthenticationDTOs;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Users.Commands.AuthenticationCommand
{
    public class AuthenticationCommand : IRequest<UserAuthenticationResponseDTO>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, UserAuthenticationResponseDTO>
    {
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationCommandHandler(IUserQueryRepository userQueryRepository, IConfiguration configuration)
        {
            this._userQueryRepository = userQueryRepository;
            this._configuration = configuration;
        }
        public async Task<UserAuthenticationResponseDTO> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            try
            {
               
                var user = (await _userQueryRepository.Query(x=>x.UserName.ToLower() == request.UserName.ToLower() && 
                            x.Status == Domain.Enums.Status.Activated)).FirstOrDefault();

                if (user is null)
                {
                    return UserAuthenticationResponseDTO.NotSuccess(ResponseMessageConstant.USER_DOES_NOT_EXIST_RESPONSE);
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return UserAuthenticationResponseDTO.NotSuccess(ResponseMessageConstant.PASSWORD_INCORRECT_RESPONSE);
                }

                var userAthunticationResponse = await ConfigureJwtToken(user, cancellationToken);

                if (userAthunticationResponse.IsLoginSuccess)
                {
                    return userAthunticationResponse;
                }
                else
                {
                    return UserAuthenticationResponseDTO.NotSuccess(ResponseMessageConstant.COMMON_EXCEPTION_RESPONSE_MESSAGE);
                }
            }
            catch (Exception ex)
            {

                return UserAuthenticationResponseDTO.NotSuccess(ResponseMessageConstant.COMMON_EXCEPTION_RESPONSE_MESSAGE);
            }
        }

        private async Task<UserAuthenticationResponseDTO> ConfigureJwtToken(User user, CancellationToken cancellationToken)
        {
            var key = _configuration["Tokens:Key"];
            var issuer = _configuration["Tokens:Issuer"];

            string role = EnumHelper.GetEnumDescription(user.Role);

            var now = DateTime.UtcNow;
            DateTime nowDate = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.NIC.ToString()),
                        new Claim("firstName",string.IsNullOrEmpty(user.FirstName)? "": user.FirstName),
                        new Claim("role",role),
                        new Claim(JwtRegisteredClaimNames.Aud,"webapp"),
                        new Claim(JwtRegisteredClaimNames.Aud,"mobileapp"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken
            (
                issuer: issuer,
                claims: claims,
                expires: nowDate.AddYears(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler()
                            .WriteToken(token);

           

            return UserAuthenticationResponseDTO.Success
                (
                    tokenString,
                    $"{user.FirstName} {user.LastName}",
                    user.NIC,
                    role
                );
        }
    }
}
