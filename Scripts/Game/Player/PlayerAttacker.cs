using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.camera;

using UniRx;

using CM.Magic;

using Inventories;

namespace Game.players
{
    public class PlayerAttacker : MonoBehaviour
    {
        public bool HadAttacked=true;
        //private Camera mainCamera;
        private CameraEffects cameraEffects;
        private PlayerAnimator playerAnimator;

        //private float waitTime; //wait for cameraEffect


        //private PlayerMP playerMp;
        //private float MpCost;

        [SerializeField] private ItemActioner actioner;
        [SerializeField] private Caster caster;

        void Start()
        {
            //waitTime = 0.4f;
            //MpCost = 2.0f;
            cameraEffects=Camera.main.GetComponent<CameraEffects>();
            //playerMp = GetComponent<PlayerMP>();
            playerAnimator = GetComponent<PlayerAnimator>();

            if (cameraEffects)
            {
                Debug.Log(cameraEffects.name);
            }
            else
            {
                Debug.Log("No game object called wibble found");
            }


            //イベント登録
            caster.onCastEvent.Subscribe(e =>
            {
                playerAnimator.AttackAnimation(1/e.freezeTime);
            });
            caster.onEndCastEvent.Subscribe(e =>
            {
                HadAttacked = true;
            });
        }



        public void Attack()
        {
            if (HadAttacked != true) return;
            
            if(!actioner.Action()) return;  //アクションの実行に失敗した場合return

            HadAttacked = false;

            Debug.Log("Attack");
            //playerMp.MpMinus(MpCost);
 
            
            /*
            StartCoroutine(AttackAnim());
            IEnumerator AttackAnim()
            {
                yield return new WaitForSeconds(1.0f);
                cameraEffects.CameraShake();
                //yield return new WaitForSeconds(1.0f);
                
                HadAttacked = true;
            }
            */
        }

    }
}
