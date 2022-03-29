using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MapDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mode_dropdown;
    [SerializeField] private TMP_Dropdown map_dropdown;
    [SerializeField] private TextMeshProUGUI text;
    private string[] maze_list = new string[3] { "Forest Stage1", "Forest Stage2", "Tokyo Stage1" };
    private string[] sullae_list = new string[4] { "apple", "mango", "juice", "pepper" };

    public void OnModeSelect()
    {
        // 현재 dropdown에 있는 모든 옵션을 제거
        map_dropdown.ClearOptions();

        // 새로운 옵션 설정을 위한 OptionData 생성
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        if (mode_dropdown.value == 1)
        {
            // sullae_list 배열에 있는 모든 문자열 데이터를 불러와서 optionList에 저장
            foreach (string str in sullae_list)
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
            text.text = $"{sullae_list[map_dropdown.value]}";
        } else
        {
            text.text = $"{maze_list[map_dropdown.value]}";
        }
        

    }
}
