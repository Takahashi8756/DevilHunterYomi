using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneTransition : MonoBehaviour
{
    private const string MAIN_SCENE_NAME = "PrologueScene";
    public void Transition()
    {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }
}
