using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;

namespace FirebaseWebGL.Examples.Auth
{
    public class SignUpHandler : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        //1. 회원가입
        private GameObject signUpUI;
        [SerializeField]
        //2. 닉넴체크
        private GameObject checkNicknameUI;
        [Space(5f)]

        [Header("Nickname References")]
        [SerializeField]
        private TMP_InputField checkUsernameText;
        [SerializeField]
        private TMP_Text outputText;
        [Space(5f)]

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


        private void CheckedName(int result)
        {

            if (result == 0)
            {
                registerNameErrorText.text = "사용할 수 없는 닉네임입니다";
            }
            else if (result == 1)
            {
                registerNameErrorText.text = "사용 가능한 닉네임입니다";
            }
            else if (result == 3)
            {
                registerNameErrorText.text = "닉네임을 입력해주세요";
            }
        }

        private void CheckedNameForSocial(int result)
        {

            if (result == 0)
            {
                outputText.text = "사용할 수 없는 닉네임입니다";
            }
            else if (result == 1)
            {
                outputText.text = "사용 가능한 닉네임입니다";
            }
            else if (result == 3)
            {
                outputText.text = "닉네임을 입력해주세요";
            }
        }
       
        public void CheckNickname() =>
           FirebaseDatabase.CheckNickname(registerUsername.text);

        public void CheckNicknameForSocial() =>
           FirebaseDatabase.CheckNicknameForSocial(checkUsernameText.text);

        public void CreateUserWithEmailAndPassword() =>
           //Firebase Authentication & Realtime Database에 유저 등록
           FirebaseAuth.CreateUserWithEmailAndPassword(registerUsername.text, registerEmail.text, registerPassword.text, gameObject.name, "DisPlayInfo");

        public void SignInWithGoogle() =>
           FirebaseAuth.SignInWithGoogle(gameObject.name, "DisPlayInfo", "DisplayError");

        public void SignInWithGithub() =>
            FirebaseAuth.SignInWithGithub(gameObject.name, "DisPlayInfo", "DisplayError");


        public void ClearUI()
        {
            signUpUI.SetActive(false);
            checkNicknameUI.SetActive(false);
            checkUsernameText.text = "";
            outputText.text = "";
        }

        public void CheckNickUI()
        {
            ClearUI();
            checkNicknameUI.SetActive(true);
        }

        public void LoginScreen()
        {
            GameManager.instance.ChangeScene("Login");
        }

        public void CheckComplete()
        {
            if (outputText.text == "사용 가능한 닉네임입니다"){
                FirebaseAuth.UpdateInfoWithGoogleOrGithub(checkUsernameText.text);
            }
            
        }


    }

}