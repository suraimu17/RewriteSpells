using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using CM.Magic.Legacy;



namespace Game.players
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerState playerState;
        private PlayerMover playerMover;
        private PlayerAttacker playerAttacker;
        private MovePointer movePointer;
        private PlayerAnimator playerAnimator;
        [SerializeField] private PlayerHP playerHp;

        private float playerVel;
        
        void Start()
        {
            Init();
        }
        protected void Init()
        {
            playerState = GetComponent<PlayerState>();
            playerMover = GetComponent<PlayerMover>();
            playerAttacker = GetComponent<PlayerAttacker>();
            movePointer = GetComponent<MovePointer>();
            playerAnimator = GetComponent<PlayerAnimator>();
            //playerHp = GetComponent<PlayerHP>();

            playerVel = 0.1f;

            //walk Input
            var Iwalk = this.UpdateAsObservable()
               .Where(_ => Input.GetMouseButtonDown(0))
               .Where(_ => playerAttacker.HadAttacked)
               .Subscribe(_ => {
                   if (playerState.nowPlayer == PlayerState.PlayerActionState.Dead) return;
                   playerState.nowPlayer=PlayerState.PlayerActionState.Walk;
                   playerMover.PlayerWalk(movePointer.Pointer(Input.mousePosition), playerVel);
                   playerAnimator.PlayRunAnimation();
                   })
               .AddTo(this);

            var Irest = this.UpdateAsObservable()
               .Where(_ => playerMover.HadGoaled)
               .Where(_ => playerAttacker.HadAttacked)
               .Subscribe(_ => {
                   if (playerState.nowPlayer == PlayerState.PlayerActionState.Dead) return;
                   playerState.nowPlayer = PlayerState.PlayerActionState.Rest;
                   playerAnimator.StopRunAnimation();
                   //Debug.Log("Rest");
               })
               .AddTo(this);

            //attack Input
            var Iattack = this.UpdateAsObservable()
               .Where(_ => Input.GetMouseButtonDown(1))
               .Where(_ => Time.timeScale > 0)
               .Subscribe(_ =>
               {
                   if (playerState.nowPlayer == PlayerState.PlayerActionState.Dead) return;
                   playerState.nowPlayer = PlayerState.PlayerActionState.Attack;
                   playerMover.PlayerRotate(movePointer.Pointer(Input.mousePosition));
                   //playerAnimator.AttackAnimation();
                   playerAttacker.Attack();
                   //Debug.Log("Magic Attack!");
               })
               .AddTo(this);

            var Idead = this.UpdateAsObservable()
                .Where(_ =>playerHp.Alive==false)
                .DistinctUntilChanged()
                .Subscribe(_ =>
                {
                    playerState.nowPlayer = PlayerState.PlayerActionState.Dead;
                    playerAnimator.DeadAnimation();
                    Debug.Log("Shindayo!");
                })
                .AddTo(this);
        }
    }
}
