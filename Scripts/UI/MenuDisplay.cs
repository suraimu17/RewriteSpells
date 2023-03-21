using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplay : MonoBehaviour
{
    [SerializeField] GameObject obj;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            obj.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            obj.SetActive(false);
        }
        
    }
}
