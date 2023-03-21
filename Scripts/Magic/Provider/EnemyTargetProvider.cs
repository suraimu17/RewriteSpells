using UnityEngine;


namespace CM.Magic.Provider
{
    /// <summary>
    /// 仮実装 どうやってターゲットとなるGameObjectを渡すか考える
    /// </summary>
    public class EnemyTargetProvider : MonoBehaviour, ITargetProvider
    {
        //targetGameObject 適当に渡す前提
        public GameObject targetObj;

        public Vector3 GetTarget(Transform root)
        {
            Vector3 target;
            if (targetObj)
            {
                target = targetObj.transform.position;
            }
            else
            {
                target = root.position + root.transform.forward;
            }

            return target;
        }
    }
}
