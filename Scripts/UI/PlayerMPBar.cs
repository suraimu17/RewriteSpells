using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMPBar : MonoBehaviour
{
    //�ő�HP�ƌ��݂�HP�B
    [Header("�ő�MP")] public float maxMp;
    [Header("���݂�MP")] public float currentMp;
    [Header("�����MP")] public float mp;
    [Header("��")] public float healMp;
    [Header("MP���R�񕜗�")] public float autohealMp;
    float time = 0f;
    //Slider������
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxMp;
        //���݂�HP���ő�MP�Ɠ����ɁB
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

            //����MP�͂����Ō��߂�B
            float usemp = mp;
            Debug.Log("damage : " + usemp);

            //���݂�MP����_���[�W������
            currentMp = currentMp - usemp;
            Debug.Log("After currentMp : " + currentMp);


            slider.value = currentMp;
            Debug.Log("slider.value : " + slider.value);
        }
        if (Input.GetKeyDown(KeyCode.M) && currentMp < maxMp)
        {
            //��MP�͂����Ō��߂�B
            float heal = healMp;
            Debug.Log("heal : " + heal);

            //���݂�MP�������MP������
            currentMp = currentMp + heal;
            Debug.Log("After currentMp : " + currentMp);


            slider.value = currentMp;
            Debug.Log("slider.value : " + slider.value);
        }

    }
}

