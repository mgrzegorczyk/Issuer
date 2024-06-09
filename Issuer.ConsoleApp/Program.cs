using Issuer.BusinessLogic;
using Issuer.BusinessLogic.Github;
using Issuer.BusinessLogic.GitLab;

Console.WriteLine("Hello, Issuer!");

var github = new GitHubService(
    "mgrzegorczyk",
    "Issuer",
    "token"
);

var issues = await github.GetIssuesAsync();

Console.WriteLine("Here is a list of GitHub issues:");
foreach (var issue in issues)
{
    Console.WriteLine($"{issue.Title}");
}

var gitlab = new GitLabService("58659168",
    "token");

var gitlabIssues = await gitlab.GetIssuesAsync();

Console.WriteLine("Here is a list of GitLab issues:");
foreach (var issue in gitlabIssues)
{
    Console.WriteLine($"{issue.Title}");
}

var gitHubIssuesManager = new IssuesManager(github);
var gitLabIssuesManager = new IssuesManager(gitlab);

await gitHubIssuesManager.ExportIssuesAsync("gitHubIssues.txt");
Console.WriteLine("GitHub issues exported!");
await gitLabIssuesManager.ExportIssuesAsync("gitLabIssues.txt");
Console.WriteLine("GitLab issues exported!");
    
Console.ReadLine();