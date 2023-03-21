using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    //�ő�HP�ƌ��݂�HP�B
    [Header("�ő�HP")] public float maxHp;
    [Header("���݂�HP")] public float currentHp;
    [Header("�G����󂯂�_���[�W")] public float enemydamage;
    [Header("��")] public float healHp;
    [Header("HP���R�񕜗�")] public float autohealHp;
    float time = 0f;
    //Slider������
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxHp;
        //���݂�HP���ő�HP�Ɠ����ɁB
        //currentHp = maxHp;
        slider.value = currentHp;
        Debug.Log("Start currentHp : " + currentHp);
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1f)
        {
            currentHp += autohealHp;
            slider.value = currentHp;
            Debug.Log("slider.value : " + slider.value);
            time = 0;
        }
        if (currentHp <= 0)
        {
            currentHp = 0;
        }
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //�͂����Ō��߂�B
            float heal = healHp;
            Debug.Log("heal : " + heal);

            //���݂�HP����_���[�W������
            currentHp = currentHp + heal;
            Debug.Log("After currentHp : " + currentHp);


            slider.value = currentHp;
            Debug.Log("slider.value : " + slider.value);
        }
    }
    //Collider�I�u�W�F�N�g��IsTrigger�Ƀ`�F�b�N�����B
    private void OnTriggerEnter(Collider collider)
    {
        //Enemy1�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (collider.gameObject.tag == "Enemy" && currentHp <= maxHp)
        {

            //�_���[�W�͂����Ō��߂�B
            float damage = enemydamage;
            Debug.Log("damage : " + damage);

            //���݂�HP����_���[�W������
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);


            slider.value = currentHp;
            Debug.Log("slider.value : " + slider.value);
        }
    }
}
