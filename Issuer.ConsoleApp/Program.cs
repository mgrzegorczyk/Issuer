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

Console.ReadLine();