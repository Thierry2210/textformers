namespace RedacaoAPI.DTO_s
{
    public class UserDTO
    {
        public class UserRegisterDTO
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public int PlanId { get; set; }
        }

        public class UserLoginDTO {       
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UserUpdateDTO
        {
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
            public int? PlanId { get; set; }
        }

        public class UpdateUserPermissionDTO
        {
            public Guid Id { get; set; }
            public string Permission { get; set; }
        }
    }
}
