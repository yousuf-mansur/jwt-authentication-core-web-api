using SharedLibrary.DTO_s;
using static SharedLibrary.DTO_s.ServiceResponse;

namespace APIWithJWTAuthentication.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDTO userDTO);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
    }
}
