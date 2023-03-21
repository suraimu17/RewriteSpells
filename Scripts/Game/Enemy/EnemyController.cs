using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.players;
using Game.Manager;
using Inventories.Enemy;
using Inventories.Input;
using UniRx;
using UniRx.Triggers;



namespace Game.enemys
{
    public class EnemyController : MonoBehaviour, IMove
    {
        [SerializeField] public GameObject PlayerBody;
        [SerializeField] PlayerHP hp;

        [SerializeField] private IndexInputEnemy inputter;

        [SerializeField] private int firstExistFloor;
        [SerializeField] private int skipFloor;
        [SerializeField] private int lastExistFloor;
        public enum EnemyState
        {
            Rest,
            Walk,
            Attack,
            Wait,
            Dead
        }
        public enum TargetPosition
        {
            InAttackArea,
            InSearch,
            OutOfSearch
        }
        public enum EnemyType 
        {
            Skelton,
            Spider,
            Wizard,
            Golem,
            RedSpider,
            RedWizard,
            SuperWizard
         }
        public enum MagicType
        {
            Near,
            Far
        }
        public TargetPosition targetPosition;
        public Vector3 TargetAtTheMoment;
        public EnemyState nowState;
        public MagicType magicType;
        public float enemySpeed = 0.03f;
        public EnemyType enemyType;
        

        private EnemyAttacker enemyAttacker;
        private EnemyWalker enemyWalker;
        private IAnime enemyAnimator;

        private Coroutine deadAnime =null;
        public bool hadGoal;

        public float CoolTime = 1.0f;
        private static float DeathAnimeTime = 1.5f;
        private float timeCount;
        public Vector3 targetActivePos;
        [SerializeField]public int[] enemyFloorArray { private set; get; } = new int[100];
        //public int[] enemyFloorArray = new int[100];

        protected virtual void Start()
        {
            hadGoal = true;
            targetPosition = TargetPosition.OutOfSearch;
            nowState = EnemyState.Rest;
            magicType = MagicType.Far;
            enemyAttacker = GetComponent<EnemyAttacker>();
            enemyWalker = GetComponent<EnemyWalker>();
            enemyAnimator = GetComponent<IAnime>();

            this.UpdateAsObservable()
                .Where(_ => this.transform.position.y > 0.7f)
                .Subscribe(_ =>
                {
                    Debug.Log("消えたよ");
                    Destroy(gameObject);
                })
                .AddTo(this);
        }
        public void SetTarget(GameObject newTarget, PlayerHP hpTarget)
        {
            PlayerBody = newTarget;
            hp = hpTarget;
        }
        private IEnumerator WhenChasing(Vector3 pos)
        {

            enemyWalker.EnemyPatrol(TargetAtTheMoment, enemySpeed);
            if ((this.transform.position-TargetAtTheMoment).magnitude<1) hadGoal = true;
            else hadGoal = false;
            enemyAnimator.PlayEnemyWalk();
            yield return null;
        }
        private IEnumerator WhenAttacking()
        {
            hadGoal = true;
            yield return new WaitForSeconds(0f);

            enemyAnimator.StopEnemyWalk();
            this.gameObject.transform.LookAt(hp.transform);
         
            if (magicType == MagicType.Near)
            {
                inputter.index = 0;
                enemyAttacker.EnemyCast(PlayerBody.gameObject);//Magic Type:Near
                Debug.Log("Near");
            }
            else
            {
                inputter.index = 1;
                enemyAttacker.EnemyCast(PlayerBody.gameObject);//Magic Type:Far
                Debug.Log("Far");
            }

        }
        private IEnumerator WhenResting()
        {
            hadGoal = true;
            //enemyWalker.EnemyLook(PlayerBody);
            enemyAnimator.StopEnemyWalk();
            yield return null;
        }
        private IEnumerator WhenDead()
        {
            enemyAttacker.EnemyCancelAttack();  //castCancelˆ—

            enemyAnimator.PlayEnemyDead();
            yield return new WaitForSeconds(DeathAnimeTime);
            this.GetComponent<DropTable>().Drop();
            Destroy(this.gameObject);
            yield break;
        }

        virtual public void AttackStart()
        {
            StartCoroutine(WhenAttacking());
           
        }
        protected void DoEveryFrame()
        {
            targetActivePos = PlayerBody.transform.position;
            if (deadAnime == null)
            {
                if (nowState == EnemyState.Dead)
                {

                    deadAnime = StartCoroutine(WhenDead());
                    return;


                }
                if (nowState == EnemyState.Wait)
                {
                    StartCoroutine(WhenResting());
                    timeCount += 0.01f;
                    if (timeCount > CoolTime)
                    {
                        nowState = EnemyState.Rest;
                        timeCount = 0;
                    }
                }

                if (nowState != EnemyState.Wait)
                {


                    switch (targetPosition)
                    {
                        case TargetPosition.OutOfSearch:
                            nowState = EnemyState.Rest;
                            break;
                        case TargetPosition.InSearch:
                            nowState = EnemyState.Walk;
                            break;
                        case TargetPosition.InAttackArea:
                            nowState = EnemyState.Attack;
                            break;
                        default:
                            break;
                    }
                    switch (nowState)
                    {
                        case EnemyState.Rest:
                            StartCoroutine(WhenResting());
                            break;
                        case EnemyState.Walk:
                            if (hadGoal == false)
                            {
                                StartCoroutine(WhenChasing(TargetAtTheMoment));
                            }
                            else nowState = EnemyState.Rest;
                            break;
                        case EnemyState.Attack:
                            AttackStart();
                            nowState = EnemyState.Wait;
                            timeCount = 0;
                            break;
                        default:
                            break;
                    }

                }
            }
        }
        private void FixedUpdate()
        {
            DoEveryFrame();

        }
        public void SetEnemyFloorData()
        {
            Debug.Log("Setできた"+gameObject.name);
            for (int floorNum = firstExistFloor; floorNum < lastExistFloor; floorNum += skipFloor)
            {
                enemyFloorArray[floorNum] = 1;
            }
        }
    }
}