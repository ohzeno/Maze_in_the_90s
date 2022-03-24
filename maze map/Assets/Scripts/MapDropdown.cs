using TMPro;
using UnityEngine;

public class MapDropdown : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    public void OnDropdownEvent(int index)
    {
        text.text = $"Dropdown Index : {index}";
    }
}
