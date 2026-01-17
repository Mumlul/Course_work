using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;
using Test = course_work.Migrations.Test;

namespace course_work.Services;

public class TestService:ITestService
{
    private readonly ApplicationDbContext _context;

    public TestService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<Test?> GetTestByCourseIdAsync(int courseId)
    {
        throw new System.NotImplementedException();
    }

    public Task<Test?> GetTestByIdAsync(int testId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> CourseHasTestAsync(int courseId)
    {
        throw new System.NotImplementedException();
    }

    public Task<Test> CreateTestAsync(Test test)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateTestAsync(Test test)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteTestAsync(int testId)
    {
        throw new System.NotImplementedException();
    }

    public Task<TestQuestion> AddQuestionAsync(int testId, TestQuestion question)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateQuestionAsync(TestQuestion question)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteQuestionAsync(int questionId)
    {
        throw new System.NotImplementedException();
    }

    public Task<TestQuestionOption> AddOptionAsync(int questionId, TestQuestionOption option)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateOptionAsync(TestQuestionOption option)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteOptionAsync(int optionId)
    {
        throw new System.NotImplementedException();
    }

    public Task<TestResult> StartTestAsync(int testId, int userId)
    {
        throw new System.NotImplementedException();
    }

    public Task SaveUserAnswerAsync(int testResultId, int questionId, int? optionId, string? textAnswer)
    {
        throw new System.NotImplementedException();
    }

    public Task<TestResult> FinishTestAsync(int testResultId)
    {
        throw new System.NotImplementedException();
    }

    public Task<TestResult?> GetLastResultAsync(int testId, int userId)
    {
        throw new System.NotImplementedException();
    }

    public Task<TestResult> GetUserResultsAsync(int testId, int userId)
    {
        throw new System.NotImplementedException();
    }
}