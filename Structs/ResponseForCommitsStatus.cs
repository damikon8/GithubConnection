using System.Collections.Generic;

namespace GithubConnection.Structs
{
    public struct ResponseForCommitsStatus
    {
        public bool WasSuccesfull;
        public List<CommitGithubModel> Output;
    }
}