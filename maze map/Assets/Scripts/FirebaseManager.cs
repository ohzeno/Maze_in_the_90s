using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public FirebaseUser user;
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


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependencies());
    }

    private IEnumerator CheckAndFixDependencies()
    {
        var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependenciesTask.IsCompleted);

        var dependencyResult = checkAndFixDependenciesTask.Result;

        if (dependencyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyResult}");
        }
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

    }

    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    private void AutoLogin()
    {
        if (user != null)
        {
            if (user.IsEmailVerified)
            {
                GameManager.instance.ChangeScene("MyPage");
            }
            else
            {
                StartCoroutine(SendVerificationEmail());
            }
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("로그아웃됨!!");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log($"로그인됨: {user.DisplayName}");
            }
        }
    }

    public void ClearOutPuts()
    {
        emailErrorText.text = "";
        passwordErrorText.text = "";
        registerNameErrorText.text = "";
        registerEmailErrorText.text = "";
        registerPasswordErrorText.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(LoginLogic(loginEmail.text, loginPassword.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(RegisterLogic(registerUsername.text, registerEmail.text, registerPassword.text, registerConfirmPassword.text));
    }

    private IEnumerator LoginLogic(string _email, string _password)
    {
        Credential credential = EmailAuthProvider.GetCredential(_email, _password);

        var loginTask = auth.SignInWithCredentialAsync(credential);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string output = "알 수 없는 에러가 발생하였습니다 다시 시도해주세요";
            string pwoutput = "";

            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "이메일을 입력해주세요";
                    break;
                case AuthError.InvalidEmail:
                    output = "잘못된 이메일 형식입니다";
                    break;
                case AuthError.MissingPassword:
                    output = "";
                    pwoutput = "비밀번호를 입력해주세요";
                    break;
                case AuthError.WrongPassword:
                    output = "";
                    pwoutput = "비밀번호가 틀립니다";
                    break;
                case AuthError.UserNotFound:
                    output = "존재하지 않는 계정입니다";
                    break;
            }
           emailErrorText.text = output;
           passwordErrorText.text = pwoutput;
        }
        else
        {
            if (user.IsEmailVerified)
            {
                yield return new WaitForSeconds(1f);
                GameManager.instance.ChangeScene("MyPage");
            }
            else
            {
                StartCoroutine(SendVerificationEmail());
            }
        }
    }

    private IEnumerator RegisterLogic(string _username, string _email, string _password, string _confirmPassword)
    {
        if (_username == "")
        {
            registerNameErrorText.text = "닉네임을 입력해주세요";
        }
        else if (_password != _confirmPassword)
        {
            registerPasswordErrorText.text = "비밀번호가 일치하지 않습니다";
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;
                string output = "알 수 없는 에러가 발생하였습니다 다시 시도해주세요";
                string pwoutput = "";
                string emailoutput = "";

                switch (error)
                {
                    case AuthError.InvalidEmail:
                        emailoutput = "잘못된 이메일 형식입니다";
                        output = "";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        emailoutput = "이미 사용 중인 이메일입니다";
                        output = "";
                        break;
                    case AuthError.WeakPassword:
                        pwoutput = "비밀번호는 최소 6자리로 만들어주세요";
                        output = "";
                        break;
                    case AuthError.MissingEmail:
                        emailoutput = "이메일을 입력해주세요";
                        output = "";
                        break;
                    case AuthError.MissingPassword:
                        pwoutput = "비밀번호를 입력해주세요";
                        output = "";
                        break;
                }
                registerNameErrorText.text = output;
                registerEmailErrorText.text = emailoutput;
                registerPasswordErrorText.text = pwoutput;
            }
            else
            {
                UserProfile profile = new UserProfile
                {
                    DisplayName = _username,

                    //TODO: Give Profile Default Photo
                    PhotoUrl = new System.Uri("https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"),
                };

                var defaultUserTask = user.UpdateUserProfileAsync(profile);

                yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);

                if (defaultUserTask.Exception != null)
                {
                    user.DeleteAsync();
                    FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;
                    string output = "알 수 없는 에러가 발생하였습니다 다시 시도해주세요";

                    switch (error)
                    {
                        case AuthError.Cancelled:
                            output = "업데이트 취소됨";
                            break;
                        case AuthError.SessionExpired:
                            output = "세션이 만료됨";
                            break;
                    }
                    registerNameErrorText.text = output;
                }
                else
                {
                    Debug.Log($"유저 생성 성공: {user.DisplayName} ({user.UserId})");

                    StartCoroutine(SendVerificationEmail());
                }
            }
        }
    }

    private IEnumerator SendVerificationEmail()
    {
        if (user != null)
        {
            var emailTask = user.SendEmailVerificationAsync();

            yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

            if (emailTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string output = "알 수 없는 에러가 발생하였습니다 다시 시도해주세요";

                switch (error)
                {
                    case AuthError.Cancelled:
                        output = "인증이 취소되었습니다";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        output = "잘못된 이메일 형식입니다";
                        break;
                    case AuthError.TooManyRequests:
                        output = "요청이 지나치게 많습니다";
                        break;
                }

                AuthUIManager.instance.AwaitVerification(false, user.Email, output);
            }
            else
            {
                AuthUIManager.instance.AwaitVerification(true, user.Email, null);
                Debug.Log("이메일이 성공적으로 전송되었습니다");
            }
        }
    }

    public void UpdateProfilePicture(string _newPfpURL)
    {
        StartCoroutine(UpdateProfilePictureLogic(_newPfpURL));
    }

    private IEnumerator UpdateProfilePictureLogic(string _newPfpURL)
    {
        if (user != null)
        {
            UserProfile profile = new UserProfile();

            try
            {
                UserProfile _profile = new UserProfile
                {
                    PhotoUrl = new System.Uri(_newPfpURL),
                };

                profile = _profile;
            }
            catch
            {
                MyPageManager.instance.Output("이미지를 불러올 수 없습니다. 유효한 링크인지 확인해주세요");
                yield break;
            }

            var pfpTask = user.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(predicate: () => pfpTask.IsCompleted);

            if (pfpTask.Exception != null)
            {
                Debug.LogError($"프로필 사진 변경에 실패했습니다: {pfpTask.Exception}");
            }
            else
            {
                MyPageManager.instance.ChangePfpSuccess();
                Debug.Log("프로필 사진이 성공적으로 변경되었습니다");
            }
        }
    }

    public void SignOut()
    {
        //Signs out of Firebase
        auth.SignOut();
        //Go back to Login UI
        GameManager.instance.ChangeScene("Auth");
    }
}
