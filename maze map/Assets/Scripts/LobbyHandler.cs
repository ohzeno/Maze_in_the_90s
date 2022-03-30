using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace FirebaseWebGL.Examples.Auth
{
    public class LobbyHandler : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        //1. 마이페이지
        private GameObject profileUI;
        [SerializeField]
        //2. 프사 변경페이지
        private GameObject changePfpUI;
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

        [Header("Profile Picture References")]
        [SerializeField]
        private Image lobbyProfilePicture;
        [SerializeField]
        private Image myPageProfilePicture;
        [SerializeField]
        private TMP_InputField profilePictureLink;
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
        string userName = null;
        string photoURL = null;


        private void Start()
        {
            //최초 진입 시 프로필 로드
            CheckAuthState();
        }

        private void GetUsername(string _username)
        {
            userName = _username;

            //둘 다 모이면 로드프로필 실행
            if (userName != null && photoURL != null)
            {
                LoadProfile();
            }
        }

        private void GetPhotoURL(string _photoURL)
        {
            photoURL = _photoURL;

            //둘 다 모이면 로드프로필 실행
            if (userName != null && photoURL != null)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            //Set UI
            StartCoroutine(LoadImage(photoURL.ToString()));
            lobbyUsernameText.text = userName.ToString();
            myPageUsernameText.text = userName.ToString();
        }

        private IEnumerator LoadImage(string _photoUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(_photoUrl);

            request.SetRequestHeader("Access-Control-Allow-Origin", "*");

            request.SetRequestHeader("Access-Control-Allow-Credentials", "true");
            request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, Content-Type, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
            request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS");

            yield return request.SendWebRequest();

            if (request.error != null)
            {
                string output = "알 수 없는 에러가 발생하였습니다 다시 시도해주세요";
                Debug.Log("로드이미지 에러");
                Debug.Log(request.error);

                if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    output = "지원하지 않는 이미지 파일 형식입니다 다른 이미지를 사용하세요";
                }

                Output(output);
            }
            else
            {
                Texture2D image = ((DownloadHandlerTexture)request.downloadHandler).texture;

                lobbyProfilePicture.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);

                myPageProfilePicture.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);
            }
        }

        public void Output(string _output)
        {
            outputText.text = _output;
        }

        public void ClearUI()
        {
            profileUI.SetActive(false);
            changePfpUI.SetActive(false);
            changePasswordUI.SetActive(false);
            actionSuccessPanelUI.SetActive(false);
            deleteUserConfirmUI.SetActive(false);
            actionSuccessText.text = "";
        }

        //1. 마이페이지
        public void ProfileUI()
        {
            ClearUI();
            profileUI.SetActive(true);
            //프로필 바꿨을 때 다시 호출
            CheckAuthState();
        }

        //2. 프사 변경
        public void ChangePfpUI()
        {
            ClearUI();
            changePfpUI.SetActive(true);
        }

        public void ChangePfpSuccess()
        {
            ClearUI();
            actionSuccessPanelUI.SetActive(true);
            actionSuccessText.text = "프로필 사진이 성공적으로 변경되었습니다";
        }

        public void SubmitProfileImageButton()
        {
            UpdateProfilePicture(profilePictureLink.text);
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
            if ((changePasswordInputField.text == changePasswordConfirmInputField.text) == true)
            {
                UpdatePw(changePasswordInputField.text);
            }
            else
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
            DeleteUser();
        }

        //로그아웃
        public void SignOutButton()
        {
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
            GameManager1.instance.ChangeScene("Login");
        }

    }

}