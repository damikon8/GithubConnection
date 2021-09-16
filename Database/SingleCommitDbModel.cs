using System;

namespace GithubConnection.Database
{
    public class SingleCommitDbModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string RepoName { get; set; }
        public string Sha { get; set; }
        public string Message { get; set; }
        public string Committer { get; set; }
    }
}