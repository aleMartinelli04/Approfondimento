using Telegraph.Net;
using Telegraph.Net.Models;
using Wrapper.wrapper;

namespace Wrapper;

public static class Accounts
{
    public static Imdb Imdb { get; } = new();
    
    private static readonly TelegraphClient TelegraphClient = new();
    private static readonly Account TelegraphAccount = TelegraphClient.CreateAccountAsync("movie_scraper_bot").Result;
    public static ITokenClient Telegraph { get; } = TelegraphClient.GetTokenClient(TelegraphAccount.AccessToken);
}