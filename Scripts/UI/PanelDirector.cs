using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDirector : MonoBehaviour
{
    public GameObject obj;
    public static PanelDirector Instance;
    public void Start()
    {
        if (Instance == null)
            Instance = this;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            obj.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0;
            obj.SetActive(true);
        }
    } 
}
