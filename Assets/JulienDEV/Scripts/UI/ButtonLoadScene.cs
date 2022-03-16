using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
    [SerializeField] private string _gamePlayScene;

    public void StartButton()
    {
        SceneManager.LoadScene(_gamePlayScene, LoadSceneMode.Single);
    }
}
