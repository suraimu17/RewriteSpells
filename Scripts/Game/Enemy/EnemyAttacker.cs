using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.players;

using UniRx;

using CM.Magic;
using CM.Magic.Provider;

using Inventories;

namespace Game.enemys
{
    public class EnemyAttacker : MonoBehaviour
    {
        //�ǉ��R�[�h
        [SerializeField] private EnemyTargetProvider provider;
        [SerializeField] private Caster caster;
        [SerializeField] private ItemActioner actioner;

        //�A�j���[�V�����֘A
        [SerializeField] private Animator animator;
        private IAnime anime;
        [SerializeField] private float animationLength = 1.0f;

        //������
        public void Start()
        {
            anime = GetComponent<IAnime>();

            //�C�x���g�o�^
            caster.onCastEvent.Subscribe(e =>
            {
                if(animator) animator.SetFloat("AttackSpeedMul", 1 / e.freezeTime * animationLength);
                anime.PlayEnemyAttack();
            });
        }


        public void EnemyCast(GameObject target)
        {
            provider.targetObj = target;
            actioner.Action();
        }
        public void EnemyCancelAttack()
        {
            caster.CancelCast();
        }


        /*public void EnemyAttack(int damage,PlayerHP hp)
        {

        Debug.Log("Enemy Attack!");
        hp.HpMinus(damage);

        Debug.Log("Enemy Attack!");
        }*/
    }
}
