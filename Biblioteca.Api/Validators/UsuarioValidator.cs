using Biblioteca.Api.DTOs.Usuario;
using System.Text.RegularExpressions;

namespace Biblioteca.Api.Validators
{
    public class UsuarioValidator
    {
        public Dictionary<string, string> ValidateCreate(UsuarioCreateRequest usuario)
        {
            Dictionary<string, string> errors = [];

            if (string.IsNullOrEmpty(usuario.Nome))
                errors.Add("nome", "O nome é obrigatório");

            if (string.IsNullOrEmpty(usuario.Cpf))
                errors.Add("cpf", "O cpf é obrigatório");
            else
            {
                if (usuario.Cpf.Length != 11)
                    errors.Add("cpf", "O cpf deve conter 11 dígitos");
                else if (!usuario.Cpf.All(char.IsDigit))
                    errors.Add("cpf", "O cpf deve conter apenas números");
            }

            if (string.IsNullOrEmpty(usuario.Email))
                errors.Add("email", "O email é obrigatório");
            else
            {
                if (!Regex.IsMatch(usuario.Email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    errors.Add("email", "E-mail inválido");
                }
            }

            if (string.IsNullOrEmpty(usuario.Senha))
                errors.Add("senha", "A senha é obrigatória");
            else if (usuario.Senha.Length < 6)
                errors.Add("senha", "A senha deve possuir no mínimo 6 caracteres");

            if (string.IsNullOrEmpty(usuario.ConfirmarSenha))
                errors.Add("confirmarSenha", "A confirmação senha é obrigatória");
            else if (usuario.ConfirmarSenha.Length < 6)
                errors.Add("confirmarSenha", "A senha deve possuir no mínimo 6 caracteres");

            if (!errors.ContainsKey("senha") &&
                !errors.ContainsKey("confirmarSenha") &&
                usuario.Senha != usuario.ConfirmarSenha)
            {
                errors.Add("confirmarSenha", "As senhas não coincidem");
            }

            return errors;
        }
        public Dictionary<string, string> ValidateUpdate(UsuarioUpdateRequest usuario)
        {
            Dictionary<string, string> errors = [];

            if (string.IsNullOrEmpty(usuario.Nome))
                errors.Add("nome", "O nome é obrigatório");

            if (string.IsNullOrEmpty(usuario.Cpf))
                errors.Add("cpf", "O cpf é obrigatório");
            else
            {
                if (usuario.Cpf.Length != 11)
                    errors.Add("cpf", "O cpf deve conter 11 dígitos");
                else if (!usuario.Cpf.All(char.IsDigit))
                    errors.Add("cpf", "O cpf deve conter apenas números");
            }

            if (string.IsNullOrEmpty(usuario.Email))
                errors.Add("email", "O email é obrigatório");
            else
            {
                if (!Regex.IsMatch(usuario.Email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    errors.Add("email", "E-mail inválido");
                }
            }
            
            return errors;
        }
    }
}
