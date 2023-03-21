using UnityEngine;
using UnityEngine.UI;

using UniRx;

using Game.players;

namespace CM.UI.Presenter
{
    public class MpPresenter : MonoBehaviour
    {

        [SerializeField] private Slider slider;
        [SerializeField] private PlayerMP mp;


        private void Start()
        {
            slider.maxValue = mp._MaxMp;
            mp.onUpdateMp.Subscribe(currentMp =>
            {
                if (currentMp < 0) slider.value = 0;
                else slider.value = currentMp;
            }).AddTo(this);
        }
    }
}

