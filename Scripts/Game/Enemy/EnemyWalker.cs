using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.enemys
{
    public class EnemyWalker : MonoBehaviour
    {
        public void EnemyLook(GameObject PlayerBody)
        {
            //var direction = PlayerBody.transform.position - this.transform.position;
            var direction = PlayerBody.transform.position;
            direction.y = 0;
            this.transform.LookAt(direction);
            /*
            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, 0.1f);
            */
            //this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        public void EnemyWalk(GameObject PlayerBody, float enemySpeed)
        {

            var direction = PlayerBody.transform.position - this.transform.position;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, 0.1f);

            this.GetComponent<Rigidbody>().MovePosition(this.transform.position+this.transform.forward * enemySpeed);
        }
        public void EnemyPatrol(Vector3 target, float enemySpeed)
        {

            var direction = target - this.transform.position;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, 0.1f);

            this.GetComponent<Rigidbody>().MovePosition(this.transform.position + this.transform.forward * enemySpeed);
        }
    }
}