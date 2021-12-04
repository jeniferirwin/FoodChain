using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace FoodChain.UI
{
    public class TitleScreenBehavior : MonoBehaviour
    {
        public void QuitSimulation()
        {
            #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
            #else
                Application.Quit();
            #endif
        }

        public void BeginSimulation()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
