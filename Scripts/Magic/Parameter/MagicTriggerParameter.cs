using UnityEngine;


namespace CM.Magic.Parameter
{
    public struct MagicTriggerParameter
    {
        //コンストラクタ
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

        //パラメーター endとendRoot以外は不変
        public readonly GameObject origin;          //魔法の使用オブジェクト
        public readonly Transform originRoot;       //魔法の使用座標
        public readonly GameObject end;             //当たったgameObject - filterに入れるだけ、nullでもListに入れるならmissingReferenceは吐かない
        public readonly Vector3? endRoot;           //当たった時のmaticObjectのposition Transformにするとmissing吐くかもだからVector3に null許容
        public readonly Vector3 souce;              //初期発動座標
        public readonly Vector3 target;             //ターゲット


        //Originが生きているか
        public bool isLivingOrigin()
        {
            return !(origin == null || originRoot == null);
        }
    }
}
