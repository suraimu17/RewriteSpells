using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.players
{
    public class MovePointer : MonoBehaviour
    {
        
        public Vector3 Pointer(Vector2 mousePoint)
        {
                mousePoint.x = Mathf.Clamp(mousePoint.x, 0.0f, Screen.width);
                mousePoint.y = Mathf.Clamp(mousePoint.y, 0.0f, Screen.height);
                Camera PlayerCamera = Camera.main;
                Ray touchPointToRay = PlayerCamera.ScreenPointToRay(mousePoint);
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(touchPointToRay, out hitInfo))
                {
                    //Debug.Log(hitInfo.point);
                    
                }
            return hitInfo.point;
            
        }
    }
}