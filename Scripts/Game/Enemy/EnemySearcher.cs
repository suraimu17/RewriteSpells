using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.enemys
{
    public class EnemySearcher : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyCon;
        [SerializeField] private EnemyRadar radar;

        //private int ChaceLine;
        [SerializeField] private int NearAttackLine;
        [SerializeField] private int FarAttackLine;
        private float timer;
        private void Start()
        {
            timer = 0;

            //ChaceLine = 4;
            enemyCon.targetPosition = EnemyController.TargetPosition.OutOfSearch;
        }

        void OnTriggerStay(Collider collider)
        {
            timer+=0.01f;
            radar.SearchWall(enemyCon.targetActivePos);
            if (radar.hitWall == true)
            {
                if (enemyCon.hadGoal == false)
                {
                    enemyCon.targetPosition = EnemyController.TargetPosition.InSearch;
                }
                else
                {
                    enemyCon.targetPosition = EnemyController.TargetPosition.OutOfSearch;
                    ResetTargetPosition();
                }
            }
            else if (collider.CompareTag("Player"))
            {
                if (timer > 60f)
                {
                    timer = 0;
                    enemyCon.hadGoal = false;
                    SaveTargetPosition(collider);
                }
                //Debug.Log((collider.transform.position - transform.position).magnitude);
                if ((collider.transform.position - transform.position).magnitude < NearAttackLine)
                {
                    enemyCon.hadGoal = true;
                    enemyCon.targetPosition = EnemyController.TargetPosition.InAttackArea;

                    enemyCon.magicType = EnemyController.MagicType.Near;
                }else if ((collider.transform.position - transform.position).magnitude < FarAttackLine)
                {
                    enemyCon.hadGoal = true;
                    enemyCon.targetPosition = EnemyController.TargetPosition.InAttackArea;
                    enemyCon.magicType = EnemyController.MagicType.Far;

                }
                else
                {

                    enemyCon.targetPosition = EnemyController.TargetPosition.InSearch;

                }
                //Debug.Log("Chase!");
            }
        }

        
        private void SaveTargetPosition(Collider collider)
        {
            enemyCon.TargetAtTheMoment=collider.transform.position;
        }
        private void ResetTargetPosition() {
            enemyCon.TargetAtTheMoment = Vector3.zero;
                }


        void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                if (enemyCon.hadGoal == false) enemyCon.targetPosition = EnemyController.TargetPosition.InSearch;
                else {
                    enemyCon.targetPosition = EnemyController.TargetPosition.OutOfSearch;
                    ResetTargetPosition();

                }

            }
        }

    }
}