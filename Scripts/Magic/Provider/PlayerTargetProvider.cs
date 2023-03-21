using UnityEngine;


namespace CM.Magic.Provider
{
    public class PlayerTargetProvider : MonoBehaviour, ITargetProvider
    {
        public Vector3 GetTarget(Transform root)
        {
            //�^�[�Q�b�g
            Vector3 target;

            //���C�L���X�g�Ŏ擾���ĕԂ�
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit))
            {
                target = hit.point;

                //�ǂ��ђʂ��Ă���ꍇ�͕ǑO�ɐݒ�
                Ray toTargetRay = new Ray(root.position, target - root.transform.position);
                if (Physics.Raycast(toTargetRay, out hit, (target - root.position).magnitude))
                {
                    target = hit.point;
                }
            }
            else
            {
                //���C�L���X�g���擾�ł��Ȃ������ꍇgameObject�̐��ʂ�Ԃ�
                target = root.position + root.forward;
            }

            return target;
        }
    }
}
