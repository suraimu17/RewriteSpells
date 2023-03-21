using System;
using System.Collections.Generic;

using UnityEngine;

using UniRx;

using CM.Magic.Parameter;

namespace CM.Magic.MagicObject
{
    public abstract class BaseMagicObject : MonoBehaviour, ICloneable
    {
        //トリガーパラメーター
        public MagicTriggerParameter param { get; protected set; }

        //ヒットフィルター
        protected List<GameObject> filterList = new List<GameObject>();


        //イベント発行用
        private Subject<Unit> _onGenerate = new Subject<Unit>();
        private Subject<Unit> _onRemove = new Subject<Unit>();
        private Subject<Unit> _onUpdate = new Subject<Unit>();

        //イベント登録用
        public IObservable<Unit> onGenerate => _onGenerate;
        public IObservable<Unit> onRemove => _onRemove;
        public IObservable<Unit> onUpdate => _onUpdate;


        //パラメータ関連
        private int level;                                                                  //スペルのレベルと同じ
        [SerializeField] private float _damage;                                             //ダメージ量 インスペクタから
        protected float damage => _damage * (1.0f + (float)level / 10f);                    //サブクラス参照用 計算式




        public void Init(MagicTriggerParameter param)            //生成時の初期化
        {
            //パラメータを更新
            this.param = param;
        }
        public void Init(int level)                             //クローン前に呼び出す用
        {
            this.level = level;
        }


        //開始 - override可
        public virtual void Trigger()
        {
            //フィルターに追加
            filterList.Add(param.origin);

            //paramにendRootを追加
            param = new MagicTriggerParameter(param, param.end, transform.position);

            //回転 この時 基本的な回転はoriginRootを参照
            Vector3 direction = (this.param.target - param.originRoot.position).normalized;
            gameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }


        //クローンの実装
        public object Clone()
        {
            //生成
            BaseMagicObject magicObject = Instantiate(this);

            //レベルをコピー
            magicObject.level = level;

            //アクティブ化
            magicObject.gameObject.SetActive(true);

            return magicObject;
        }


        //イベント発行のラップ
        protected void OnGenerate()
        {
            _onGenerate.OnNext(Unit.Default);
        }
        protected void OnRemove()
        {
            _onRemove.OnNext(Unit.Default);
        }

    }
}
