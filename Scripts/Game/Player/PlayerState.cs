using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.players
{
    public class PlayerState : MonoBehaviour
    {
        // Start is called before the first frame update
        public enum PlayerActionState
        {
            Rest,
            Walk,
            Attack,
            Dead
        }

        public PlayerActionState nowPlayer=PlayerActionState.Rest;
       
        
    }
}