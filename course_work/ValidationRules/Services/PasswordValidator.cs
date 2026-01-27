using System;
using System.Threading;
using System.Threading.Tasks;
using course_work.ValidationRules.Interfaces;

namespace course_work.ValidationRules.Services;


public class PasswordValidator:IPasswordValidator
{
    public async Task Validate(string pas, Action clearErrors, Action<string> addError, CancellationToken token)
    {
        try
        {
            await Task.Delay(400, token);

            if (token.IsCancellationRequested)
                return;

            clearErrors();

            if (string.IsNullOrWhiteSpace(pas))
            {
                addError("Пароль не может быть пустым");
                return;
            }
            
            if (pas.Length < 8)
            {
                addError("Пароль должен содержать минимум 8 символов");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(pas, @"[A-Z]"))
            {
                addError("Пароль должен содержать хотя бы одну заглавную букву");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(pas, @"[\W_]"))
            {
                addError("Пароль должен содержать хотя бы один специальный символ");
                return;
            }

            if (token.IsCancellationRequested)
                return;;
            
        }
        catch (TaskCanceledException)
        {
            // Игнорируем отмену
        }
    }
    
    public void ValidateConfirmPassword(string password, string confirmPassword, Action clearErrors, Action<string> addError)
    {
        clearErrors();

        if (password != confirmPassword)
        {
            addError("Пароли не совпадают");
        }
    }
    
    public void ValidateConfirmCode(
        string password, 
        string code, 
        Action clearErrors, 
        Action<string> addError)
    {
        clearErrors();

        if (password != code)
        {
            addError("Код не верен");
        }
    }
}