using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;//���� ��� ���

public class MapDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mode_dropdown;
    [SerializeField] private TMP_Dropdown map_dropdown;
    [SerializeField] private TextMeshProUGUI text;

    public static string[] mode_list = new string[3] { "-", "Maze", "Hide and Seek" };
    public static string[] maze_list = new string[10] { "-","MazeForest1", "MazeForest2", "MazeForest3", "MazeForest4", "MazeGrave1", "MazeGrave2", "MazeGrave3", "MazeGrave4", "MazeGrave5" };
    public static string[] hideAndSeek_list = new string[2] {"-", "SullaeGrave1" };

    public void OnModeSelect()
    {
        // ���� dropdown�� �ִ� ��� �ɼ��� ����
        map_dropdown.ClearOptions();
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.CurrentRoom.CustomProperties["Mode"] = mode_dropdown.value;
        }
        // ���ο� �ɼ� ������ ���� OptionData ����
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        if (mode_dropdown.value == 2)
        {
            // sullae_list �迭�� �ִ� ��� ���ڿ� �����͸� �ҷ��ͼ� optionList�� ����
            foreach (string str in hideAndSeek_list)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }
        else
        {
            // maze_list �迭�� �ִ� ��� ���ڿ� �����͸� �ҷ��ͼ� optionList�� ����
            foreach (string str in maze_list)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }

        // ������ ������ optionList�� dropdown�� �ɼ� ���� �߰�
        map_dropdown.AddOptions(optionList);

        // ���� dropdown�� ���õ� �ɼ��� 0������ ����
        map_dropdown.value = 0;
    }
    
    public void OnDropdownEvent()
    {
        // ������ map �̸��� ������ 
        if (mode_dropdown.value == 2)
        {
            text.text = $"{hideAndSeek_list[map_dropdown.value]}";
        }
        else if (mode_dropdown.value == 1)
        {
            text.text = $"{maze_list[map_dropdown.value]}";
        }
        Debug.Log("dropdown event!");
        Debug.Log(mode_list[mode_dropdown.value]);
        Debug.Log(text.text);

        //먼저 내용 리셋
        RankingHandler.instance.ClearContents();

        //다시 정보 요청
        RankingHandler.instance.ChangeTab(mode_list[mode_dropdown.value], text.text);
        Debug.Log("~~~");

        //ClearTab(mode_list[mode_dropdown.value], text.text);
        //RankingHandler.instance.ClearTab(mode_list[mode_dropdown.value], text.text);
        //RankingHandler.instance.ChangeTab(mode_list[mode_dropdown.value], text.text);
    }

    public void mapChange()
    {
        PhotonNetwork.CurrentRoom.CustomProperties["Mode"] = mode_dropdown.value;
        PhotonNetwork.CurrentRoom.CustomProperties["Map"] = map_dropdown.value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(PhotonNetwork.CurrentRoom.CustomProperties);
    }
}
