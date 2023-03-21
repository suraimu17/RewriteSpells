using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    //最大HPと現在のHP。
    [Header("最大HP")] public float maxHp;
    [Header("現在のHP")] public float currentHp;
    [Header("敵から受けるダメージ")] public float enemydamage;
    [Header("回復")] public float healHp;
    [Header("HP自然回復量")] public float autohealHp;
    float time = 0f;
    //Sliderを入れる
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxHp;
        //現在のHPを最大HPと同じに。
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
            //はここで決める。
            float heal = healHp;
            Debug.Log("heal : " + heal);

            //現在のHPからダメージを引く
            currentHp = currentHp + heal;
            Debug.Log("After currentHp : " + currentHp);


            slider.value = currentHp;
            Debug.Log("slider.value : " + slider.value);
        }
    }
    //ColliderオブジェクトのIsTriggerにチェック入れる。
    private void OnTriggerEnter(Collider collider)
    {
        //Enemy1タグのオブジェクトに触れると発動
        if (collider.gameObject.tag == "Enemy" && currentHp <= maxHp)
        {

            //ダメージはここで決める。
            float damage = enemydamage;
            Debug.Log("damage : " + damage);

            //現在のHPからダメージを引く
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);


            slider.value = currentHp;
            Debug.Log("slider.value : " + slider.value);
        }
    }
}
