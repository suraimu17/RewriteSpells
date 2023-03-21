using UnityEngine;

using UniRx;
using UniRx.Triggers;

using CM.Magic.Parameter;


namespace CM.Magic.MagicObject
{
    //必要コンポーネント
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Projectile : BaseMagicObject
    {
        //パラメータ
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private bool isInitialDirection;

        //方向
        private Vector3 direction;


        //移動した距離
        private float distance = 0;

        //rigidBody
        private new Rigidbody rigidbody;


        //初期化
        public override void Trigger()
        {
            //親のInit
            base.Trigger();

            //endObjectもfilterに追加する
            if (this.param.end) filterList.Add(this.param.end);

            //方向を求めて回転 回転処理を上書き
            if(isInitialDirection) direction = direction = (this.param.target - param.souce).normalized;
            else direction = (this.param.target - transform.position).normalized;
            gameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            //rigidBodyの取得
            rigidbody = GetComponent<Rigidbody>();

            //fixedUpdateで進むようにする
            this.FixedUpdateAsObservable()
                .Where(_ => distance < range)
                .Subscribe(_ =>
                {
                    rigidbody.MovePosition(transform.position + direction * speed);
                    distance += speed;
                }).AddTo(this);


            //射程外消滅時にremoveを呼ぶ
            this.FixedUpdateAsObservable()
                .Where(_ => distance >= range)
                .Subscribe(_ =>
                {
                    this.param = new MagicTriggerParameter(this.param, null, transform.position);
                    OnRemove();
                    Destroy(gameObject);
                }).AddTo(this);


            //衝突時
            this.OnTriggerEnterAsObservable()
                .Where(collider => !collider.isTrigger)                         //相手がトリガーじゃない
                .Where(collider => param.isLivingOrigin())                      //Origin生存確認
                .Where(collider => collider.tag != this.param.origin.tag)       //タグがoriginと違う
                .Where(collider => !filterList.Contains(collider.gameObject))   //filterに含まれてない
                .Subscribe(collider =>
                {
                    //構造体を更新
                    this.param = new MagicTriggerParameter(this.param, collider.gameObject, transform.position);

                    //ダメージを与えられる場合は与える
                    IDamagable damagable = collider.GetComponent<IDamagable>();
                    if (damagable != null) damagable.Damage(damage);


                    OnRemove();
                    Destroy(gameObject);
                }).AddTo(this);

            //全て終わったらOnGenerate
            OnGenerate();
        }
    }
}
