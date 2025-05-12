using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ChangeSceneToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
