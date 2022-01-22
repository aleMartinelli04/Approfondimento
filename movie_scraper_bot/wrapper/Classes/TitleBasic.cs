using Newtonsoft.Json.Linq;

namespace Wrapper.wrapper.Classes;

public class TitleBasic
{
    public string TConst { get; private init; }
    public string Title { get; private init; }
    public string ImageUrl { get; private init; }
    public string? ReleaseYear { get; private init; }
    public string? Rating { get; private init; }
    
    public string? Role { get; private init; }

    public static TitleBasic Deserialize(string deserializable)
    {
        var jTitleBasic = JObject.Parse(deserializable);
        
        return new TitleBasic
        {
            TConst = jTitleBasic["title"]!["id"]!.ToString().Split("/")[2],
            Title = jTitleBasic["title"]!["title"]!.ToString(),
            ImageUrl = jTitleBasic["title"]?["image"]?["url"]?.ToString() ?? "https://d1nhio0ox7pgb.cloudfront.net/_img/o_collection_png/green_dark_grey/512x512/plain/tv.png",
            ReleaseYear = jTitleBasic["title"]?["year"]?.ToString(),
            Rating = jTitleBasic?["imdbRating"]?.ToString(),
            Role = jTitleBasic?["summary"]?["characters"]?[0]?.ToString()
        };
    }
}