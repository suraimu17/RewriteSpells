using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.enemys
{
    public class GolemAnimator : MonoBehaviour, IAnime
    {
        [SerializeField] private Animator animator;
        //[SerializeField] private GameObject enemyObject;

        public void PlayEnemyWalk()
        {
            animator.SetBool("IsWalk", true);
        }
        public void StopEnemyWalk()
        {
            animator.SetBool("IsWalk", false);
        }
        public void PlayEnemyDead()
        {
            animator.SetBool("IsDead", true);
        }
        public void PlayEnemyAttack()
        {
            animator.SetTrigger("IsAttack");
        }
    }
}