using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager CM;
    public KeyCode attack { get; set; }
    public KeyCode special { get; set; }
    public KeyCode dash { get; set; }
    public KeyCode up { get; set; }
    public KeyCode down { get; set; }
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode map { get; set; }
    public KeyCode shop { get; set; }   

    private void Awake()
    {
        if (CM == null)
        {
            DontDestroyOnLoad(gameObject);
            CM = this;
        }
        else if (CM != this) {
            Destroy(gameObject);
        }
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attackKey", "N"));
        special = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("specialKey", "M"));
        dash = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("dashKey", "Space"));
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("upKey", "W"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("downKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        map = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("mapKey", "Tab"));
        shop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("shopKey", "E"));
    }
}
