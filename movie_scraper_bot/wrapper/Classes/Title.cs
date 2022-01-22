using Newtonsoft.Json.Linq;

namespace Wrapper.wrapper.Classes;

public class Title
{
    public string Name { get; private init; }
    public int Episodes { get; private init; }
    public string ImageUrl { get; private init; }
    public int Year { get; private init; }

    public static Title Deserialize(string deserializable)
    {
        var jObject = JObject.Parse(deserializable);

        return new Title
        {
            Name = jObject["title"]!.ToString(),
            Episodes = Convert.ToInt32(jObject["numberOfEpisodes"]),
            ImageUrl = jObject["image"]!["url"]!.ToString(),
            Year = Convert.ToInt32(jObject["year"])
        };
    }
}