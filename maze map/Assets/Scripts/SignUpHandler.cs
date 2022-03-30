using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;

namespace FirebaseWebGL.Examples.Auth
{
    public class SignUpHandler : MonoBehaviour
    {

        [Header("Register References")]
        [SerializeField]
        private TMP_InputField registerUsername;
        [SerializeField]
        private TMP_InputField registerEmail;
        [SerializeField]
        private TMP_InputField registerPassword;
        [SerializeField]
        private TMP_InputField registerConfirmPassword;
        [SerializeField]
        private TMP_Text registerNameErrorText;
        [SerializeField]
        private TMP_Text registerEmailErrorText;
        [SerializeField]
        private TMP_Text registerPasswordErrorText;

        public TMP_Text statusText;

        private void Start()
        {
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                DisplayError("Webgl 플랫폼이 아니면 Javascript 기능은 인식되지 않습니다.");
                return;
            }
        }

        private void DisplayError(string errortext)
        {
            statusText.text = errortext;
        }

        private void DisPlayInfo(string Infotext)
        {
            statusText.text = Infotext;
        }


        public void CreateUserWithEmailAndPassword() =>
           //Firebase Authentication & Realtime Database에 유저 등록
           FirebaseAuth.CreateUserWithEmailAndPassword(registerUsername.text, registerEmail.text, registerPassword.text, gameObject.name, "DisPlayInfo");

        public void SignInWithGoogle() =>
           FirebaseAuth.SignInWithGoogle(gameObject.name, "DisPlayInfo", "DisplayError");

        public void SignInWithGithub() =>
            FirebaseAuth.SignInWithGithub(gameObject.name, "DisPlayInfo", "DisplayError");



        public void LoginScreen()
        {
            GameManager1.instance.ChangeScene("Login");
        }

        
    }

}