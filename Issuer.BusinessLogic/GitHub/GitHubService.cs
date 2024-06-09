using System.Text;
using System.Text.Json;
using Issuer.BusinessLogic.GitHub.Models;
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
        ConfigureRequestHeaders(request);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseData = await response.Content.ReadAsStringAsync();
        var gitHubIssues = JsonSerializer.Deserialize<List<GitHubIssue>>(responseData);

        return gitHubIssues.Select(issue => new Issue(issue.Title,
            issue.Body,
            issue.State == "closed"
        )).ToList();
    }

    public async Task CloseIssueAsync(Int64 issueNumber)
    {
        var issueData = new
        {
            state = "closed"
        };

        var content = new StringContent(JsonSerializer.Serialize(issueData), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch, $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues/{issueNumber}")
        {
            Content = content
        };
        ConfigureRequestHeaders(request);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateIssueTitleAsync(long issueId, string newTitle)
    {
        var issueData = new
        {
            title = newTitle
        };

        var content = new StringContent(JsonSerializer.Serialize(issueData), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch,
            $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues/{issueId}")
        {
            Content = content
        };
        ConfigureRequestHeaders(request);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateIssueDescriptionAsync(long issueId, string newDescription)
    {
        var issueData = new
        {
            body = newDescription
        };

        var content = new StringContent(JsonSerializer.Serialize(issueData), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch,
            $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues/{issueId}")
        {
            Content = content
        };
        ConfigureRequestHeaders(request);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateIssueAsync(string title, string description)
    {
        var issueData = new
        {
            title = title,
            body = description
        };

        var content = new StringContent(JsonSerializer.Serialize(issueData), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post,
            $"https://api.github.com/repos/{_repositoryOwner}/{_repositoryName}/issues")
        {
            Content = content
        };
        ConfigureRequestHeaders(request);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private void ConfigureRequestHeaders(HttpRequestMessage request)
    {
        request.Headers.Add("Authorization", $"token {_authToken}");
        request.Headers.Add("User-Agent", "HttpClient");
    }
}