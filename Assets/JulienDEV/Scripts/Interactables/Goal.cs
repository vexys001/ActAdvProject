using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log($"Goal reached!");
            SceneManager.LoadScene(sceneName: "EndScene");
        }
    }
}
