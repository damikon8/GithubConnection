using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubConnection.Database;

namespace GithubConnection.Services
{
    public class FlowService
    {
        private readonly LinkBuilderService _linkBuilder;
        private readonly RequestService _requestService;
        private readonly JsonService _jsonService;
        
        private List<RepoGithubModel> _userRepos;
        private readonly string _repositoryName;
        private List<CommitGithubModel> _commitHistory;
        private readonly string _userName;

        public FlowService(string user, string repoName)
        {
            this._repositoryName = repoName;
            this._userName = user;
            this._linkBuilder = new LinkBuilderService(user, repoName);
            this._requestService = new RequestService();
            this._jsonService = new JsonService();
        }
        
        public async Task<bool> CheckUser()
        {
            var response = await this._requestService.GetResponse(this._linkBuilder.GetUserLink());
            if (response.WasSuccesfull)
            {
                var userHtml = this._jsonService.GetUserHtmlUrl(response.Output);
                if (string.IsNullOrEmpty(userHtml))
                {
                    Console.WriteLine("Nie odnaleziono podanego użytkownika");
                    return false;
                }
                Console.WriteLine(userHtml);
                return true;
            }
            else
            {
                Console.WriteLine("Nie odnaleziono podanego użytkownika");
                return false;
            }
        }
        
        public async Task<bool> CheckRepos()
        {
            var response = await this._requestService.GetResponse(this._linkBuilder.GetUserReposLink());
            if (response.WasSuccesfull)
            {
                var repos = this._jsonService.GetUserRepositories(response.Output);
                if (repos.Count == 0)
                {
                    Console.WriteLine("Podany użytkownik nie posiada repozytoriów");
                    return false;
                }

                this._userRepos = repos;
                Console.WriteLine("REPOZYTORIA: \n");
                foreach (var repo in repos)
                {
                    Console.WriteLine($"\t - {repo.Name} [{repo.Id}]");
                }
                return true;
            }
            else
            {
                Console.WriteLine("Podany użytkownik nie posiada repozytoriów");
                return false;
            }
        }

        public bool CheckIfRepoExist()
        {
            if (this._userRepos.Select(n => n.Name).Contains(this._repositoryName))
            {
                Console.WriteLine($"\n Repozytorium {this._repositoryName} znajduje się na liscie");
                return true;
            }
            
            Console.WriteLine($"\n Repozytorium {this._repositoryName} NIE znajduje się na liscie repozytoriów tego użytkownika");
            return false;
        }
        

        
        public async Task<bool> GetRepoCommitHistory()
        {
            var response = await this._requestService.GetResponseOfCommits(this._linkBuilder.GetRepoCommitsLink());
            if (response.WasSuccesfull)
            {

                this._commitHistory = response.Output;
                Console.WriteLine("Commity: \n");
                foreach (var commit in this._commitHistory)
                {
                    var trimmedMsg = commit.Commit.Message.Replace("\n", " ").Trim();
                    Console.WriteLine($"\t [{this._repositoryName}]/[{commit.Sha}]: [{trimmedMsg}] [{commit.Commit.Commiter.Name}]");
                }
                return true;
            }
            else
            {
                Console.WriteLine("Nie odnaleziono histori commitów");
                return false;
            }
        }

        public async Task SaveCommitHistoryToDB()
        {
            using (var context = new DatabaseContext())
            {
                var list = this._commitHistory.Select(n => new SingleCommitDbModel()
                {
                    UserName = this._userName,
                    RepoName = this._repositoryName,
                    Sha = n.Sha,
                    Message = n.Commit.Message,
                    Committer = n.Commit.Commiter.Name
                });


                foreach (var element in list)
                {
                    if(context.CommitsTable.Select(n=>n.Sha).Contains(element.Sha)) continue;
                    context.CommitsTable.Update(element);
                }

                context.SaveChanges();
                Console.WriteLine($"Elementów w bazie danych: [{context.CommitsTable.Count()}]");
            }
        }
    }
}