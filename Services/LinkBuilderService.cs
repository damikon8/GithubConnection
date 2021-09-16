namespace GithubConnection.Services
{
    public class LinkBuilderService
    {
        private readonly string _repoName;
        private readonly string _user;

        public LinkBuilderService(string user, string repoNme)
        {
            this._user = user;
            this._repoName = repoNme;
        }

        public string GetUserLink()
        {
            return $"https://api.github.com/users/{this._user}";
        }

        public string GetUserReposLink()
        {
            return $"https://api.github.com/users/{this._user}/repos";
        }
        
        public string GetRepoLink()
        {
            return $"https://api.github.com/repos/{this._user}/{this._repoName}";
        }
        
        public string GetRepoCommitsLink()
        {
            return $"https://api.github.com/repos/{this._user}/{this._repoName}/commits";
        }
    }
}