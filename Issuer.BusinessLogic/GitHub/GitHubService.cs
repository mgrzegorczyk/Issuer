using System.Text.Json;
using Issuer.BusinessLogic.GitHub.Models;
using Issuer.BusinessLogic.Interfaces;
using Issuer.BusinessLogic.Models;
using Issuer.BusinessLogic.Abstractions;

namespace Issuer.BusinessLogic.Github
{
    public sealed class GitHubService : IssuesHostingServiceBase, IIssuesHostingService
    {
        private readonly string _repositoryOwner;
        private readonly string _repositoryName;

        public GitHubService(string repositoryOwner, string repositoryName, string authToken)
            : base(authToken)
        {
            _repositoryOwner = repositoryOwner;
            _repositoryName = repositoryName;
        }

        public async Task<List<Issue>> GetIssuesAsync()
        {
            var request = CreateRequest(HttpMethod.Get,
                $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues?state=all");

            var response = await SendRequestAsync(request);

            var gitHubIssues = JsonSerializer.Deserialize<List<GitHubIssue>>(response);

            return gitHubIssues.Select(issue => new Issue(issue.Title,
                issue.Body,
                issue.State == "closed"
            )).ToList();
        }

        public async Task CloseIssueAsync(long issueNumber)
        {
            await UpdateIssueStateAsync(issueNumber, "closed");
        }

        public async Task UpdateIssueTitleAsync(long issueId, string newTitle)
        {
            await UpdateIssueAsync(issueId, new { title = newTitle });
        }

        public async Task UpdateIssueDescriptionAsync(long issueId, string newDescription)
        {
            await UpdateIssueAsync(issueId, new { body = newDescription });
        }

        public async Task CreateIssueAsync(string title, string description)
        {
            var issueData = new { title, body = description };
            var request = CreateRequest(HttpMethod.Post,
                $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues",
                issueData);

            await SendRequestAsync(request);
        }

        private async Task UpdateIssueStateAsync(long issueId, string state)
        {
            var issueData = new { state };
            var request = CreateRequest(HttpMethod.Patch,
                $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues/{issueId}",
                issueData);

            await SendRequestAsync(request);
        }

        private async Task UpdateIssueAsync(long issueId, object issueData)
        {
            var request = CreateRequest(HttpMethod.Patch,
                $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues/{issueId}",
                issueData);

            await SendRequestAsync(request);
        }

        protected override void ConfigureRequestHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", $"token {_authToken}");
            request.Headers.Add("User-Agent", "HttpClient");
        }
    }
}
