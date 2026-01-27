using System;
using System.Threading;
using System.Threading.Tasks;

namespace course_work.ValidationRules.Interfaces;

public interface IEmailValidator
{
    Task ValidateAsync(
        string email,
        Func<string, Task<bool>> existsFunc,
        Action clearErrors,
        Action<string> addError,
        CancellationToken token);
}