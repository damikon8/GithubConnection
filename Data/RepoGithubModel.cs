using System.Text.Json.Serialization;

namespace GithubConnection
{
    public class RepoGithubModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("node_id")]
        public string Node_id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("full_name")]
        public string Full_name { get; set; }
        
        [JsonPropertyName("commits_url")]
        public string CommitsUrl { get; set; }
    }
}