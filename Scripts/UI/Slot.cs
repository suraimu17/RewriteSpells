using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UniRx;    //�ǉ��s
using System;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler,IDropHandler,IEndDragHandler
{
    public Item item;                      //�X���b�g�̓A�C�e���������
                                           //�A�C�e���̃A�C�R����\��������I�u�W�F�N�g�́A�X���b�g�̎q�Ƃ��č���Ă���



    /*
     * �ǉ��R�[�h �e��C�x���g
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
    public GameObject itemImageObj;        //������

    //private Transform canvasTransform;

    private Hand hand;

    AudioClip clip;

    public Item MyItem { get => item; set => item = value; }        //�A�C�e�����J�v�Z����   

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


    public void OnBeginDrag(PointerEventData eventData)     //�h���b�O�J�n
    {
        if (MyItem ==null) return;      //��̃X���b�g���h���b�O���Ȃ��悤�ɂ���   


        //�h���b�O�̃C���[�W�𕡐�
        //draggingObj = Instantiate(itemImageObj, canvasTransform);
        draggingObj.sprite = itemImage.sprite;
        draggingObj.gameObject.SetActive(true);

        //�������őO�ʂɔz�u
        //draggingObj.transform.SetAsLastSibling();
        //draggingObj.transform.SetAsFirstSibling();

        //�������̐F���Â�����
        itemImage.color = Color.gray;


        //���������C�L���X�g���u���b�N���Ȃ��悤�ɂ���
        //�L�����o�X�O���[�v�ɂ���Ď���


        //����l�ɃA�C�e����n��
        hand.SetGrabbingItem(MyItem);


        //�ǉ��R�[�h Begin�̎��ɂ��ʒm������悤��
        _beginDrag.OnNext(this);
    }

    public void OnDrag(PointerEventData eventData)      //�h���b�O��
    {
        if (MyItem == null) return;     //��̃X���b�g���h���b�O���Ȃ��悤�ɂ���


        //�������|�C���^�[��Ǐ]����悤�ɂ���
        draggingObj.transform.position 
            = hand.transform.position + new Vector3(20,20,0);





    }


    public void SetItem(Item item)      //�Z�b�g�A�C�e�����g��ꂽ�Ƃ��A�����ɃX���b�g�̃A�C�R�����ύX
    {
        MyItem = item;
      

        if(item!=null)                                  //�A�C�e�����\�������Ƃ�
        {
            itemImage.color = new Color(1,1,1,1);   //���̐F�ɖ߂�
            itemImage.sprite = item.MyItemImage;        //�A�C�e���̃A�C�R����\��
        }
        else                                            //�A�C�e�����\������Ȃ��Ƃ�
        {
            itemImage.color = new Color(0, 0, 0, 0);   //�����ɂ���
        }
        
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //����l���A�C�e���������Ă��Ȃ������瑁��return
        if (!hand.IsHavingItem()) return;

        //����l����A�C�e�����󂯎��
        Item gotItem = hand.GetGrabbingItem();

        //���X�����Ă����A�C�e���𒇉�l�ɓn��
        hand.SetGrabbingItem(MyItem);

        SetItem(gotItem);

        GetComponent<AudioSource>().PlayOneShot(clip);


        //�ǉ��R�[�h
        _drop.OnNext(this);
    }

    //OnDrop����ɌĂ΂��
    public void OnEndDrag(PointerEventData eventData)
    {
        //Destroy(draggingObj);
        draggingObj.gameObject.SetActive(false);


        //����l����A�C�e�����󂯎��

        Item gotItem = hand.GetGrabbingItem();
        SetItem(gotItem);


        //�ǉ��R�[�h
        _endDrag.OnNext(this);
    }

   

    
}
