using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;


public class DatabaseHandler : MonoBehaviour
{
    public TMP_Text statusText;
    public TMP_InputField pathInputField;
    public TMP_InputField valueInputField;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
    }

    // 데이터 쓰기 : 해당 경로에 값이 있는 경우, 덮어쓰기됨
    public void PostJSON() => FirebaseDatabase.PostJSON(pathInputField.text, valueInputField.text, gameObject.name,
            "DisplayInfo", "DisplayErrorObject");

    // 데이터 읽어오기
    public void GetJSON() => FirebaseDatabase.GetJSON(pathInputField.text, gameObject.name,
            "DisplayData", "DisplayErrorObject");

    // 데이터 쓰기 : 해당 경로에 값이 있는 경우, UID를 키로 값이 추가됨
    public void PushJSON() => FirebaseDatabase.PushJSON(pathInputField.text, valueInputField.text, gameObject.name,
        "DisplayInfo", "DisplayErrorObject");

    // 데이터 쓰기 : 해당 경로의 하위 경로 중 1개의 값만 바꿈
    public void UpdateJSON() => FirebaseDatabase.UpdateJSON(pathInputField.text, valueInputField.text,
        gameObject.name, "DisplayInfo", "DisplayErrorObject");

    // 데이터 삭제
    public void DeleteJSON() => FirebaseDatabase.DeleteJSON(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");


    public void DisplayData(string data)
    {
        statusText.text = data;
    }

    public void DisplayInfo(string info)
    {
        statusText.text = info;
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = JsonUtility.FromJson<FirebaseError>(error);
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        statusText.text = error;
    }
}