using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    Transform menuPanel;
    Event keyEvent;
    TextMeshProUGUI buttonText;
    KeyCode newKey;
    bool waitingForKey;
    // Start is called before the first frame update
    void Start()
    {
        menuPanel = transform;
        waitingForKey = false;
        for (int i = 0; i < menuPanel.childCount; i++) { 
            if(menuPanel.GetChild(i).name == "Attack")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.attack.ToString();
            else if (menuPanel.GetChild(i).name == "Special")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.special.ToString();
            else if (menuPanel.GetChild(i).name == "Dash")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.dash.ToString();
            else if (menuPanel.GetChild(i).name == "Up")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.up.ToString();
            else if (menuPanel.GetChild(i).name == "Down")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.down.ToString();
            else if (menuPanel.GetChild(i).name == "Left")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.left.ToString();
            else if (menuPanel.GetChild(i).name == "Right")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.right.ToString();
            else if (menuPanel.GetChild(i).name == "Map")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.map.ToString();
            else if (menuPanel.GetChild(i).name == "Shop")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = ControlManager.CM.shop.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        keyEvent = Event.current;
        if (keyEvent.isKey && waitingForKey) { 
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }
    public void StartAssigment(string keyName) {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }
    public void SendText(TextMeshProUGUI text) {
        buttonText = text;
    }
    IEnumerator WaitForKey() {
        while (!keyEvent.isKey)
        {
            buttonText.text = "Waiting...";
            yield return null;
        }
    }
    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;
        yield return WaitForKey();

        switch (keyName) {
            case "attack":
                ControlManager.CM.attack = newKey;
                buttonText.text = ControlManager.CM.attack.ToString();
                PlayerPrefs.SetString("attackKey", ControlManager.CM.attack.ToString());
                break;
            case "special":
                ControlManager.CM.special = newKey;
                buttonText.text = ControlManager.CM.special.ToString();
                PlayerPrefs.SetString("specialKey", ControlManager.CM.special.ToString());
                break;
            case "dash":
                ControlManager.CM.dash = newKey;
                buttonText.text = ControlManager.CM.dash.ToString();
                PlayerPrefs.SetString("dashKey", ControlManager.CM.dash.ToString());
                break;
            case "up":
                ControlManager.CM.up = newKey;
                buttonText.text = ControlManager.CM.up.ToString();
                PlayerPrefs.SetString("upKey", ControlManager.CM.up.ToString());
                break;
            case "down":
                ControlManager.CM.down = newKey;
                buttonText.text = ControlManager.CM.down.ToString();
                PlayerPrefs.SetString("downKey", ControlManager.CM.down.ToString());
                break;
            case "left":
                ControlManager.CM.left = newKey;
                buttonText.text = ControlManager.CM.left.ToString();
                PlayerPrefs.SetString("leftKey", ControlManager.CM.left.ToString());
                break;
            case "right":
                ControlManager.CM.right = newKey;
                buttonText.text = ControlManager.CM.right.ToString();
                PlayerPrefs.SetString("rightKey", ControlManager.CM.right.ToString());
                break;
            case "map":
                ControlManager.CM.map = newKey;
                buttonText.text = ControlManager.CM.map.ToString();
                PlayerPrefs.SetString("mapKey", ControlManager.CM.map.ToString());
                break;
            case "shop":
                ControlManager.CM.shop = newKey;
                buttonText.text = ControlManager.CM.shop.ToString();
                PlayerPrefs.SetString("shopKey", ControlManager.CM.shop.ToString());
                break;
        }
        yield return null;
    }
}
