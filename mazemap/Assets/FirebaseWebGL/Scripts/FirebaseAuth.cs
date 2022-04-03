using System.Runtime.InteropServices;

namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseAuth
    {
        [DllImport("__Internal")]
        public static extern void CheckAuthState();

        [DllImport("__Internal")]
        public static extern void CheckAutoLogin();

        [DllImport("__Internal")]
        public static extern void CreateUserWithEmailAndPassword(string username, string email, string password, string objectName, string callback);

        [DllImport("__Internal")]
        public static extern void SignInWithEmailAndPassword(string email, string password, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void SignInWithGoogle(string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void SignInWithGithub(string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void UpdateInfoWithGoogleOrGithub(string username);

        [DllImport("__Internal")]
        public static extern void UpdateNickname(string username);

        [DllImport("__Internal")]
        public static extern void LoginWithGoogle(string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void LoginWithGithub(string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void SignOut();

        [DllImport("__Internal")]
        public static extern void UpdateProfilePicture(string newProfile);

        [DllImport("__Internal")]
        public static extern void UpdatePw(string newPw);

        [DllImport("__Internal")]
        public static extern void ResetPassword(string email);

        [DllImport("__Internal")]
        public static extern void DeleteUser();

        [DllImport("__Internal")]
        public static extern void IsLoggedIn();

    }
}