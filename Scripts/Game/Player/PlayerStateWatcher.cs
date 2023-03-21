using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.players;
using TMPro;

public class PlayerStateWatcher : MonoBehaviour
{
    
    [SerializeField] private PlayerState playerState;
    [SerializeField] private TextMeshProUGUI stateString;
    
    void Start()
    {
        //playerState = GetComponent<PlayerState>();
        //Debug.Log(playerState.nowPlayer);
        //stateString.text = playerState.nowPlayer.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        stateString.text = playerState.nowPlayer.ToString();
    }
}
