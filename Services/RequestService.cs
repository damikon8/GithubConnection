using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GithubConnection.Structs;

namespace GithubConnection.Services
{
    public class RequestService
    {
        public async Task<ResponseStatus> GetResponse(string url)
        {
            var result = new ResponseStatus();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add( new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        var output = await response.Content.ReadAsStringAsync();
                        result.WasSuccesfull = true;
                        result.Output = output;
                    }
                }
            }
            catch
            {
                result.WasSuccesfull = false;
                result.Output = string.Empty;
            }

            return result;
        }
        
        
        public async Task<ResponseForCommitsStatus> GetResponseOfCommits(string url)
        {
            var result = new ResponseForCommitsStatus();
            result.WasSuccesfull = true;
            result.Output = new List<CommitGithubModel>();
            var parser = new JsonService();
            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add( new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                    var shouldContinue = true;
                    var page = 0;
                    while (shouldContinue)
                    {
                        page += 1;
                        UriBuilder builder = new UriBuilder(url);
                        builder.Query = $"per_page=100&page={page}";
                    
                        using (HttpResponseMessage response = client.GetAsync(builder.Uri).Result)
                        {
                            response.EnsureSuccessStatusCode();
                            var output = await response.Content.ReadAsStringAsync();
                            var parsed = parser.GetCommitHistory(output);
                            if (parsed.Count == 0) break;
                            foreach (var single in parsed)
                            {
                                if (result.Output.Select(n => n.Sha).Contains(single.Sha))
                                {
                                    shouldContinue = false;
                                }
                                else
                                {
                                    result.Output.Add(single);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                result.WasSuccesfull = false;
            }

            return result;
        }
    }
}