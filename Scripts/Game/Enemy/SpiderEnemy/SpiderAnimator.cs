using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.enemys
{
    public class SpiderAnimator : MonoBehaviour,IAnime
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
        /*
        [SerializeField] private Animation anime;
        

        public void PlayEnemyWalk()
        {
            anime.Play("Walk");
        }
        public void StopEnemyWalk()
        {

            //anime.Stop();
            //anime.Stop("Walk");
            //anime.Play("Idle");
        }
        public void PlayEnemyDead()
        {
            anime.Stop("Attack");
            Debug.Log("Dead!");
            anime.Play("Death");
        }
        public void PlayEnemyAttack()
        {
            //anime.Stop("Walk");
            //anime.Stop("Idle");
            anime.Play("Attack");
            
        }
        */
    }
}