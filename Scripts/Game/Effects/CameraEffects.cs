using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.camera
{
    public class CameraEffects : MonoBehaviour
    {
        // Start is called before the first frame update
        //public Camera mainCamera;
        private float duration;
        private float magnitude;

        void Start()
        {

            duration =0.5f;
            magnitude =0.07f;
        }
        public void CameraShake()
        {
            StartCoroutine(DoShake());

        }
        private IEnumerator DoShake()
        {
            var pos = transform.localPosition;

            var elapsed = 0f;

            while (elapsed < duration)
            {
                var x = pos.x + Random.Range(-1f, 1f) * magnitude;
                var y = pos.y + Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, y, pos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = pos;
        }
        // Update is called once per frame

    }
}