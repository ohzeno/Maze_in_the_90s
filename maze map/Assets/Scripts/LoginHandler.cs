using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;

namespace FirebaseWebGL.Examples.Auth
{
    public class LoginHandler : MonoBehaviour
    {
        [Header("Login References")]
        [SerializeField]
        private TMP_InputField loginEmail;
        [SerializeField]
        private TMP_InputField loginPassword;
        [SerializeField]
        private TMP_Text emailErrorText;
        [SerializeField]
        private TMP_Text passwordErrorText;
        [Space(5f)]

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

        public void SignWithEmailAndPassword() =>
            FirebaseAuth.SignInWithEmailAndPassword(loginEmail.text, loginPassword.text, gameObject.name, "DisPlayInfo", "DisplayError");

        public void SignInWithGoogle() =>
            FirebaseAuth.SignInWithGoogle(gameObject.name, "DisPlayInfo", "DisplayError");

        public void SignInWithGithub() =>
            FirebaseAuth.SignInWithGithub(gameObject.name, "DisPlayInfo", "DisplayError");


        public void RegisterScreen()
        {
            GameManager.instance.ChangeScene("SignUp");
        }

        public void LobbyScreen()
        {
            GameManager.instance.ChangeScene("Lobby");
        }
    }

}