using UnityEngine;


namespace CM.Magic.Provider
{
    /// <summary>
    /// ������ �ǂ�����ă^�[�Q�b�g�ƂȂ�GameObject��n�����l����
    /// </summary>
    public class EnemyTargetProvider : MonoBehaviour, ITargetProvider
    {
        //targetGameObject �K���ɓn���O��
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
