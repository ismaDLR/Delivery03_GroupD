using UnityEngine;
using UnityEngine.SceneManagement;

public class InputAction : MonoBehaviour
{
    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void OnPressEnter()
    {
        if (scene.name != "Gameplay")
        {
            SceneManager.LoadScene("Gameplay");
        }
    }

    private void OnPressEscape()
    {
        Application.Quit();
    }
}
