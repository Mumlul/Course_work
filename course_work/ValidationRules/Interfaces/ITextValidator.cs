using System;
using System.Threading;
using System.Threading.Tasks;

namespace course_work.ValidationRules.Interfaces;

public interface ITextValidator
{
    Task Validate(
        string login,
        Action clearErrors,
        Action<string> addError,
        CancellationToken token);
}