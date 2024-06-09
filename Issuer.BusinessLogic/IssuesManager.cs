using System.Text.Json;
using Issuer.BusinessLogic.Interfaces;
using Issuer.BusinessLogic.Models;

namespace Issuer.BusinessLogic;

public class IssuesManager
{
    private readonly IIssuesHostingService _issuesHostingService;

    public IssuesManager(IIssuesHostingService issuesHostingService)
    {
        _issuesHostingService = issuesHostingService;
    }

    public async Task ExportIssuesAsync(string filePath)
    {
        var issues = await _issuesHostingService.GetIssuesAsync();
        var json = JsonSerializer.Serialize(issues);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task CloseIssueAsync(Int64 number)
    {
        await _issuesHostingService.CloseIssueAsync(number);
    }

    public async Task SetIssueNameAsync(Int64 number, string newName)
    {
        await _issuesHostingService.UpdateIssueTitleAsync(number, newName);
    }

    public async Task SetIssueDescriptionAsync(Int64 number, string newDescription)
    {
        await _issuesHostingService.UpdateIssueDescriptionAsync(number, newDescription);
    }

    public async Task CreateIssueAsync(string title, string description)
    {
        await _issuesHostingService.CreateIssueAsync(title, description);
    }

    public async Task ImportIssuesAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var issues = JsonSerializer.Deserialize<List<Issue>>(json);

        var tasks = issues.Select(issue => CreateIssueAsync(issue.Title, issue.Description)).ToList();
        await Task.WhenAll(tasks);
    }
}