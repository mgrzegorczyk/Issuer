using System.Text.Json;
using Issuer.BusinessLogic.Interfaces;

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
}