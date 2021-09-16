using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GithubConnection.Services;

namespace GithubConnection
{
    class Program
    {
        
        static async Task Main(string[] args)
        {
            var unwraped = CheckParams(args);
            Console.WriteLine($"Czy poprawnie podano parametry: {unwraped.AreCorect}");
            Console.WriteLine($"Nazwa uzytkownika: [{unwraped.Username}]");
            Console.WriteLine($"Nazwa repozytorium: [{unwraped.RepoName}]");

            var flow = new FlowService(unwraped.Username, unwraped.RepoName);
            
            var proceed = await flow.CheckUser();
            if(proceed == false) return;
            
            proceed = await flow.CheckRepos();
            if(proceed == false) return;
            
            proceed = flow.CheckIfRepoExist();
            if(proceed == false) return;
            
            proceed = await flow.GetRepoCommitHistory();
            if(proceed == false) return;

            await flow.SaveCommitHistoryToDB();
        }



        private static ArgsStructure CheckParams(string[] args)
        {
            var output = new ArgsStructure() {AreCorect = false};
            if (args.Length != 4) return output;
            if (args.Contains("-u") == false) return output;
            if (args.Contains("-r") == false) return output;
            if (args.First().ToLower() == "-u")
            {
                if (args.Skip(2).First() != "-r") return output;
                output.Username = args.Skip(1).First();
                output.RepoName = args.Skip(3).First();
            }
            else //that will be -r
            {
                if (args.Skip(2).First() != "-u") return output;
                output.RepoName = args.Skip(1).First();
                output.Username = args.Skip(3).First();
            }

            output.AreCorect = true;
            return output;
        }
    }
    
    
}
