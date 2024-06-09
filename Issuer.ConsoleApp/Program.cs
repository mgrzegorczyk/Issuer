using Issuer.BusinessLogic.Github;

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

Console.ReadLine();