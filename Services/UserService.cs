using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;
using System.Security.Cryptography;
using System.Text;
using static RedacaoAPI.DTO_s.UserDTO;

namespace RedacaoAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public object Authenticate(UserLoginDTO userDTO)
        {
            if (!ValidatorService.ValidateEmail(userDTO.Email))
                throw new ArgumentException("E-mail inválido.");

            User? user = _repository.GetByEmail(userDTO.Email);

            if (user == null || !PasswordService.Verify(userDTO.Password, user.Password))
                throw new UnauthorizedAccessException("Usuário ou senha incorreta.");

            return TokenService.GenerateToken(user);
        }

        public User CreateUser(UserRegisterDTO userDTO)
        {
            User? existingUser = _repository.GetByEmail(userDTO.Email);

            if (existingUser != null)
                throw new Exception("Já existe um usuário cadastrado com este e-mail.");

            List<string> errors = ValidatorService.ValidateUserRegister(userDTO);
            if (errors.Any())
                throw new ArgumentException(string.Join(" ", errors));
                
            userDTO.Password = PasswordService.Hash(userDTO.Password);

            User user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                PlanId = userDTO.PlanId,
                UserPermission = "User"
            };

            _repository.Create(user);
            return user;
        }

        public User UpdateUser(Guid userId, UserUpdateDTO updatedUser)
        {
            User user = GetUser(userId);

            List<string> errors = ValidatorService.ValidateUserUpdate(updatedUser);
            if (errors.Any())
                throw new Exception(string.Join(" ", errors));

            user.Name = updatedUser.Name ?? user.Name;
            user.Email = updatedUser.Email ?? user.Email;
            user.PlanId = updatedUser.PlanId ?? user.PlanId;

            if (!string.IsNullOrEmpty(updatedUser.Password))
                user.Password = PasswordService.Hash(updatedUser.Password); 


            _repository.Update(user);
            return user;
        }

        public User GetUser(Guid userId)
        {
            return _repository.GetById(userId) ?? throw new Exception("Usuário não encontrado.");
        }

        public void DeleteUser(Guid userId)
        {
            User user = GetUser(userId);
            user.DeletedAt = DateTime.UtcNow.AddHours(-3);

            _repository.Update(user);
        }

        public void UpdatePermission(Guid userId, string permission)
        {
            if (permission != "Admin" && permission != "User")
                throw new ArgumentException("Permissão inválida.");

            var user = GetUser(userId);
            user.UserPermission = permission;

            _repository.Update(user);
        }
    }
}