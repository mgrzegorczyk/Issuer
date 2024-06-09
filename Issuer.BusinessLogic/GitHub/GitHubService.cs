using System.Text.Json;
using Issuer.BusinessLogic.Github.Models;
using Issuer.BusinessLogic.Interfaces;
using Issuer.BusinessLogic.Models;

namespace Issuer.BusinessLogic.Github;

public class GitHubService : IIssuesHostingService
{
    private readonly HttpClient _httpClient;
    private readonly string _repositoryOwner;
    private readonly string _repositoryName;
    private readonly string _authToken;

    public GitHubService(string repositoryOwner, string repositoryName, string authToken)
    {
        _httpClient = new HttpClient();
        _repositoryOwner = repositoryOwner;
        _repositoryName = repositoryName;
        _authToken = authToken;
    }

    public async Task<List<Issue>> GetIssuesAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues?state=all");
        request.Headers.Add("Authorization", $"token {_authToken}");
        request.Headers.Add("User-Agent", "HttpClient");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseData = await response.Content.ReadAsStringAsync();
        var gitHubIssues = JsonSerializer.Deserialize<List<GitHubIssue>>(responseData);

        return gitHubIssues.Select(issue => new Issue(issue.Title,
            issue.Body,
            issue.State == "closed"
        )).ToList();
    }
}