using RedacaoAPI.DTO_s;
using RedacaoAPI.Models;
using System;
using System.Text;
using System.Text.RegularExpressions;
using static RedacaoAPI.DTO_s.UserDTO;

namespace RedacaoAPI.Services
{
    public static class ValidatorService
    {
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 8)
                return false;

            string pattern = @"[!@#$%^&*(),.?""':{}|<>]";

            return Regex.IsMatch(password, pattern);
        }

        public static bool ValidateName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public static List<string> ValidateUserRegister(UserRegisterDTO user)
        {
            List<string> errors = new List<string>();

            if (!ValidateEmail(user.Email))
                errors.Add("Email inválido.");

            if (!ValidatePassword(user.Password))
                errors.Add("Senha deve ter pelo menos 8 caracteres e 1 caractere especial.");

            if (!ValidateName(user.Name))
                errors.Add("Nome não pode estar vazio.");

            return errors;
        }

        public static List<string> ValidateUserUpdate(UserUpdateDTO user)
        {
            List<string> errors = new List<string>();
 
            if (!string.IsNullOrWhiteSpace(user.Email) && !ValidateEmail(user.Email))
                errors.Add("Email inválido.");

            if (!string.IsNullOrWhiteSpace(user.Password) && !ValidatePassword(user.Password))
                errors.Add("Senha deve ter pelo menos 8 caracteres e 1 caractere especial.");

            if (!string.IsNullOrWhiteSpace(user.Name) && !ValidateName(user.Name))
                errors.Add("Nome não pode estar vazio.");

            return errors;
        }

        public static List<string> ValidateCreatePlan(PlanCreateDTO plan)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(plan.Name))
                errors.Add("Nome do plano não pode estar vazio.");

            if (plan.Price <= 0)
                errors.Add("Informe o preço do plano.");

            if (plan.MaxCorrections <= 0)
                errors.Add("Informe o número máximo de correções.");

            return errors;
        }

        public static List<string> ValidateUpdatePlan(PlanUpdateDTO plan)
        {
            List<string> errors = new List<string>();

            if (plan.Name != null && string.IsNullOrWhiteSpace(plan.Name))
                errors.Add("Nome do plano não pode estar vazio.");

            if (plan.Price != null && plan.Price <= 0)
                errors.Add("Informe o preço do plano.");

            if (plan.MaxCorrections != null && plan.MaxCorrections <= 0)
                errors.Add("Informe o número máximo de correções.");

            return errors;
        }
    }
}
