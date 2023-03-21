using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using UniRx;
using UniRx.Triggers;

using CM.Magic;
using CM.Magic.Parameter;

using Inventories;

namespace CM.UI.Presenter
{
    public class CoolTimePresenter : MonoBehaviour
    {
        //Model
        [Header("Model")]
        [SerializeField] private Inventories.Inventory hotBar;
        [SerializeField] private Caster caster;

        //View
        [Header("View")]
        [SerializeField] private GameObject grid;  //対象のグリッド 仮実装(複数に対応するためとあくまでslotの親であるという点が重要
        private Slot[] slots;                      //Viewのスロット配列
        [SerializeField] private Text text;        //textのPrefab


        //非同期なため前のtextが残っていたら消すための辞書
        private Dictionary<ICastable, Text> coolTimeDictionary = new Dictionary<ICastable, Text>();



        public void Awake()
        {
            //イベント登録
            IDisposable disposable = null;
            disposable = grid.UpdateAsObservable().Subscribe(_ =>
            {
                //初期化
                Init();

                //イベントを破棄(2F以降呼び出さないように)
                disposable.Dispose();
            });
        }




        //初期化
        private void Init()
        {
            //モデルの登録
            caster.onCastEvent.Subscribe(e => OnCast(e)).AddTo(this);

            //Viewの取得
            slots = grid.GetComponentsInChildren<Slot>();
            for (int i = 0; i < slots.Length; i++)
            {
                //スロット取得
                Slot slot = slots[i];
            }
        }


        //キャスト時
        private void OnCast(CastEvent castEvent)
        {
            Spell spell = castEvent.castable as Spell;
            if (spell != null)
            {
                //キャストされた魔法が該当のインベントリにあるか検索
                int? index = null;
                for (int i = 0; i < hotBar.maxSlot; i++)
                {
                    ItemStack item = hotBar.GetItemStack(i);
                    if (item != null)
                    {
                        if (castEvent.castable == item.castable)
                        {
                            index = i;
                            break;
                        }
                    }
                }
                if (index == null) return;　//無かったら中断


                //テキストが含まれていたら削除
                if (coolTimeDictionary.ContainsKey(spell))
                {
                    Text preInstance = coolTimeDictionary[spell];
                    if (preInstance != null) Destroy(preInstance);
                    coolTimeDictionary.Remove(spell);
                }

                //テキストを生成して表示
                Text textInstance = Instantiate(text);
                textInstance.gameObject.SetActive(true);


                //textをindexに応じて修正
                Slot slot = slots[index.Value];
                textInstance.transform.parent = text.transform.parent;
                textInstance.transform.position = slot.transform.position;


                //追加する
                coolTimeDictionary.Add(spell, textInstance);


                //時間を初期化
                float coolTime = spell.coolTime;
                float timeSpan = 0.1f;


                //非同期でカウントダウン(0.1秒単位)
                new Action(async () =>
                {

                    while (coolTime > 0)
                    {
                        //テキストの更新
                        if(coolTime > 9.9 && textInstance) textInstance.text = ((int)coolTime).ToString();
                        else if (textInstance) textInstance.text = coolTime.ToString("f1");

                        //ディレイの後秒数減少
                        await UniTask.Delay(TimeSpan.FromSeconds(timeSpan));
                        coolTime -= timeSpan;
                    }
                    if(textInstance != null) Destroy(textInstance.gameObject);  //非同期なのでシーン遷移対策と、上で削除しなかった場合に
                })();
            }
        }
    }

}