using UnityEngine;

using Game.players;

namespace CM.Magic.MagicObject
{
    public class Potion : BaseMagicObject
    {
        [SerializeField] private int index = 0;

        //�g���K�[
        public override void Trigger()
        {
            //�e�Ăяo��
            base.Trigger();

            //generate
            OnGenerate();

            //hp�擾���ĉ�
            if(index == 0)
            {
                PlayerHP hp = param.origin.GetComponent<PlayerHP>();
                hp.HpPlus((int)damage);
            }else if(index == 1)
            {
                PlayerMP mp = param.origin.GetComponent<PlayerMP>();
                mp.Reduce(-damage);
            }

            //remove
            OnRemove();
            Destroy(gameObject);
        }
    }
}
