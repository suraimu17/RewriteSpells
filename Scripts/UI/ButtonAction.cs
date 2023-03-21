using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    public void OnButtonClick()
    {
        CangeColor();
    }
    public void CangeColor()
    {
        GetComponent<Image>().color = new Color(0, 0, 0,0);
    }
}