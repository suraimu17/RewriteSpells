using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.players;

public class GodDistanceCheck : MonoBehaviour
{
    private PlayerMover playerMover;
    public bool isNear = false;
    private void Start()
    {
        isNear = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        playerMover = GameObject.Find("PlayerManager").GetComponent<PlayerMover>();
        playerMover.GoalSignal();
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            
        }
    }

}
