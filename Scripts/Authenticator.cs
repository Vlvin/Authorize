using System.Security.Cryptography;
using System.Text;
using Authorize.DBModels;
using Authorize.Models;


namespace Authorize.Scripts {
  public class Authenticator {
    public string SignIn(AuthModel authData) {
      Console.WriteLine("Trying to Sign in with:");
      Console.WriteLine($"email: {authData.Email}");
      Console.WriteLine($"password: {authData.Password}");
      using (var context = new AuthorizeContext()) {
        var data = context.Users.Where(b => b.Email == authData.Email).ToList();
        if (data.Count == 0)
          return "NoToken";
        var user = data[0];

        string _salt = user.Salt ?? "";


        static string GenerateHash(string salt, string password)
        {
            string mix = new StringBuilder().Append(salt).Append(password).ToString();
            var hashBytes = SHA3_256.HashData(Encoding.ASCII.GetBytes(mix));
            var builder = new StringBuilder();
            foreach (var _byte in hashBytes)
              builder.Append(_byte.ToString("x2"));
            return builder.ToString();
        } 

        string _hash = GenerateHash(_salt, authData.Password);

        if (user.Hash == _hash)
          return "token";
        else
          return "noToken";
      }
      // find user under email
      // get 'salt'
      // mix 'salt' with password by known algorithm
      // calculate mix's hash by known algorithm
      // compare to user's hash
      // return token if good and errCode if not
    }

    public string SignUp(AuthModel authData) {
      Console.WriteLine("Trying to Sign up with:");
      Console.WriteLine($"email: {authData.Email}");
      Console.WriteLine($"password: {authData.Password}");
      using (var context = new AuthorizeContext()) {
        var data = context.Users.Where(b => b.Email == authData.Email).ToList();
        if (data.Count != 0)
          return "NoToken";

        static string GenerateSalt()
        {
            var _saltBytes = RandomNumberGenerator.GetBytes(32);
            var builder = new StringBuilder();
            foreach (var _byte in _saltBytes)
                builder.Append(_byte.ToString("x2"));
            return builder.ToString();
        }
        string _salt = GenerateSalt();


        static string GenerateHash(string salt, string password)
        {
            string mix = new StringBuilder().Append(salt).Append(password).ToString();
            var hashBytes = SHA3_256.HashData(Encoding.ASCII.GetBytes(mix));
            var builder = new StringBuilder();
            foreach (var _byte in hashBytes)
              builder.Append(_byte.ToString("x2"));
            return builder.ToString();
        } 

        string _hash = GenerateHash(_salt, authData.Password);
        
        var user = new User() {
          Email = authData.Email,
          Salt = _salt,
          Hash = _hash
        };
        context.Users.Add(user);
        context.SaveChanges();
      }
      
      // if user exists, return errCode
      // create user with email
      // generate random 'salt'
      // mix 'salt' with password by known algorithm
      // calculate mix's hash by known algorithm
      // add 'salt' and hash to user
      // return token
        return "token"; 
    }
  }
}

