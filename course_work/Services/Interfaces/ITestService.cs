using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models.Classes;
using User = course_work.Migrations.User;

namespace course_work.Services;

public interface ITestService
{
    Task<Test> GetTestByCourseIdAsync(int courseId);
    Task<Test?> GetTestByIdAsync(int testId);
    Task<bool> CourseHasTestAsync(int courseId);
    Task<Test> CreateTestAsync(Test test);
    Task UpdateTestAsync(Test test);
    Task DeleteTestAsync(int testId);
    Task<TestQuestion> AddQuestionAsync(int testId, TestQuestion question);
    Task UpdateQuestionAsync(TestQuestion question);
    Task DeleteQuestionAsync(int questionId);
    Task AddOptionAsync(TestQuestionOption option);
    Task UpdateOptionAsync(TestQuestionOption option);
    Task DeleteOptionAsync(int optionId);
    Task<TestResult> StartTestAsync(int testId, int userId);
    Task SaveUserAnswerAsync(
        int testResultId,
        int questionId,
        int? optionId,
        string? textAnswer
    );
    Task<TestResult> FinishTestAsync(int testResultId);
    Task<TestResult?> GetLastResultAsync(int testId, int userId);
    Task<TestResult> GetUserResultsAsync(int testId, int userId);
    Task<List<TestQuestionOption>>  GetQuestionOptionsAsync(int questionId);
    
}