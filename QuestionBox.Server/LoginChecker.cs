namespace QuestionBox; 

using System.Security.Cryptography;
using System.Text;

public class LoginChecker(IConfiguration config) {
    public bool check(string requestMame, string requestPasswd) {
        string? name = config["Admin:username"];
        string? passwdHashBase64 = config["Admin:password"];
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(passwdHashBase64);

        if (name != requestMame) {
            return false;
        }

        using SHA3_256 hash = SHA3_256.Create();
        var passwdBytes = Encoding.UTF8.GetBytes(requestPasswd);
        var hashedPasswd = hash.ComputeHash(passwdBytes);
        var correctHash = Convert.FromBase64String(passwdHashBase64);
        
        return hashedPasswd.SequenceEqual(correctHash);
    }
}