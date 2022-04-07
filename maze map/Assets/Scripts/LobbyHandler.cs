using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Photon.Pun;

namespace FirebaseWebGL.Examples.Auth
{
    public class LobbyHandler : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        //1. 마이페이지
        private GameObject profileUI;
        [SerializeField]
        //2. 닉네임 변경페이지
        private GameObject changeNicknameUI;
        [SerializeField]
        //3. 비밀번호 변경
        private GameObject changePasswordUI;
        [SerializeField]
        //4. 회원 탈퇴
        private GameObject deleteUserConfirmUI;
        [SerializeField]
        //5. 확인페이지
        private GameObject actionSuccessPanelUI;
        [Space(5f)]

        [Header("Basic Info References")]
        [SerializeField]
        private TMP_Text lobbyUsernameText;
        [SerializeField]
        private Text myPageUsernameText;
        [Space(5f)]

        [Header("Change Nickname References")]
        [SerializeField]
        private Image lobbyProfilePicture;
        [SerializeField]
        private Image myPageProfilePicture;
        [SerializeField]
        private TMP_Text currNickname;
        [SerializeField]
        private TMP_InputField newNickname;
        [SerializeField]
        private TMP_Text outputText;

        [Header("Change Password References")]
        [SerializeField]
        private TMP_InputField changePasswordInputField;
        [SerializeField]
        private TMP_InputField changePasswordConfirmInputField;
        [SerializeField]
        private TMP_Text pwErrorText;
        [Space(5f)]

        [Header("Action Success Panel References")]
        [SerializeField]
        private TMP_Text actionSuccessText;

        //닉네임, 프사
        public static LobbyHandler instance;
        public static string userName = null;
        

        public void Start()
        {
            //최초 진입 시 프로필 로드
            CheckAuthState();
            Debug.Log("lobby scene");
            Debug.Log(FirebaseWebGL.Examples.Auth.LoginHandler.UserUid);
        }

        public void GetUsername(string _username)
        {
            userName = _username;

            //둘 다 모이면 로드프로필 실행
            if (userName != null)
            {
                LoadUsername();
            }
        }

        

        private void LoadUsername()
        {
            lobbyUsernameText.text = userName.ToString();
            myPageUsernameText.text = userName.ToString();
        }



        public void ClearUI()
        {
            profileUI.SetActive(false);
            changeNicknameUI.SetActive(false);
            changePasswordUI.SetActive(false);
            actionSuccessPanelUI.SetActive(false);
            deleteUserConfirmUI.SetActive(false);
            actionSuccessText.text = "";
            currNickname.text = "";
            newNickname.text = "";
            outputText.text = "";
            changePasswordInputField.text = "";
            changePasswordConfirmInputField.text = "";
            pwErrorText.text = "";
        }

        //1. 마이페이지
        public void ProfileUI()
        {
            ClearUI();
            profileUI.SetActive(true);
            //프로필 바꿨을 때 다시 호출
            CheckAuthState();
        }

        //2. 닉넴 변경
        public void ChangeNickUI()
        {
            ClearUI();
            changeNicknameUI.SetActive(true);
            currNickname.text = userName;
        }

        public void CheckNicknameForChange() =>
           FirebaseDatabase.CheckNicknameForChange(newNickname.text);

        private void CheckedNameForChange(int result)
        {

            if (result == 0)
            {
                outputText.text = "사용할 수 없는 닉네임입니다";
            }
            else if (result == 1)
            {
                outputText.text = "사용 가능한 닉네임입니다";
            }
            else if (result == 2)
            {
                outputText.text = "현재 닉네임과 같습니다";
            }
            else if (result == 3)
            {
                outputText.text = "변경할 닉네임을 입력해주세요";
            }
        }


        public void ChangeNicknameSuccess()
        {
            if (outputText.text == "사용 가능한 닉네임입니다")
            {
                changeNicknameUI.SetActive(false);
                actionSuccessPanelUI.SetActive(true);
                actionSuccessText.text = "닉네임이 성공적으로 변경되었습니다";
                FirebaseAuth.UpdateNickname(newNickname.text);
                Debug.Log("newnickname@@");
                Debug.Log(newNickname.text);
            }
        }

        //2. 비번 변경
        public void ChangePwUI()
        {
            ClearUI();
            changePasswordUI.SetActive(true);
        }

        public void ChangePwSuccess()
        {
            ClearUI();
            actionSuccessPanelUI.SetActive(true);
            actionSuccessText.text = "비밀번호가 성공적으로 변경되었습니다";
        }

        public void SubmitNewPwButton()
        {
            if ((changePasswordInputField.text == changePasswordConfirmInputField.text) == true && changePasswordConfirmInputField.text.Length >= 6)
            {
                UpdatePw(changePasswordInputField.text);
                ChangePwSuccess();
            }
            else if ((changePasswordInputField.text == changePasswordConfirmInputField.text) == true && changePasswordConfirmInputField.text.Length < 6)
            {
                pwErrorText.text = "비밀번호는 최소 6자리 이상으로 만들어주세요";
            }
            else if ((changePasswordInputField.text == changePasswordConfirmInputField.text) == false)
            {
                pwErrorText.text = "비밀번호가 일치하지 않습니다";
            }
        }

        //4. 회원탈퇴
        public void DeleteUserConfirmUI()
        {
            ClearUI();
            deleteUserConfirmUI.SetActive(true);
        }

        public void DeleteUserSuccess()
        {
            ClearUI();
            actionSuccessPanelUI.SetActive(true);
            actionSuccessText.text = "회원탈퇴가 완료되었습니다";
        }

        public void DeleteUserButton()
        {
            PhotonNetwork.Disconnect();
            DeleteUser();
        }


        //로그아웃
        public void SignOutButton()
        {
            PhotonNetwork.Disconnect();
            SignOut();
        }




        public void CheckAuthState() =>
           FirebaseAuth.CheckAuthState();

        public void UpdateProfilePicture(string newProfile) =>
           FirebaseAuth.UpdateProfilePicture(newProfile);

        public void UpdatePw(string newPw) =>
           FirebaseAuth.UpdatePw(newPw);

        public void DeleteUser() =>
           FirebaseAuth.DeleteUser();

        public void SignOut() =>
           FirebaseAuth.SignOut();


        public void LoginScreen()
        {
            GameManager.instance.ChangeScene("Login");
        }

    }

}