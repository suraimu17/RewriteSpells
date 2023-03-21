using UnityEngine;


namespace CM.Magic.Parameter
{
    public struct MagicTriggerParameter
    {
        //�R���X�g���N�^
        public MagicTriggerParameter(GameObject origin, Transform originRoot, Vector3 target)
        {
            this.origin = origin;
            this.originRoot = originRoot;
            end = null;
            endRoot = null;
            souce = originRoot.position;
            this.target = target;
        }
        public MagicTriggerParameter(MagicTriggerParameter param, GameObject end, Vector3? endRoot)
        {
            origin = param.origin;
            originRoot = param.originRoot;
            this.end = end;
            this.endRoot = endRoot;
            souce = param.souce;
            target = param.target;
        }
        public MagicTriggerParameter(GameObject origin, Transform originRoot, GameObject end, Vector3? endRoot, Vector3 souce, Vector3 target)
        {
            this.origin = origin;
            this.originRoot = originRoot;
            this.end = end;
            this.endRoot = endRoot;
            this.souce = souce;
            this.target = target;
        }

        //�p�����[�^�[ end��endRoot�ȊO�͕s��
        public readonly GameObject origin;          //���@�̎g�p�I�u�W�F�N�g
        public readonly Transform originRoot;       //���@�̎g�p���W
        public readonly GameObject end;             //��������gameObject - filter�ɓ���邾���Anull�ł�List�ɓ����Ȃ�missingReference�͓f���Ȃ�
        public readonly Vector3? endRoot;           //������������maticObject��position Transform�ɂ����missing�f������������Vector3�� null���e
        public readonly Vector3 souce;              //�����������W
        public readonly Vector3 target;             //�^�[�Q�b�g


        //Origin�������Ă��邩
        public bool isLivingOrigin()
        {
            return !(origin == null || originRoot == null);
        }
    }
}
