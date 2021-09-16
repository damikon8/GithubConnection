using System.Text.Json.Serialization;

namespace GithubConnection
{
    public class CommitGithubModel
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; }
        [JsonPropertyName("node_id")]
        public string NodeId { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("commit")]
        public CommitInfoGithubModel Commit { get; set; }
    }
}