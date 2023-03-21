using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.enemys
{
    public class SuperWizardController :  EnemyController
    {
        //EnemyController SuperWizard = new EnemyController();
        //private GameObject Player;
        [SerializeField] private int FarAttackLine;

        private int attackDise = 0;
        protected  override void Start()
        {
            base.Start();
        }
        public override void AttackStart()
        {
            //Debug.Log("Attack");
            attackDise = Random.Range(1, 3);
            if (attackDise<= 1)
            {
                base.AttackStart();
            }
        }

        private void FixedUpdate()
        {
            base.DoEveryFrame();
            if ((PlayerBody.transform.position - transform.position).magnitude>10&& (PlayerBody.transform.position - transform.position).magnitude <FarAttackLine)
            {
                magicType = MagicType.Far;
                targetPosition = TargetPosition.InAttackArea;
            }

        }
    }
}