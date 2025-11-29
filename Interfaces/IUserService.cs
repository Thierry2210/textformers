using RedacaoAPI.Models;
using static RedacaoAPI.DTO_s.UserDTO;

namespace RedacaoAPI.Interfaces
{
    public interface IUserService
    {
        public object Authenticate(UserLoginDTO userDTO);
        public User CreateUser(UserRegisterDTO userDTO);
        public User GetUser(Guid userId);
        public User UpdateUser(Guid userId, UserUpdateDTO updatedUser);
        public void UpdatePermission(Guid userId, string permission);
        public void DeleteUser(Guid userId);
    }
}
