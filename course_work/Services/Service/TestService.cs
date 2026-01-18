using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;
using Microsoft.EntityFrameworkCore;


namespace course_work.Services;

public class TestService:ITestService
{
    private readonly ApplicationDbContext _context;

    public TestService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Test?> GetTestByCourseIdAsync(int courseId)
    {
        return await _context.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(t => t.CourseId == courseId && t.IsActive);
    }

    public async Task<Test?> GetTestByIdAsync(int testId)
    {
        return await _context.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(t => t.Id == testId);
    }

    public async Task<bool> CourseHasTestAsync(int courseId)
    {
        return await _context.Tests.AnyAsync(t => t.CourseId == courseId);
    }

    public async Task<Test> CreateTestAsync(Test test)
    {
        test.CreatedAt = DateTime.UtcNow;
        test.UpdatedAt = DateTime.UtcNow;

        _context.Tests.Add(test);
        await _context.SaveChangesAsync();
        return test;
    }

    public async Task UpdateTestAsync(Test test)
    {
        test.UpdatedAt = DateTime.UtcNow;
        _context.Tests.Update(test);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTestAsync(int testId)
    {
        var test = await _context.Tests.FindAsync(testId);
        if (test == null) return;

        _context.Tests.Remove(test);
        await _context.SaveChangesAsync();
    }

    public async Task<TestQuestion> AddQuestionAsync(int testId, TestQuestion question)
    {
        question.TestId = testId;
        question.CreatedAt = DateTime.UtcNow;

        _context.TestQuestions.Add(question);
        await _context.SaveChangesAsync();
        return question;
    }

    public async Task UpdateQuestionAsync(TestQuestion question)
    {
        _context.TestQuestions.Update(question);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteQuestionAsync(int questionId)
    {
        var question = await _context.TestQuestions.FindAsync(questionId);
        if (question == null) return;

        _context.TestQuestions.Remove(question);
        await _context.SaveChangesAsync();
    }

    public async Task<TestQuestionOption> AddOptionAsync(int questionId, TestQuestionOption option)
    {
        option.QuestionId = questionId;
        _context.TestQuestionOptions.Add(option);
        await _context.SaveChangesAsync();
        return option;
    }

    public async Task UpdateOptionAsync(TestQuestionOption option)
    {
        _context.TestQuestionOptions.Update(option);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOptionAsync(int optionId)
    {
        var option = await _context.TestQuestionOptions.FindAsync(optionId);
        if (option == null) return;

        _context.TestQuestionOptions.Remove(option);
        await _context.SaveChangesAsync();
    }

    public async Task<TestResult> StartTestAsync(int testId, int userId)
    {
        var attempt = await _context.TestResults
            .Where(r => r.TestId == testId && r.UserId == userId)
            .CountAsync() + 1;

        var result = new TestResult
        {
            TestId = testId,
            UserId = userId,
            AttemptNumber = attempt,
            StartedAt = DateTime.UtcNow,
            Passed = false,
            Score = 0,
            AnswersJson = JsonSerializer.Serialize(new Dictionary<int, object>())
        };

        _context.TestResults.Add(result);
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task SaveUserAnswerAsync(int testResultId, int questionId, int? optionId, string? textAnswer)
    {
        /*var result = await _context.TestResults.FindAsync(testResultId);
        if (result == null) return;

        var answers = string.IsNullOrEmpty(result.AnswersJson)
            ? new Dictionary<int, object>()
            : JsonSerializer.Deserialize<Dictionary<int, object>>(result.AnswersJson)!;

        answers[questionId] = optionId ?? textAnswer!;
        result.AnswersJson = JsonSerializer.Serialize(answers);

        await _context.SaveChangesAsync();*/
    }

    public async Task<TestResult> FinishTestAsync(int testResultId)
    {
        var result = await _context.TestResults
            .Include(r => r.Test)
            .ThenInclude(t => t.Questions)
            .ThenInclude(q => q.Options)
            .FirstAsync(r => r.Id == testResultId);

        var answers = JsonSerializer.Deserialize<Dictionary<int, JsonElement>>(result.AnswersJson!);
        decimal totalPoints = 0;
        decimal earnedPoints = 0;

        foreach (var q in result.Test.Questions)
        {
            totalPoints += q.Points;

            if (!answers!.ContainsKey(q.Id))
                continue;

            if (q.QuestionType == QuestionType.TextAnswer)
            {
                earnedPoints += q.Points;
            }
            else
            {
                var correctIds = q.Options.Where(o => o.IsCorrect).Select(o => o.Id).ToList();
                var userAnswer = answers[q.Id];

                if (q.QuestionType == QuestionType.SingleChoice &&
                    correctIds.Contains(userAnswer.GetInt32()))
                {
                    earnedPoints += q.Points;
                }
            }
        }

        result.Score = totalPoints == 0 ? 0 : earnedPoints / totalPoints * 100;
        result.Passed = result.Score >= result.Test.PassingScore;
        result.CompletedAt = DateTime.UtcNow;
        result.TimeSpentSeconds =
            (int)(result.CompletedAt.Value - result.StartedAt).TotalSeconds;

        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<TestResult?> GetLastResultAsync(int testId, int userId)
    {
        return await _context.TestResults
            .Where(r => r.TestId == testId && r.UserId == userId)
            .OrderByDescending(r => r.AttemptNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<TestResult> GetUserResultsAsync(int testId, int userId)
    {
        return await _context.TestResults
            .Where(r => r.TestId == testId && r.UserId == userId)
            .OrderByDescending(r => r.Score)
            .FirstAsync();
    }
}