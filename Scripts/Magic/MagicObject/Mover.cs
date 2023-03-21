using UnityEngine;

using CM.Magic.Parameter;


namespace CM.Magic.MagicObject
{
    public class Mover : BaseMagicObject
    {
        [SerializeField] private bool isParentMove;
        //初期化
        public override void Trigger()
        {
            //親のInit
            base.Init(param);


            //生成処理
            OnGenerate();

            //移動処理
            if (isParentMove)
            {
                Vector3 vec = param.origin.transform.parent.position;
                vec = new Vector3(param.target.x, vec.y, param.target.z);
                param.origin.transform.parent.position = vec;
            }
            else param.origin.transform.position += param.target;

            //パラメータの更新
            this.param = new MagicTriggerParameter(this.param, this.param.origin, this.param.originRoot.position);

            //削除
            OnRemove();
            Destroy(gameObject);
        }
    }
}
