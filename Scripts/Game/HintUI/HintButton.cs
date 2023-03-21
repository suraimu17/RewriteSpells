using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HintButton : MonoBehaviour
{
    // Start is called before the first frame update
    //private Button hintButton;
    [SerializeField] private GameObject hint1;
    [SerializeField] private GameObject hint2;
    [SerializeField] private GameObject hint3;
    void Start()
    {
        hint1.SetActive(false);
        hint2.SetActive(false);
        hint3.SetActive(false);
    }
    public void OnClickHint()
    {
        hint1.SetActive(true);
        hint2.SetActive(true);
        hint3.SetActive(true);
    }
    
}
