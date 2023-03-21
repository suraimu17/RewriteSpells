using UnityEngine;

using CM.Magic.Parameter;


namespace CM.Magic.MagicObject
{
    public class Mover : BaseMagicObject
    {
        [SerializeField] private bool isParentMove;
        //������
        public override void Trigger()
        {
            //�e��Init
            base.Init(param);


            //��������
            OnGenerate();

            //�ړ�����
            if (isParentMove)
            {
                Vector3 vec = param.origin.transform.parent.position;
                vec = new Vector3(param.target.x, vec.y, param.target.z);
                param.origin.transform.parent.position = vec;
            }
            else param.origin.transform.position += param.target;

            //�p�����[�^�̍X�V
            this.param = new MagicTriggerParameter(this.param, this.param.origin, this.param.originRoot.position);

            //�폜
            OnRemove();
            Destroy(gameObject);
        }
    }
}
