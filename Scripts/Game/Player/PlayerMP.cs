using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using CM.Magic;

namespace Game.players
{
    public class PlayerMP : MonoBehaviour, IMana
    {
        //[SerializeField] private float playerMp;
        [SerializeField] private float MaxMp;
        private static float RecoverMp = 0.005f;
        [SerializeField] private float multiply;

        private void Start()
        {
            //MaxMp = 10;
            playerMp.Value = MaxMp;
        }

        public void MpMinus(float minus)
        {
            if (playerMp.Value - minus > 0)
            {
                {
                    playerMp.Value -= minus;
                }
            }
        }

        public void MaxMpPlus(float MaxPlus)
        {
            MaxMp += MaxPlus;

        }

        private void FixedUpdate()
        {
            if (MaxMp > playerMp.Value)
            {
                if (playerMp.Value + RecoverMp < MaxMp)
                {
                    playerMp.Value += RecoverMp * multiply;
                }
                else playerMp.Value = MaxMp;
            }
        }



        //’Ç‰ÁƒR[ƒh
        private ReactiveProperty<float> playerMp = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<float> onUpdateMp => playerMp;
        public float _MaxMp => MaxMp;
        public bool Reduce(float value)
        {
            if (playerMp.Value < value) return false;
            playerMp.Value -= value;
            if (MaxMp < playerMp.Value) playerMp.Value = MaxMp;
            return true;
        }
    }
}