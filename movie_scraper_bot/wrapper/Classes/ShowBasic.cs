using Newtonsoft.Json.Linq;

namespace Wrapper.wrapper.Classes;

public class ShowBasic
{
    public string TConst { get; private init; }
    public string Title { get; private init; }

    public static ShowBasic Deserialize(JObject jShowBasic)
    {
        return new ShowBasic
        {
            TConst = jShowBasic["id"]?.ToString()!,
            Title = jShowBasic["l"]?.ToString()!
        };
    }
}