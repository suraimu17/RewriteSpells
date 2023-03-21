using UnityEngine;

namespace Game.players
{
    public class PlayerStairsSearch : MonoBehaviour
    {
        public bool onStairs { private set; get; }

        private float distance=5f;
        private float capsuleColliderRad = 0.42f;

        public void SearchStairs()
        {
            //Debug.DrawRay(transform.position+transform.up,Vector3.down*distance, Color.red, 0.3f);
            if (Physics.CapsuleCast(transform.position + transform.up,
                                    transform.position+transform.up*0.5f,
                                    capsuleColliderRad,
                                    Vector3.down,
                                    out RaycastHit hit,
                                    distance,
                                    LayerMask.GetMask("DownStairs")))
            {
                onStairs = true;
            }
            else onStairs = false;
        }
        private void Update()
        {
            SearchStairs();
        }
    }
}