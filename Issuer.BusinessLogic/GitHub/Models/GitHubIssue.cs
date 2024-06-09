using System.Text.Json.Serialization;

namespace Issuer.BusinessLogic.Github.Models;

public class GitHubIssue
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }
}