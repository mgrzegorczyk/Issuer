using System.Text.Json;
using Issuer.BusinessLogic.GitLab.Models;
using Issuer.BusinessLogic.Interfaces;
using Issuer.BusinessLogic.Models;
using Issuer.BusinessLogic.Abstractions;

namespace Issuer.BusinessLogic.GitLab
{
    public sealed class GitLabService : IssuesHostingServiceBase, IIssuesHostingService
    {
        private readonly string _repositoryId;

        public GitLabService(string repositoryId, string authToken)
            : base(authToken)
        {
            _repositoryId = repositoryId;
        }

        public async Task<List<Issue>> GetIssuesAsync()
        {
            var request = CreateRequest(HttpMethod.Get,
                $"https://gitlab.com/api/v4/projects/{_repositoryId}/issues?scope=all");

            var response = await SendRequestAsync(request);

            var gitLabIssues = JsonSerializer.Deserialize<List<GitLabIssue>>(response);

            return gitLabIssues.Select(issue => new Issue(issue.Title,
                issue.Description,
                issue.State == "closed"
            )).ToList();
        }

        public async Task CloseIssueAsync(long issueId)
        {
            await UpdateIssueStateAsync(issueId, "close");
        }

        public async Task UpdateIssueTitleAsync(long issueId, string newTitle)
        {
            await UpdateIssueAsync(issueId, new { title = newTitle });
        }

        public async Task UpdateIssueDescriptionAsync(long issueId, string newDescription)
        {
            await UpdateIssueAsync(issueId, new { description = newDescription });
        }

        public async Task CreateIssueAsync(string title, string description)
        {
            var issueData = new { title, description };
            var request = CreateRequest(HttpMethod.Post,
                $"https://gitlab.com/api/v4/projects/{_repositoryId}/issues",
                issueData);

            await SendRequestAsync(request);
        }

        private async Task UpdateIssueStateAsync(long issueId, string state)
        {
            var issueData = new { state_event = state };
            var request = CreateRequest(HttpMethod.Put,
                $"https://gitlab.com/api/v4/projects/{_repositoryId}/issues/{issueId}",
                issueData);

            await SendRequestAsync(request);
        }

        private async Task UpdateIssueAsync(long issueId, object issueData)
        {
            var request = CreateRequest(HttpMethod.Put,
                $"https://gitlab.com/api/v4/projects/{_repositoryId}/issues/{issueId}",
                issueData);

            await SendRequestAsync(request);
        }

        protected override void ConfigureRequestHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("PRIVATE-TOKEN", $"{_authToken}");
            request.Headers.Add("User-Agent", "HttpClient");
        }
    }
}
