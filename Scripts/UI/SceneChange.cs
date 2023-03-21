using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private GameObject otherUI;
    public void OnStart()
    {
        if (otherUI)
        {
           if(!otherUI.activeSelf) SceneManager.LoadScene("MainScene");
        }else SceneManager.LoadScene("MainScene");
    }
}
