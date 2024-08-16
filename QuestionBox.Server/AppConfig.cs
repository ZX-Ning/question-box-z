using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.FileProviders;

namespace QuestionBoxl;
public class AppConfig {
    string json;
    public record MySqlConfigObj (
        bool UsingMysql,
        string server,
        int port,
        string user,
        string password,
        string db
    );
    public record ConfigObj(
        string ServerDevUrl,
        string ServerProductionUrl,
        string SqlitePath,
        MySqlConfigObj Mysql,
        int QuestionLengthLimit
    );

    public ConfigObj Config { get; }

    public AppConfig(IFileProvider fileProvider) {
        StreamReader reader = new(fileProvider.GetFileInfo("AppConfig.json").CreateReadStream());
        json = reader.ReadToEnd();
        Config = JsonSerializer.Deserialize<ConfigObj>(json)!;
    }
}