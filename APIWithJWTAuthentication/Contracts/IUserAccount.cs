using SharedLibrary.DTOs;
using static SharedLibrary.DTOs.ServiceResponse;

namespace APIWithJWTAuthentication.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDTO userDTO);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
    }
}
