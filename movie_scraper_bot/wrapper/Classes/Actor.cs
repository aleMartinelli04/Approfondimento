using Newtonsoft.Json.Linq;

namespace Wrapper.wrapper.Classes;

public class Actor
{
    public string NConst { get; private init; }
    public string Name { get; private init; }
    public string? BirthDate { get; private init; }
    public string? BirthPlace { get; private init; }
    public string ImageUrl { get; private init; }
    public string? Bio { get; private init; }

    public static Actor Deserialize(string deserializable)
    {
        var json = JObject.Parse(deserializable);
        
        return new Actor
        {
            NConst = json["id"]?.ToString().Split("/")[2]!,
            Name = json["name"]?.ToString()!,
            BirthDate = json["birthDate"]?.ToString(),
            BirthPlace = json["birthPlace"]?.ToString(),
            ImageUrl = json["image"]?["url"]?.ToString() ?? "https://www.golfeturismo.it/wp-content/uploads/2020/12/avatar-generico.jpg",
            Bio = json["miniBios"]?[0]?["text"]?.ToString()
        };
    }
}