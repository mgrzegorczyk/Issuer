using Issuer.BusinessLogic.Models;

namespace Issuer.BusinessLogic.Interfaces;

public interface IIssuesHostingService
{
    Task<List<Issue>> GetIssuesAsync();
    Task CloseIssueAsync(Int64 issueNumber);
    Task UpdateIssueTitleAsync(Int64 issueId, string newTitle);
    Task UpdateIssueDescriptionAsync(Int64 issueId, string newDescription);
    Task CreateIssue(string title, string description);
}
