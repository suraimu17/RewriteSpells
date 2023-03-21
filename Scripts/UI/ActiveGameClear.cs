using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveGameClear : MonoBehaviour
{
    [SerializeField] GameObject image;


    void Update()
    {

         //ゲームオブジェクト非表示→表示
         image.SetActive(true);

    }
}
