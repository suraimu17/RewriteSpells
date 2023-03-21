using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.players
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject playerObject;

        public void PlayRunAnimation()
        {
            animator.SetBool("Run", true);
        }
        public void StopRunAnimation()
        {
            animator.SetBool("Run", false);
        }
        public void AttackAnimation(float speed)
        {
            animator.SetFloat("AttackSpeedMul", speed);
            animator.SetTrigger("Attack");
        }
        public void DeadAnimation()
        {
            animator.SetTrigger("Dead");
        }

    }
}