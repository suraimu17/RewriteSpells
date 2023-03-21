using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Game.Manager;

using CM.Magic.Parameter;

namespace Game.enemys
{
    public class EnemyStatus : MonoBehaviour,IHealth, IDamagable, ILevelProvider
    {

        [SerializeField]private int nowHP;
        [SerializeField]private ReactiveProperty<bool> isDead;
        [SerializeField]private int baseEnemyHP;
        public IReadOnlyReactiveProperty<bool> _isDead => isDead;
        public int EnemyLevel;

        public int Value => EnemyLevel;

        private FloorManager floorManager;
        public int BaseHP
        {
            get
            {
                return baseEnemyHP;
            }
        }

        public void MaxHPSet(int floor)
        {
            nowHP = BaseHP+(BaseHP * floor/4);
        }

        public void DealDamage(int damage)
        {
            nowHP -= damage;
            if (nowHP < 0)
            {
                isDead.Value = true;
            }
        }
        private void Start()
        {
            floorManager = FindObjectOfType<FloorManager>();
            EnemyLevel = floorManager.currentFloor;
            MaxHPSet(floorManager.currentFloor);
            isDead.Value = false;
            Debug.Log(this.gameObject.tag);
            Debug.Log(nowHP);
            var Idead = this.UpdateAsObservable()
                .Where(_ => isDead.Value==true)
                .Subscribe(x =>
                {
                    this.GetComponent<EnemyController>().nowState = EnemyController.EnemyState.Dead;
                })
                .AddTo(this);
        }

        
        public void Damage(float damage)
        {
            DealDamage((int)damage);
        }
    }
}