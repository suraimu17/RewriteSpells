using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeSample : MonoBehaviour
{
    [SerializeField]
    SoundManager soundManager;
    [SerializeField]
    AudioClip clip;
    [SerializeField, Min(0)]
    float waitTime = 0.5f;
    bool isPlaying = false;
    WaitForSeconds wait;
    void Start()
    {
        wait = new WaitForSeconds(waitTime);
    }
    void Update()
    {
        if (isPlaying)
        {
            return;
        }
        StartCoroutine(nameof(SeTimer));
    }
    IEnumerator SeTimer()
    {
        isPlaying = true;
        yield return wait;
        soundManager.PlaySe(clip);
        isPlaying = false;
    }
}
