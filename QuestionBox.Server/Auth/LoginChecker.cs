namespace QuestionBox.Auth;

using System.Security.Cryptography;
using System.Text;

public interface ILoginChecker {
    public Role check(string requestMame, string requestPasswd);
}

public class SimpleLoginChecker(IConfiguration config) : ILoginChecker {
    public Role check(string requestMame, string requestPasswd) {
        string? name = config["Admin:username"];
        string? passwdHashBase64 = config["Admin:password"];
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(passwdHashBase64);

        if (name != requestMame) {
            return Role.Unauthorized;
        }

        using SHA3_256 hash = SHA3_256.Create();
        var passwdBytes = Encoding.UTF8.GetBytes(requestPasswd);
        var hashedPasswd = hash.ComputeHash(passwdBytes);
        var correctHash = Convert.FromBase64String(passwdHashBase64);

        return hashedPasswd.SequenceEqual(correctHash) ?
            Role.Admin : Role.Unauthorized;
    }
}