using System.Text.Json;
using Issuer.BusinessLogic.GitLab.Models;
using Issuer.BusinessLogic.Interfaces;
using Issuer.BusinessLogic.Models;

namespace Issuer.BusinessLogic.GitLab
{
    public class GitLabService : IIssuesHostingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _repositoryId;
        private readonly string _authToken;

        public GitLabService(string repositoryId, string authToken)
        {
            _httpClient = new HttpClient();
            _repositoryId = repositoryId;
            _authToken = authToken;
        }

        public async Task<List<Issue>> GetIssuesAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://gitlab.com/api/v4/projects/{_repositoryId}/issues?scope=all");
            request.Headers.Add("Authorization", $"token {_authToken}");
            request.Headers.Add("User-Agent", "HttpClient");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var gitLabIssues = JsonSerializer.Deserialize<List<GitLabIssue>>(responseData);

            return gitLabIssues.Select(issue => new Issue(issue.Title,
                issue.Description,
                issue.State == "closed"
            )).ToList();
        }
    }
}