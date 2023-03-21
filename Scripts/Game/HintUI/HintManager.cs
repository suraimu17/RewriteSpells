using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    
    public void HideMe()
    {
        this.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape)) this.gameObject.SetActive(false);

    }
}
