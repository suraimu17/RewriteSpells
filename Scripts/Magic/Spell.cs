using System;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using UnityEngine;

using UniRx;

using CM.Magic.Parameter;


namespace CM.Magic
{
    public class Spell : MonoBehaviour, ICastable
    {
        //初期パラメータ
        [Header("Parameter")]
        private int _level;                             //レベルはインスペクタから設定できない
        [SerializeField] private float _delayTime;
        [SerializeField] private float _coolTime;
        [SerializeField] private float _manaCost;

        //パラメータ 動的
        public int level { private set; get; }
        public float delayTime { private set; get; }
        public float coolTime { private set; get; }
        public float manaCost { private set; get; }

        //最初のコントローラー
        [Header("BaseController")]
        [SerializeField] private MagicObjectController controller;

        //サポート追加対象のコントローラー
        [Header("ToAddController")]
        [SerializeField] private List<MagicObjectController> toAddControllerList = new List<MagicObjectController>();

        //サポートリスト
        private readonly List<Support> _supportList = new List<Support>();
        public List<Support> supportList => _supportList;


        //クールタイムとDelayの終了通知(Delayは使ってない)
        private Subject<Unit> onEndDelaySubject = new Subject<Unit>();
        private Subject<Unit> onEndCoolTimeSubject = new Subject<Unit>();
        public IObservable<Unit> onEndDelay => onEndDelaySubject;
        public IObservable<Unit> onEndCoolTime => onEndCoolTimeSubject;


        //タスク関連
        public bool isCoolTime { private set; get; }        //クールタイム中か
        private CancellationTokenSource delayTokenSouce;    //Delayキャンセル用




        //初期化用関数
        public Spell Instantiate(int level)
        {
            Spell spell = Instantiate(this);
            spell._level = level;
            spell.Reload();
            return spell;
        }



        //発動用の関数 delayが発生する
        //どちらも終了を待たない
        public void Trigger(MagicTriggerParameter param)
        {
            //変数の更新
            isCoolTime = true;
            delayTokenSouce = new CancellationTokenSource();


            //Delay
            new Action(async () =>
            {
                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: delayTokenSouce.Token);
                    controller.Trigger(param);
                    delayTokenSouce = null;
                    onEndDelaySubject.OnNext(Unit.Default);
                }
                catch(OperationCanceledException e)
                {
                    delayTokenSouce = null;
                }
            })();

            //CoolTime
            new Action(async () =>
            {
                while (isCoolTime)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(coolTime));
                    isCoolTime = false;
                    onEndCoolTimeSubject.OnNext(Unit.Default);
                }
            })();
        }




        /*
         * ICastableの実装
         */
        //詠唱
        public float Cast(GameObject origin, Transform root, Vector3 target, IMana mana)
        {
            if (!isCoolTime)  //クールタイムじゃない
            {
                if (mana.Reduce(manaCost)) //マナが足りている
                {
                    MagicTriggerParameter param = new MagicTriggerParameter(origin, root, target);
                    Trigger(param);

                    return delayTime;
                }
            }
            return -1;
        }
        
        //中断
        public void CancelCast()
        {
            if (delayTokenSouce != null) delayTokenSouce.Cancel();
            delayTokenSouce = null;
        }



        //リロード関数
        public void Reload()
        {
            //初期状態にする
            //インスペクタの数値に初期化
            level = _level;
            delayTime = _delayTime;
            coolTime = _coolTime;
            manaCost = _manaCost;

            //コントローラーをクリア
            controller.ClearController();



            //更新する
            //パラメータのみ更新する
            foreach (Support support in supportList)
            {
                //サポがnullなら飛ばす
                if (support == null) continue;

                //子オブジェクトにしておく
                support.transform.parent = transform;

                //スペルのパラメータ改変
                level = (int)(level * support.addLevel);
                delayTime *= support.addDelayTime;
                coolTime += support.addCoolTime;    //クールタイムは加算式
                manaCost *= support.addManaCost;

                if (coolTime < 0) coolTime = 0;

                //追加処理 - まだ初期化はしない
                foreach (MagicObjectController controller in toAddControllerList)
                {
                    if (support.addController == null) continue;
                    if (controller.spellObjectType == support.spellObjectType)
                    {
                        controller.AddController(support.addController, support.addControllerType);
                    }
                }
            }

            //リロードする(ここでリロードすることでnullコントローラーに追加コントローラーがセットされた状態にする)
            controller.Reload(level);
        }


    }
}
