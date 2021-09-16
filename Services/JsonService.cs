using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GithubConnection.Services
{
    public class JsonService
    {
        public string GetUserHtmlUrl(string json)
        {
            dynamic deserialized = JsonConvert.DeserializeObject(json);

            if (deserialized != null)
            {
                return deserialized.html_url;
            }

            return string.Empty;
        }

        public List<RepoGithubModel> GetUserRepositories(string json)
        {
            var output = new List<RepoGithubModel>();
            var array = JsonSerializer.Deserialize<RepoGithubModel[]>(json);
            if (array != null)
            {
                output = array.ToList();
            }
            return output;
        }
        
        public List<CommitGithubModel> GetCommitHistory(string json)
        {
            var output = new List<CommitGithubModel>();
            var array = JsonSerializer.Deserialize<CommitGithubModel[]>(json);
            if (array != null)
            {
                output = array.ToList();
            }
            return output;
        }
        
    }


}