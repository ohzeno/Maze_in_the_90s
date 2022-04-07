using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FirebaseWebGL.Examples.Auth
{
    public class LoginHandler : MonoBehaviour
    {
        public static LoginHandler instance;
        public static string UserUid;
        public static int UserChar = 0;


        [Header("UI References")]
        [SerializeField]
        //1. 로그인
        private GameObject loginUI;
        [SerializeField]
        //2. 비밀번호 재설정
        private GameObject resetPwUI;
        [SerializeField]
        //3. 이메일 전송 확인
        private GameObject emailSentUI;
        [SerializeField]
        //4. 닉넴체크
        private GameObject checkNicknameUI;
        [Space(5f)]

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

        [Header("Reset Password References")]
        [SerializeField]
        private TMP_InputField resetPwEmail;
        [SerializeField]
        private TMP_Text resetEmailErrorText;
        [Space(5f)]

        [Header("Email Sent References")]
        [SerializeField]
        private TMP_Text usedEmail;
        public TMP_Text statusText;
        [Space(5f)]

        [Header("Nickname References")]
        [SerializeField]
        private TMP_InputField checkUsernameText;
        [SerializeField]
        private TMP_Text outputText;
        [Space(5f)]

        public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        //이메일 양식 유효성 검사
        public static bool ValidateEmail(string email)
        {
            if (email != null)
                return Regex.IsMatch(email, MatchEmailPattern);
            else
                return false;
        }

        private void Start()
        {
            CheckAutoLogin();
        }

        private void DisplayError(string errortext)
        {
            statusText.text = errortext;
        }

        private void DisPlayInfo(string Infotext)
        {
            statusText.text = Infotext;
        }

        public void SignUpNicknameCheck()
        {
            CheckNickUI();
        }

        public void ClearUI()
        {
            loginUI.SetActive(false);
            resetPwUI.SetActive(false);
            emailSentUI.SetActive(false);
            checkNicknameUI.SetActive(false);
            checkUsernameText.text = "";
        }

        public void CheckNickUI()
        {
            ClearUI();
            checkNicknameUI.SetActive(true);
        }

        public void LoginScreen()
        {
            ClearUI();
            loginUI.SetActive(true);
        }

        public void ResetPwScreen()
        {
            ClearUI();
            resetPwUI.SetActive(true);
        }

        public void EmailSentScreen(string email)
        {
            ClearUI();
            emailSentUI.SetActive(true);
            usedEmail.text = email;

        }

        public void ResetPw()
        {
            if (ValidateEmail(resetPwEmail.text) != false)
            {
                ResetPassword(resetPwEmail.text);
            }
            else
            {
                resetEmailErrorText.text = "유효한 이메일이 아니거나 일치하는 사용자가 없습니다!";
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


        public void SignWithEmailAndPassword() =>
            FirebaseAuth.SignInWithEmailAndPassword(loginEmail.text, loginPassword.text, gameObject.name, "DisPlayInfo", "DisplayError");

        public void LoginWithGoogle() =>
            FirebaseAuth.LoginWithGoogle(gameObject.name, "DisPlayInfo", "DisplayError");

        public void LoginWithGithub() =>
            FirebaseAuth.LoginWithGithub(gameObject.name, "DisPlayInfo", "DisplayError");

        public void CheckAutoLogin() =>
            FirebaseAuth.CheckAutoLogin();

        public void ResetPassword(string email) =>
            FirebaseAuth.ResetPassword(email);

        public void CheckNicknameForSocial() =>
           FirebaseDatabase.CheckNicknameForSocial(checkUsernameText.text);


        public void CheckComplete()
        {
            if (outputText.text == "사용 가능한 닉네임입니다")
            {

                FirebaseAuth.UpdateInfoWithGoogleOrGithub(checkUsernameText.text);
            }

        }


        public void RegisterScreen()
        {
            GameManager.instance.ChangeScene("SignUp");
        }

        public void LobbyScreen(string uid)
        {
            UserUid = uid;
            Debug.Log("from login to lobby");
            Debug.Log(uid);
            GameManager.instance.ChangeScene("Lobby");
        }

        public void GetCharacter(string _character)
        {
            Debug.Log("get character!");
            Debug.Log(_character);
            var intChar = Int32.Parse(_character);
            Debug.Log(intChar);
            UserChar = intChar;
            Debug.Log(UserChar);
        }

        public void RankingScreen()
        {
            GameManager.instance.ChangeScene("Ranking");
        }
    }

}
