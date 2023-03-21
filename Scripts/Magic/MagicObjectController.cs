using System.Collections.Generic;
using UnityEngine;

using UniRx;

using CM.Magic.MagicObject;
using CM.Magic.Parameter;
using CM.Magic.Legacy;

namespace CM.Magic
{
    [RequireComponent(typeof(IMagicObjectGenerator))]
    public class MagicObjectController : MonoBehaviour
    {
        //生成するスペルオブジェクト
        [Header("MagicObject")]
        [SerializeField] private BaseMagicObject magicObject;
        [SerializeField] private bool isNullController = false;         //NullController判定用(magicObjectにセットされてても無視する)
        public SpellObjectType spellObjectType => SpellObjectType.NONE.GetSpellObjectType(magicObject); //タイプを返す

        //生成
        private IMagicObjectGenerator generator;


        //次のコントローラー 発動/時間経過/消滅時 に移行
        [Header("BaseController")]
        [SerializeField] private MagicObjectController onGenerateController;
        [SerializeField] private MagicObjectController onRemoveController;


        //追加コントローラー 動的に変化する
        private MagicObjectController onGenerateAddController;
        private MagicObjectController onRemoveAddController;




        //生成時に一度だけ呼べば良い初期化
        private void Awake()
        {
            //generator取得 取得に失敗したらエラー
            generator = GetComponent<IMagicObjectGenerator>();
            if(generator == null)
            {
                Debug.LogError("Not Found IMagicObjectGenerator Component");
                enabled = false;
                return;
            }

            //magicObjectを非アクティブ化
            if(magicObject) magicObject.gameObject.SetActive(false);
        }



        //追加コントローラーを削除する
        public void ClearController()
        {
            //追加コントローラーを全て破棄
            onGenerateAddController = null;
            onRemoveAddController = null;

            if (isNullController) magicObject = null;

            //つながっているものも呼び出す
            if (onGenerateController != null) onGenerateController.ClearController();
            if (onRemoveController != null) onRemoveController.ClearController();
        }

        //再読み込み
        public void Reload(int level)
        {
            //magicObjectのレベル初期化
            magicObject.Init(level);

            //各コントローラーがnullの時にmagicObjectを複製するようにする 更にReloadを実行
            //Generate
            if (onGenerateController)
            {                                    
                //Nullコントローラーの時だけ
                if (onGenerateController.isNullController)
                {
                    onGenerateController.magicObject = magicObject;
                    if (onGenerateAddController && !onGenerateAddController.isNullController) onGenerateController.onGenerateAddController = onGenerateAddController; //AddGenerate
                    if (onRemoveController) onGenerateController.onRemoveController = onRemoveController;                                                             //Remove
                    if (onRemoveAddController) onGenerateController.onRemoveAddController = onRemoveAddController;                                                    //AddRemove
                }

                //リロード
                onGenerateController.Reload(level);
            }
            //AddGenerate
            if (onGenerateAddController)
            {
                if (onGenerateAddController.isNullController)
                {
                    onGenerateAddController.magicObject = magicObject;
                    if (onGenerateController && !onGenerateController.isNullController) onGenerateAddController.onGenerateController = onGenerateController;           //Generate
                    if (onRemoveController) onGenerateAddController.onRemoveController = onRemoveController;                                                           //Remove
                    if (onRemoveAddController) onGenerateAddController.onRemoveAddController = onRemoveAddController;                                                  //AddRemove
                }
                onGenerateAddController.Reload(level);
            }
            //Remove
            if (onRemoveController)      //全てにおいてnullable禁止
            {
                if (onRemoveController.isNullController)
                {
                    onRemoveController.magicObject = magicObject;
                    if (onGenerateController && !onGenerateController.isNullController) onRemoveController.onGenerateController = onGenerateController;                //Generate
                    if (onGenerateAddController && !onGenerateAddController.isNullController) onRemoveController.onGenerateAddController = onGenerateAddController;    //AddGenerate
                    if (onRemoveAddController && !onRemoveAddController.isNullController) onRemoveController.onRemoveAddController = onRemoveAddController;            //AddRemove
                }
                onRemoveController.Reload(level);
            }
            //RemoveAdd
            if (onRemoveAddController)
            {
                if (onRemoveAddController.isNullController)
                {
                    onRemoveAddController.magicObject = magicObject;
                    if (onGenerateController && !onGenerateController.isNullController) onRemoveAddController.onGenerateController = onGenerateController;                //Generate
                    if (onGenerateAddController && !onGenerateAddController.isNullController) onRemoveAddController.onGenerateAddController = onGenerateAddController;    //AddGenerate
                    if (onRemoveController && !onRemoveController.isNullController) onRemoveAddController.onRemoveController = onRemoveController;                        //Remove
                }
                onRemoveAddController.Reload(level);
            }
        }






        //コントローラー追加
        public void AddController(MagicObjectController controller, SpellObjectControllerType controllerType)
        {
            //コントローラー追加
            switch (controllerType)
            {
                case SpellObjectControllerType.GENERATE:
                    if(!onGenerateAddController) onGenerateAddController = controller;
                    break;
                case SpellObjectControllerType.REMOVE:
                    if (!onRemoveAddController) onRemoveAddController = controller;
                    break;
            }
        }




        //発火関数 - Initの最後でgenerateを呼んでるパターンもあるため先にイベント登録を済ませる
        public void Trigger(MagicTriggerParameter param)
        {
            //paramのOriginの参照が切れていないか確認
            if (!param.isLivingOrigin()) return;

            //generatorで生成
            List<BaseMagicObject> magicObjectList = generator.Generate(magicObject, param);

            //イベント登録処理
            foreach(BaseMagicObject magicObject in magicObjectList)
            {
                //イベント登録
                magicObject.onGenerate.Subscribe(_ =>
                {
                    if (!magicObject.param.isLivingOrigin()) return;
                    if (onGenerateController) onGenerateController.Trigger(magicObject.param);
                    if (onGenerateAddController) onGenerateAddController.Trigger(magicObject.param);
                }).AddTo(this);
                magicObject.onRemove.Subscribe(_ =>
                {
                    if (!magicObject.param.isLivingOrigin()) return;
                    if (onRemoveController) onRemoveController.Trigger(magicObject.param);
                    if (onRemoveAddController) onRemoveAddController.Trigger(magicObject.param);
                }).AddTo(this);

                //初期化 - generatorで非同期を作る場合はここでもoriginの生存確認をするべき
                //(正しスレッド待ちするならGeneratorインタフェースも変わるのでとりあえずはそのままにしておく)
                //一応最初に生存確認を行っているためこの時点ではoriginは生きている
                //magicObject.Init(param);
                //->paramを変えたいときできないためここではやらない
                //開始に変更
                magicObject.Trigger();
            }
        }


    }




    //Generatorインターフェース
    public interface IMagicObjectGenerator
    {
        public List<BaseMagicObject> Generate(BaseMagicObject prefab, MagicTriggerParameter param);
    }
}
