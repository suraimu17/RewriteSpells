using UnityEngine;

using Game.players;

namespace CM.Magic.MagicObject
{
    public class Potion : BaseMagicObject
    {
        [SerializeField] private int index = 0;

        //トリガー
        public override void Trigger()
        {
            //親呼び出し
            base.Trigger();

            //generate
            OnGenerate();

            //hp取得して回復
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
