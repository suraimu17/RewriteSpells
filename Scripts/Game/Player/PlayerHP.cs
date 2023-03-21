using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

using CM.Magic.Parameter;

namespace Game.players
{
    public class PlayerHP : MonoBehaviour, IDamagable
    {
        //[SerializeField] private int playerHp;
        [SerializeField] private int MaxHp;
        [SerializeField] private int MaxPlus=10;
        
        //���S�ʒm�ł���悤�ɕς��Ă���܂�
        public bool Alive
        {
            get { return _Alive.Value; }
            set { _Alive.Value = value; }
        }
        private ReactiveProperty<bool> _Alive = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<bool> onUpdateAlive => _Alive;

        //�C���X�^���X�l���ǉ����܂���
        public static PlayerHP instance;
        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        
        private void Start()
        {
            Alive = true;
            //MaxHp = 10;
            playerHp.Value = MaxHp;
        }


        public void HpPlus(int plus)
        {
            if (playerHp.Value + plus > MaxHp)
            {
                playerHp.Value =MaxHp;
            }
            else
            {
                playerHp.Value += plus;
            }
        }

        public void HpMinus(int minus)
        {
            playerHp.Value -= minus;
            if(playerHp.Value<=0) Alive = false;
        }

        public void MaxHpPlus()
        {
            MaxHp += MaxPlus;
        }



        //�ȉ��ǉ��R�[�h
        private ReactiveProperty<int> playerHp = new ReactiveProperty<int>();
        public IReadOnlyReactiveProperty<int> onUpdateHealth => playerHp;
        public int _MaxHp => MaxHp;

        public void Damage(float damage)
        {
            HpMinus((int)damage);
        }
    }
}