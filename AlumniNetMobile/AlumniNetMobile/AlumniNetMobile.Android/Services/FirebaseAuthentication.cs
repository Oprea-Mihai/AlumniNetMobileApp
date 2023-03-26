using AlumniNetMobile.Common;
using AlumniNetMobile.Droid.Services;
using Firebase.Auth;
using System.Threading.Tasks;
[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAuthentication))]
namespace AlumniNetMobile.Droid.Services
{
    public class FirebaseAuthentication : IAuthenticationService
    {
        public async Task<bool> CreateUser(string username, string email, string password)
        {

            var authResult = await FirebaseAuth.Instance
               .CreateUserWithEmailAndPasswordAsync(email, password);

            var userProfileChangeRequestBuilder = new UserProfileChangeRequest.Builder();
            userProfileChangeRequestBuilder.SetDisplayName(username);

            var userProfileChangeRequest = userProfileChangeRequestBuilder.Build();
            await authResult.User.UpdateProfileAsync(userProfileChangeRequest);

            return await Task.FromResult(true);
        }

        public async Task<string> SignIn(string email, string password)
        {
            var authResult = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            var token = await authResult.User.GetIdTokenAsync(false);
            return token.Token;
        }

        public bool IsSignedIn()
            => FirebaseAuth.Instance.CurrentUser != null;


        public void SignOut()
            => FirebaseAuth.Instance.SignOut();

        public async Task ResetPassword(string email)
            => await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);
    }
}