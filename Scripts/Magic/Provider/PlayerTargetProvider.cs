using UnityEngine;


namespace CM.Magic.Provider
{
    public class PlayerTargetProvider : MonoBehaviour, ITargetProvider
    {
        public Vector3 GetTarget(Transform root)
        {
            //ターゲット
            Vector3 target;

            //レイキャストで取得して返す
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit))
            {
                target = hit.point;

                //壁を貫通している場合は壁前に設定
                Ray toTargetRay = new Ray(root.position, target - root.transform.position);
                if (Physics.Raycast(toTargetRay, out hit, (target - root.position).magnitude))
                {
                    target = hit.point;
                }
            }
            else
            {
                //レイキャストが取得できなかった場合gameObjectの正面を返す
                target = root.position + root.forward;
            }

            return target;
        }
    }
}
