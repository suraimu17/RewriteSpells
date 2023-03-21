using UnityEngine;
using UnityEngine.UI;

using UniRx;

using Game.players;

namespace CM.UI.Presenter
{
    public class HpPresenter : MonoBehaviour
    {

        [SerializeField] private Slider slider;
        [SerializeField] private PlayerHP hp;


        private void Start()
        {
            slider.maxValue = hp._MaxHp;
            hp.onUpdateHealth.Subscribe(currentHealth =>
            {
                if (currentHealth < 0) slider.value = 0;
                else  slider.value = currentHealth;
            }).AddTo(this);

            this.ObserveEveryValueChanged(x => hp._MaxHp)
                .Subscribe(_ =>
                {
                    slider.maxValue = hp._MaxHp;
                })
                .AddTo(this);
        }


    }


}