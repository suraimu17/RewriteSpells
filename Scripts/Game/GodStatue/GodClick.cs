using UnityEngine;
using UnityEngine.EventSystems;
using Game.players;

public class GodClick : MonoBehaviour, IPointerClickHandler
{
    private GodDistanceCheck godDistanceCheck;
    private GodCheckItem godCheckItem;
    private PlayerMover playerMover;
    private PlayerHP playerHP;

    private void Start()
    {
        godDistanceCheck = GetComponent<GodDistanceCheck>();
        godCheckItem = GetComponent<GodCheckItem>();
        playerMover = GameObject.Find("PlayerManager").GetComponent<PlayerMover>();
        playerHP = FindObjectOfType<PlayerHP>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (godDistanceCheck.isNear == true)
        {
            playerMover.GoalSignal();
            Debug.Log("touch");
            if (godCheckItem.HaveSpecialItem()==false) return;

            playerHP.MaxHpPlus();
            //Textで表示させる
            Debug.Log("アイテムを消費して最大HPが増えました!");
        }
    }
}
