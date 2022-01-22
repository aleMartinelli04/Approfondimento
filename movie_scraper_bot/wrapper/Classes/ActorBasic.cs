using Newtonsoft.Json.Linq;

namespace Wrapper.wrapper.Classes;

public class ActorBasic
{
    public string Name { get; private init; }
    public string NConst { get; private init; }

    public static ActorBasic Deserialize(JObject jActorBasic)
    {
        return new ActorBasic()
        {
            Name = jActorBasic["l"]?.ToString()!,
            NConst = jActorBasic["id"]?.ToString()!
        };
    }
}