using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.enemys
{
    public class EnemyRadar : MonoBehaviour, IRadar
    {
        public bool hitWall { private set; get; }
        [SerializeField] float distance;

        
        
        

        public void SearchWall(Vector3 target)
        {
            
            Vector3 rayPosition = transform.position+new Vector3(0,0.7f,0);
            Ray ray = new Ray(rayPosition, (target-transform.position).normalized*distance);
            RaycastHit hit;
           // Debug.Log(ray);
            //Debug.DrawRay(rayPosition, (target - transform.position).normalized * distance, Color.red,0.3f);
            if (Physics.Raycast(ray, out hit, distance))
            {
                //Debug.Log("Wall!");
                if(hit.collider.gameObject.layer== LayerMask.NameToLayer("Wall"))
                hitWall = true;

            }
            else hitWall = false;
        }
        
    }
}