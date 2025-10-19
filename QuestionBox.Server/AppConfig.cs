using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.FileProviders;

namespace QuestionBox;
public class AppConfig {
    public JsonNode Config { get; }

    public AppConfig(IFileProvider fileProvider) {
        StreamReader reader = new(fileProvider.GetFileInfo("AppConfig.json").CreateReadStream());
        string json = reader.ReadToEnd();
        Config = JsonNode.Parse(json)!;
    }
}