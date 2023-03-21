using UnityEngine;
using UnityEngine.SceneManagement;
using Game.players;
using UniRx.Triggers;
using UniRx;
public class SceneChenger : MonoBehaviour
{
    // Update is called once per frame
    private void Start()
    {
        GameOverObserver();
    }

    private void GameOverObserver() 
    {
        this.UpdateAsObservable()
           .Where(_ => PlayerHP.instance.Alive == false)
           .Subscribe(_ =>
           {
               GameOverSceneChange();
           }).AddTo(this);
            
    }
        
    public void GameOverSceneChange()
    {
       SceneManager.LoadScene("GameClear&GameOver");
    }  

    public void GameClearSceneChange()
    {
       SceneManager.LoadScene("GameClear&GameOver");
    }
}

