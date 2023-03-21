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
        //�����p�����[�^
        [Header("Parameter")]
        private int _level;                             //���x���̓C���X�y�N�^����ݒ�ł��Ȃ�
        [SerializeField] private float _delayTime;
        [SerializeField] private float _coolTime;
        [SerializeField] private float _manaCost;

        //�p�����[�^ ���I
        public int level { private set; get; }
        public float delayTime { private set; get; }
        public float coolTime { private set; get; }
        public float manaCost { private set; get; }

        //�ŏ��̃R���g���[���[
        [Header("BaseController")]
        [SerializeField] private MagicObjectController controller;

        //�T�|�[�g�ǉ��Ώۂ̃R���g���[���[
        [Header("ToAddController")]
        [SerializeField] private List<MagicObjectController> toAddControllerList = new List<MagicObjectController>();

        //�T�|�[�g���X�g
        private readonly List<Support> _supportList = new List<Support>();
        public List<Support> supportList => _supportList;


        //�N�[���^�C����Delay�̏I���ʒm(Delay�͎g���ĂȂ�)
        private Subject<Unit> onEndDelaySubject = new Subject<Unit>();
        private Subject<Unit> onEndCoolTimeSubject = new Subject<Unit>();
        public IObservable<Unit> onEndDelay => onEndDelaySubject;
        public IObservable<Unit> onEndCoolTime => onEndCoolTimeSubject;


        //�^�X�N�֘A
        public bool isCoolTime { private set; get; }        //�N�[���^�C������
        private CancellationTokenSource delayTokenSouce;    //Delay�L�����Z���p




        //�������p�֐�
        public Spell Instantiate(int level)
        {
            Spell spell = Instantiate(this);
            spell._level = level;
            spell.Reload();
            return spell;
        }



        //�����p�̊֐� delay����������
        //�ǂ�����I����҂��Ȃ�
        public void Trigger(MagicTriggerParameter param)
        {
            //�ϐ��̍X�V
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
         * ICastable�̎���
         */
        //�r��
        public float Cast(GameObject origin, Transform root, Vector3 target, IMana mana)
        {
            if (!isCoolTime)  //�N�[���^�C������Ȃ�
            {
                if (mana.Reduce(manaCost)) //�}�i������Ă���
                {
                    MagicTriggerParameter param = new MagicTriggerParameter(origin, root, target);
                    Trigger(param);

                    return delayTime;
                }
            }
            return -1;
        }
        
        //���f
        public void CancelCast()
        {
            if (delayTokenSouce != null) delayTokenSouce.Cancel();
            delayTokenSouce = null;
        }



        //�����[�h�֐�
        public void Reload()
        {
            //������Ԃɂ���
            //�C���X�y�N�^�̐��l�ɏ�����
            level = _level;
            delayTime = _delayTime;
            coolTime = _coolTime;
            manaCost = _manaCost;

            //�R���g���[���[���N���A
            controller.ClearController();



            //�X�V����
            //�p�����[�^�̂ݍX�V����
            foreach (Support support in supportList)
            {
                //�T�|��null�Ȃ��΂�
                if (support == null) continue;

                //�q�I�u�W�F�N�g�ɂ��Ă���
                support.transform.parent = transform;

                //�X�y���̃p�����[�^����
                level = (int)(level * support.addLevel);
                delayTime *= support.addDelayTime;
                coolTime += support.addCoolTime;    //�N�[���^�C���͉��Z��
                manaCost *= support.addManaCost;

                if (coolTime < 0) coolTime = 0;

                //�ǉ����� - �܂��������͂��Ȃ�
                foreach (MagicObjectController controller in toAddControllerList)
                {
                    if (support.addController == null) continue;
                    if (controller.spellObjectType == support.spellObjectType)
                    {
                        controller.AddController(support.addController, support.addControllerType);
                    }
                }
            }

            //�����[�h����(�����Ń����[�h���邱�Ƃ�null�R���g���[���[�ɒǉ��R���g���[���[���Z�b�g���ꂽ��Ԃɂ���)
            controller.Reload(level);
        }


    }
}
