using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UniRx;    //追加行
using System;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler,IDropHandler,IEndDragHandler
{
    public Item item;                      //スロットはアイテムを一つ持つ
                                           //アイテムのアイコンを表示させるオブジェクトは、スロットの子として作ってある



    /*
     * 追加コード 各種イベント
     */
    //Subject
    private Subject<Slot> _beginDrag = new Subject<Slot>();
    private Subject<Slot> _drop = new Subject<Slot>();
    private Subject<Slot> _endDrag = new Subject<Slot>();

    //observable
    public IObservable<Slot> beginDrag => _beginDrag;
    public IObservable<Slot> drop => _drop;
    public IObservable<Slot> endDrag => _endDrag;




    [SerializeField]
    public Image itemImage;

    private static Image draggingObj;


    [SerializeField]
    public GameObject itemImageObj;        //複製元

    //private Transform canvasTransform;

    private Hand hand;

    AudioClip clip;

    public Item MyItem { get => item; set => item = value; }        //アイテムをカプセル化   

    //public static Slot Instance;

    /*void Awake()
    {
        if (Instance = null)
            Instance = this;
    }*/

   protected virtual void Start()
    {
        //canvasTransform = FindObjectOfType<Canvas>().transform;

        hand = FindObjectOfType<Hand>();

        clip = gameObject.GetComponent<AudioSource>().clip;

        GameObject dragger = GameObject.Find("Dragger");
        if (dragger)
        {
            draggingObj = dragger.GetComponent<Image>();
            draggingObj.gameObject.SetActive(false);
        }
       // if (MyItem==null) itemImage.color = new Color(0, 0, 0, 0);
        
    }


    public void OnBeginDrag(PointerEventData eventData)     //ドラッグ開始
    {
        if (MyItem ==null) return;      //空のスロットをドラッグしないようにする   


        //ドラッグのイメージを複製
        //draggingObj = Instantiate(itemImageObj, canvasTransform);
        draggingObj.sprite = itemImage.sprite;
        draggingObj.gameObject.SetActive(true);

        //複製を最前面に配置
        //draggingObj.transform.SetAsLastSibling();
        //draggingObj.transform.SetAsFirstSibling();

        //複製元の色を暗くする
        itemImage.color = Color.gray;


        //複製がレイキャストをブロックしないようにする
        //キャンバスグループによって実装


        //仲介人にアイテムを渡す
        hand.SetGrabbingItem(MyItem);


        //追加コード Beginの時にも通知をするように
        _beginDrag.OnNext(this);
    }

    public void OnDrag(PointerEventData eventData)      //ドラッグ中
    {
        if (MyItem == null) return;     //空のスロットをドラッグしないようにする


        //複製がポインターを追従するようにする
        draggingObj.transform.position 
            = hand.transform.position + new Vector3(20,20,0);





    }


    public void SetItem(Item item)      //セットアイテムが使われたとき、同時にスロットのアイコンも変更
    {
        MyItem = item;
      

        if(item!=null)                                  //アイテムが表示されるとき
        {
            itemImage.color = new Color(1,1,1,1);   //元の色に戻す
            itemImage.sprite = item.MyItemImage;        //アイテムのアイコンを表示
        }
        else                                            //アイテムが表示されないとき
        {
            itemImage.color = new Color(0, 0, 0, 0);   //透明にする
        }
        
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //仲介人がアイテムを持っていなかったら早期return
        if (!hand.IsHavingItem()) return;

        //仲介人からアイテムを受け取る
        Item gotItem = hand.GetGrabbingItem();

        //元々持っていたアイテムを仲介人に渡す
        hand.SetGrabbingItem(MyItem);

        SetItem(gotItem);

        GetComponent<AudioSource>().PlayOneShot(clip);


        //追加コード
        _drop.OnNext(this);
    }

    //OnDropが先に呼ばれる
    public void OnEndDrag(PointerEventData eventData)
    {
        //Destroy(draggingObj);
        draggingObj.gameObject.SetActive(false);


        //仲介人からアイテムを受け取る

        Item gotItem = hand.GetGrabbingItem();
        SetItem(gotItem);


        //追加コード
        _endDrag.OnNext(this);
    }

   

    
}
