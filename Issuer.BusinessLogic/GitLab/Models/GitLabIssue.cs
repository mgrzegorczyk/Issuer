using System;
using System.Text.Json.Serialization;

namespace Issuer.BusinessLogic.GitLab.Models
{
    public class GitLabIssue
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
