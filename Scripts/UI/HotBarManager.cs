using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarManager : MonoBehaviour
{
    [SerializeField] private GameObject[] hotbarSlots;
    [SerializeField] private GameObject hotbarSelector;
    [SerializeField] private int selectedSlotIndex = 0;
    public int index => selectedSlotIndex;

    // Update is called once per frame
    public void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") <0) //scrolling up
        {
            selectedSlotIndex=Mathf.Clamp(selectedSlotIndex +1, 0, hotbarSlots.Length-1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0) //scrolling down
        {
            selectedSlotIndex=Mathf.Clamp(selectedSlotIndex -1, 0, hotbarSlots.Length-1);
        }
        hotbarSelector.transform.position = hotbarSlots[selectedSlotIndex].transform.position;
    }
 
}
