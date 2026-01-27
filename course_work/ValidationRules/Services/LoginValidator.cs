using System;
using System.Threading;
using System.Threading.Tasks;
using course_work.ValidationRules.Interfaces;

namespace course_work.ValidationRules.Services;

public class LoginValidator:ILoginValidator
{
    public async Task ValidateAsync(string login, Func<string, Task<bool>> existsFunc, Action clearErrors, Action<string> addError, CancellationToken token)
    {
        try
        {
            await Task.Delay(400, token);

            if (token.IsCancellationRequested)
                return;

            clearErrors();

            if (string.IsNullOrWhiteSpace(login))
            {
                addError("Логин не может быть пустым");
                return;
            }

            var exists = await existsFunc(login);

            if (token.IsCancellationRequested)
                return;

            if (exists)
            {
                addError("Логин уже занят");
            }
        }
        catch (TaskCanceledException)
        {
            // Игнорируем отмену
        }
    }
}