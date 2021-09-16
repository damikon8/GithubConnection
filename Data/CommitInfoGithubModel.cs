using System;
using System.Text.Json.Serialization;

namespace GithubConnection
{
    public class CommitInfoGithubModel
    {
        [JsonPropertyName("author")]
        public PersonGithubModel Author { get; set; }
        [JsonPropertyName("committer")]
        public PersonGithubModel Commiter { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
    


}