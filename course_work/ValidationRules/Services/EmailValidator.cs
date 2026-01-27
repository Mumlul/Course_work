using System;
using System.Threading;
using System.Threading.Tasks;
using course_work.ValidationRules.Interfaces;

namespace course_work.ValidationRules.Services;

public class EmailValidator:IEmailValidator
{
    public async Task ValidateAsync(
        string email,
        Func<string, Task<bool>> existsFunc,
        Action clearErrors,
        Action<string> addError,
        CancellationToken token)
    {
        clearErrors();

        if (string.IsNullOrWhiteSpace(email))
        {
            addError("Email не может быть пустым");
            return;
        }

        if (!IsValidEmail(email))
        {
            addError("Некорректный формат email");
            return;
        }

        await Task.Delay(400, token);

        if (token.IsCancellationRequested)
            return;

        if (await existsFunc(email))
            addError("Email уже используется");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}