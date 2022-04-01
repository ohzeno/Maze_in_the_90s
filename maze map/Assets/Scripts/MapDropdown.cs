using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;//포톤 기능 사용

public class MapDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mode_dropdown;
    [SerializeField] private TMP_Dropdown map_dropdown;
    [SerializeField] private TextMeshProUGUI text;
    public static string[] mode_list = new string[2] { "Maze", "Hide and Seek" };
    public static string[] maze_list = new string[5] { "MazeForest1", "MazeForest2", "MazeForest3", "MazeForest4", "MazeGrave1" };
    public static string[] hideAndSeek_list = new string[2] { "MazeForest1", "MazeForest2"};

    public void OnModeSelect()
    {
        // 현재 dropdown에 있는 모든 옵션을 제거
        map_dropdown.ClearOptions();
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.CurrentRoom.CustomProperties["Mode"] = mode_list[mode_dropdown.value];
        }
        // 새로운 옵션 설정을 위한 OptionData 생성
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        if (mode_dropdown.value == 1)
        {
            // sullae_list 배열에 있는 모든 문자열 데이터를 불러와서 optionList에 저장
            foreach (string str in hideAndSeek_list)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }
        else
        {
            // maze_list 배열에 있는 모든 문자열 데이터를 불러와서 optionList에 저장
            foreach (string str in maze_list)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }

        // 위에서 생성한 optionList를 dropdown의 옵션 값에 추가
        map_dropdown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        map_dropdown.value = 0;
    }


    public void OnDropdownEvent(int index)
    {
        // 선택한 map 이름을 보여줌 
        if (mode_dropdown.value == 1)
        {
            text.text = $"{hideAndSeek_list[map_dropdown.value]}";
        }
        else
        {
            text.text = $"{maze_list[map_dropdown.value]}";
        }
    }
    public void mapChange()
    {
        PhotonNetwork.CurrentRoom.CustomProperties["Mode"] = mode_dropdown.value;
        PhotonNetwork.CurrentRoom.CustomProperties["Map"] = map_dropdown.value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(PhotonNetwork.CurrentRoom.CustomProperties);
    }
}
