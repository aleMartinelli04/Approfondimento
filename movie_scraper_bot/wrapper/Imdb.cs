using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Wrapper.Bot;
using Wrapper.wrapper.Classes;
using Wrapper.wrapper.Exceptions;

namespace Wrapper.wrapper;

public class Imdb
{
    private const string Baseurl = "https://imdb8.p.rapidapi.com/";

    private readonly Random _random = new ();

    private string RandomHeader()
    {
        return Config.RapidapiKeys[_random.Next(0, Config.RapidapiKeys.Count)];
    }

    private string? Request(string url, Dictionary<string, string>? parameters = null)
    {
        var client = new RestClient(Baseurl + url);
        var request = new RestRequest(Baseurl + url);

        request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
        request.AddHeader("x-rapidapi-key", RandomHeader());

        if (parameters != null)
        {
            foreach (var (key, value) in parameters)
            {
                request.AddParameter(key, value);
            }
        }

        return client.ExecuteAsync(request).Result.Content;
    }

    public IEnumerable<ShowBasic> GetShows(string q)
    {
        const string url = "auto-complete";

        var parameters = new Dictionary<string, string>
        {
            {"q", q}
        };

        var response = Request(url, parameters);

        if (response is null)
        {
            throw new RequestFailedException(url);
        }

        var showList = JObject.Parse(response)["d"];
        if (showList is null)
        {
            throw new RequestFailedException(url);
        }

        return showList.Where(show => show["id"]?.ToString()[..2] == "tt")
            .Select(show => ShowBasic.Deserialize(JObject.Parse(show.ToString())))
            .ToList();
    }

    public IEnumerable<ActorBasic> GetActors(string q)
    {
        const string url = "auto-complete";

        var parameters = new Dictionary<string, string>
        {
            {"q", q}
        };

        var response = Request(url, parameters);

        if (response is null)
        {
            throw new RequestFailedException(url);
        }

        var actorList = JObject.Parse(response)["d"];
        if (actorList is null)
        {
            throw new RequestFailedException(url);
        }

        return actorList.Where(actor => actor["id"]?.ToString()[..2] == "nm")
            .Select(show => ActorBasic.Deserialize(JObject.Parse(show.ToString())))
            .ToList();
    }
    
    public Title TitlesGetDetails(string tconst)
    {
        const string url = "title/get-details";

        var parameters = new Dictionary<string, string>
        {
            {"tconst", tconst}
        };

        var response = Request(url, parameters);
        if (response is null)
        {
            throw new RequestFailedException(url);
        }

        return Title.Deserialize(response);
    }
    
    public IEnumerable<string> GetCrazyCommentsFor(string tconst)
    {
        const string url = "title/get-crazycredits";

        var parameters = new Dictionary<string, string>
        {
            {"tconst", tconst}
        };

        var response = Request(url, parameters);
        if (response is null)
        {
            throw new RequestFailedException(url);
        }

        return JObject.Parse(response)["crazycredits"]!
            .Select(jToken => jToken["text"]?.ToString()!)
            .ToList();
    }
    
    public IEnumerable<string> GetTopCast(string tconst, int limit = 5)
    {
        const string url = "title/get-top-cast";
        
        var parameters = new Dictionary<string, string>
        {
            {"tconst", tconst}
        };
        
        var response = Request(url, parameters);
        if (response is null)
        {
            throw new RequestFailedException(url);
        }

        var actorCodes = JsonConvert.DeserializeObject<List<string>>(response);
        if (actorCodes is null)
        {
            throw new RequestFailedException(url);
        }

        return actorCodes.Select(code => code.Split("/")[2]).Take(limit).ToList();
    }

    public Actor GetActorBio(string nconst)
    {
        const string url = "actors/get-bio";

        var parameters = new Dictionary<string, string>
        {
            {"nconst", nconst}
        };
        
        var response = Request(url, parameters);
        if (response is null)
        {
            throw new RequestFailedException(url);
        }

        return Actor.Deserialize(response);
    }
    
    public IEnumerable<TitleBasic> GetKnownFor(string nconst)
    {
        const string url = "actors/get-known-for";

        var parameters = new Dictionary<string, string>
        {
            {"nconst", nconst}
        };
        
        var response = Request(url, parameters);
        if (response is null)
        {
            throw new RequestFailedException(url);
        }

        var shows = JsonConvert.DeserializeObject<List<JObject>>(response);
        if (shows is null)
        {
            throw new RequestFailedException(url);
        }
        
        return shows.Select(show => TitleBasic.Deserialize(show.ToString())).ToList();
    }
    
    public object? GetById(string idconst)
    {
        return idconst[..2] switch
        {
            "nm" => GetActorBio(idconst),
            "tt" => TitlesGetDetails(idconst),
            _ => null
        };
    }
}