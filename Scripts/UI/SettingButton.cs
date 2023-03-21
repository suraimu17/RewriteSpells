using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public GameObject panel;
  
    public void OnClickSetting()
    {
        panel.SetActive(true);
    }
    public void OffClickSetting()
    {
        panel.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            panel.SetActive(false);
        }
    }
}
