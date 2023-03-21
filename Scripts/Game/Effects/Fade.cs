using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Effects
{
    public class Fade : MonoBehaviour
    {
        [SerializeField] Image f_image = null;
        private Color color;
        // Start is called before the first frame update
        void Start()
        {
            f_image = GetComponent<Image>();
        }


        private IEnumerator FadeImage(float duration, Action on_completed, bool is_reversing = false)
        {
            if (!is_reversing) f_image.enabled = true;

            var elapsed_time = 0.0f;
            color = f_image.color;

            while (elapsed_time < duration)
            {
                var elapsed_rate = Math.Min(elapsed_time / duration, 1.0f);
                color.a = is_reversing ? 1.0f - elapsed_rate : elapsed_rate;
                f_image.color = color;

                yield return null;
                elapsed_time += Time.deltaTime;
            }
            if (is_reversing) f_image.enabled = false;
            if (on_completed != null) on_completed();
        }

        public void FadeIn(float duration, Action on_completed = null)
        {
            StartCoroutine(FadeImage(duration, on_completed, true));
        }

        public void FadeOut(float duration, Action on_completed = null)
        {
            StartCoroutine(FadeImage(duration, on_completed));
        }
        public void alphaZero() 
        {
            color.a = 255;
            f_image.color = color;
        }
    }
}