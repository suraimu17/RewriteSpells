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
        //追加コード
        [SerializeField] private EnemyTargetProvider provider;
        [SerializeField] private Caster caster;
        [SerializeField] private ItemActioner actioner;

        //アニメーション関連
        [SerializeField] private Animator animator;
        private IAnime anime;
        [SerializeField] private float animationLength = 1.0f;

        //初期化
        public void Start()
        {
            anime = GetComponent<IAnime>();

            //イベント登録
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
