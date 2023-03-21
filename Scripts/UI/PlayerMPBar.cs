using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMPBar : MonoBehaviour
{
    //最大HPと現在のHP。
    [Header("最大MP")] public float maxMp;
    [Header("現在のMP")] public float currentMp;
    [Header("消費するMP")] public float mp;
    [Header("回復")] public float healMp;
    [Header("MP自然回復量")] public float autohealMp;
    float time = 0f;
    //Sliderを入れる
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxMp;
        //現在のHPを最大MPと同じに。
        //currentHp = maxMp;
        slider.value = currentMp;
        Debug.Log("Start currentMp : " + currentMp);
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1f)
        {
            currentMp += autohealMp;
            slider.value = currentMp;
            Debug.Log("slider.value : " + slider.value);
            time = 0;
        }
        if (currentMp < 0)
        {
            currentMp = 0;
        }
        if (currentMp > maxMp)
        {
            currentMp = maxMp;
        }
        if (Input.GetMouseButtonDown(1) && currentMp > mp)
        {

            //消費MPはここで決める。
            float usemp = mp;
            Debug.Log("damage : " + usemp);

            //現在のMPからダメージを引く
            currentMp = currentMp - usemp;
            Debug.Log("After currentMp : " + currentMp);


            slider.value = currentMp;
            Debug.Log("slider.value : " + slider.value);
        }
        if (Input.GetKeyDown(KeyCode.M) && currentMp < maxMp)
        {
            //回復MPはここで決める。
            float heal = healMp;
            Debug.Log("heal : " + heal);

            //現在のMPから消費MPを引く
            currentMp = currentMp + heal;
            Debug.Log("After currentMp : " + currentMp);


            slider.value = currentMp;
            Debug.Log("slider.value : " + slider.value);
        }

    }
}

