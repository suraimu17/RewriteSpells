using System;
using Cysharp.Threading.Tasks;

using UnityEngine;

using UniRx;

using CM.Magic.Parameter;


namespace CM.Magic
{
    [RequireComponent(typeof(IMana))]
    [RequireComponent(typeof(ITargetProvider))]
    public class Caster : MonoBehaviour
    {
        [SerializeField] private Transform root;            //発生地点
        [SerializeField] private float castTimeMultiply;    //硬直時間倍率
        [SerializeField] private bool freezeY;              //魔法のY軸を固定するか

        //コンポーネント
        private IMana mana;
        private ITargetProvider provider;


        //使用中の魔法を保存
        private ICastable casting;
        public bool isCasting => casting != null;


        //イベント
        private Subject<CastEvent> onCastSubject = new Subject<CastEvent>();
        private Subject<CastEvent> onEndCastSubject = new Subject<CastEvent>();
        public IObservable<CastEvent> onCastEvent => onCastSubject;
        public IObservable<CastEvent> onEndCastEvent => onEndCastSubject;




        //初期化
        private void Awake()
        {
            //コンポーネントの取得 取得に失敗したらエラー
            mana = GetComponent<IMana>();
            provider = GetComponent<ITargetProvider>();
            if(mana == null || provider == null)
            {
                Debug.LogError("Not Found IMana or ITargetProvider Component");
                enabled = false;
                return;
            }

            //rootが設定されていなかったらCasterにする
            if (!root) root = transform;
        }

        


        //魔法の使用 キャスト成功時硬直時間(castTime)を返す
        public float Cast(ICastable castable)
        {
            if(!isCasting && castable != null)         //詠唱中でないか,castableがnullでないか
            {
                //ターゲットの設定
                Vector3 target = provider.GetTarget(root);
                if (freezeY) target.y = root.position.y;

                //詠唱
                float time = castable.Cast(gameObject, root, target, mana);
                if (time > 0)                                       //正しく詠唱できたかを判定
                {
                    //詠唱した魔法を保持
                    casting = castable;

                    //硬直時間(詠唱時間)を算出
                    float freezeTime = time * castTimeMultiply;

                    //イベント発行
                    CastEvent castEvent = new CastEvent(castable, freezeTime);
                    onCastSubject.OnNext(castEvent);

                    //castingの更新とイベントの発行
                    new Action(async () =>
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(freezeTime));
                        casting = null;
                        onEndCastSubject.OnNext(castEvent);
                    })();

                    return castEvent.freezeTime;
                }
            }
            return 0;
        }



        //魔法のキャンセル
        public void CancelCast()
        {
            if (isCasting)
            {
                casting.CancelCast();
                casting = null;
            }
        }
    }




    //詠唱インターフェース
    public interface ICastable
    {
        //詠唱 成功した場合待機時間を返す 失敗でnullを返す
        public float Cast(GameObject origin, Transform root, Vector3 target, IMana mana);

        //キャンセル
        public void CancelCast();
    }


    //マナインターフェース
    public interface IMana
    {
        public bool Reduce(float value);
    }


    //ターゲット提供インターフェース
    public interface ITargetProvider
    {
        //ターゲット取得
        public Vector3 GetTarget(Transform root); //target元のTransformが引数
    }



    /*
     * freezeTimeに関してのメモ アニメーションの長さが1秒じゃないなら最後に乗算
     * 
     * player => 17F目攻撃、30F目終了 => 30/17
     * 
     * 魔法使い => playerと同じ
     * 
     * 蜘蛛 => 35/24/1.458　
     * 
     * スケルトン => 30/10
     * 
     * ゴーレム => 60/30
     * 
     */
}




