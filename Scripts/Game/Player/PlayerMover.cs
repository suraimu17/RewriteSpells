using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager;


namespace Game.players
{
    public class PlayerMover : MonoBehaviour
    {
        public bool HadGoaled { private set; get; } = true;//used in PlayerController
        [SerializeField] private GameObject Player;
        [SerializeField] private GameObject PlayerBody;
        private FloorManager floorManager;
        private Rigidbody playerRb;
        
        
        Coroutine someCoroutine;
        private void Start()
        {
            playerRb = Player.GetComponent<Rigidbody>();
            floorManager = FindObjectOfType<FloorManager>();
        }

        public void PlayerWalk(Vector3 movePos,float vel)
        {
            playerRb.velocity = Vector3.zero;

            HadGoaled = false;
            Vector3 playerPos = Player.transform.position;
            movePos.y = playerPos.y;


            someCoroutine=StartCoroutine(MoveAnim());
            IEnumerator MoveAnim()
            {
               
                while ((movePos - playerPos).magnitude>=1)
                {
                    yield return new WaitForFixedUpdate();
                    if (floorManager.IsStairs == true)
                    {
                        HadGoaled = true;
                        yield break;
                    }
                    PlayerBody.transform.LookAt(movePos);
                    playerRb.MovePosition(PlayerBody.transform.position+PlayerBody.transform.forward*vel);
                    playerPos = Player.transform.position;

                }
                
                HadGoaled = true;
            }

        }

        public void PlayerRotate(Vector3 movePos)
        {
            Vector3 playerPos = Player.transform.position;
            movePos.y = playerPos.y;
            PlayerBody.transform.LookAt(movePos);
        }

        public void GoalSignal()
        {
            if (someCoroutine != null)
            {
                StopCoroutine(someCoroutine);
                HadGoaled = true;
            }
        }

        private void Update()
        {
            
            if ((Input.GetMouseButtonDown(1)&&someCoroutine != null )|| (Input.GetMouseButtonDown(0)&&someCoroutine!=null))
            {
                StopCoroutine(someCoroutine);
                HadGoaled = true;
            }
        }

    }
}