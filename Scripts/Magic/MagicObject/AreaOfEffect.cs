using System;
using Cysharp.Threading.Tasks;

using UnityEngine;

using UniRx;
using UniRx.Triggers;

using CM.Magic.Parameter;

namespace CM.Magic.MagicObject
{
    //必要コンポーネント
    [RequireComponent(typeof(Collider))]
    public class AreaOfEffect : BaseMagicObject
    {
        //パラメータ
        [SerializeField] private float duration;     //効果時間

        
        //初期化
        public override void Trigger()
        {
            //親のInit
            base.Trigger();

            //一定フレーム後に削除
            new Action(async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                if (this != null)
                {
                    Destroy(gameObject);
                    OnRemove();
                }
            })();

            //衝突時
            this.OnTriggerEnterAsObservable()
                .Where(collider => !collider.isTrigger)                         //相手がトリガーじゃない
                .Where(collider => this.param.isLivingOrigin())                 //Origin生存確認
                .Where(collider => collider.tag != this.param.origin.tag)       //タグがoriginと違う
                .Where(collider => !filterList.Contains(collider.gameObject))   //filterに含まれてない
                .Subscribe(collider =>
                {
                    this.param = new MagicTriggerParameter(this.param, param.end, transform.position);  //end変えない

                    //ダメージを与えられる場合は与える
                    IDamagable damagable = collider.GetComponent<IDamagable>();
                    if (damagable != null) damagable.Damage(damage);

                    //filterに追加
                    filterList.Add(collider.gameObject);
                }).AddTo(this);

            //全て終わったらOnGenerate
            OnGenerate();
        }
    }
}
