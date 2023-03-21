using System;
using System.Collections.Generic;

using UnityEngine;

using UniRx;

using CM.Magic.Parameter;

namespace CM.Magic.MagicObject
{
    public abstract class BaseMagicObject : MonoBehaviour, ICloneable
    {
        //�g���K�[�p�����[�^�[
        public MagicTriggerParameter param { get; protected set; }

        //�q�b�g�t�B���^�[
        protected List<GameObject> filterList = new List<GameObject>();


        //�C�x���g���s�p
        private Subject<Unit> _onGenerate = new Subject<Unit>();
        private Subject<Unit> _onRemove = new Subject<Unit>();
        private Subject<Unit> _onUpdate = new Subject<Unit>();

        //�C�x���g�o�^�p
        public IObservable<Unit> onGenerate => _onGenerate;
        public IObservable<Unit> onRemove => _onRemove;
        public IObservable<Unit> onUpdate => _onUpdate;


        //�p�����[�^�֘A
        private int level;                                                                  //�X�y���̃��x���Ɠ���
        [SerializeField] private float _damage;                                             //�_���[�W�� �C���X�y�N�^����
        protected float damage => _damage * (1.0f + (float)level / 10f);                    //�T�u�N���X�Q�Ɨp �v�Z��




        public void Init(MagicTriggerParameter param)            //�������̏�����
        {
            //�p�����[�^���X�V
            this.param = param;
        }
        public void Init(int level)                             //�N���[���O�ɌĂяo���p
        {
            this.level = level;
        }


        //�J�n - override��
        public virtual void Trigger()
        {
            //�t�B���^�[�ɒǉ�
            filterList.Add(param.origin);

            //param��endRoot��ǉ�
            param = new MagicTriggerParameter(param, param.end, transform.position);

            //��] ���̎� ��{�I�ȉ�]��originRoot���Q��
            Vector3 direction = (this.param.target - param.originRoot.position).normalized;
            gameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }


        //�N���[���̎���
        public object Clone()
        {
            //����
            BaseMagicObject magicObject = Instantiate(this);

            //���x�����R�s�[
            magicObject.level = level;

            //�A�N�e�B�u��
            magicObject.gameObject.SetActive(true);

            return magicObject;
        }


        //�C�x���g���s�̃��b�v
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
