using System;
using System.Threading;
using System.Threading.Tasks;
using course_work.ValidationRules.Interfaces;

namespace course_work.ValidationRules.Services;

public class TextValidator:ITextValidator
{
    public async Task Validate(string text, Action clearErrors, Action<string> addError, CancellationToken token)
    {
        try
        {
            await Task.Delay(400, token);

            if (token.IsCancellationRequested)
                return;

            clearErrors();

            if (string.IsNullOrWhiteSpace(text))
            {
                addError("Поле не может быть пустым");
                return;
            }
            
            if (token.IsCancellationRequested)
                return;
            
        }
        catch (TaskCanceledException)
        {
            // Игнорируем отмену
        }
    }
}