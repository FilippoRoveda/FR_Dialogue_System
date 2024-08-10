using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// Class that handle operation for main menu demo screeen.
    /// </summary>
    public class MainMenuController : Singleton<MainMenuController>
    {
        [SerializeField] private GameObject _creditsScreen;
        [SerializeField] private GameObject _commandsScreen;
      
        public void LoadDemoScene() { SceneManager.LoadScene("02_DemoScene"); }
        public void LoadCreditsScreen() { _creditsScreen.gameObject.SetActive(true); }
        public void CloseCreditsScreen() { _creditsScreen.gameObject.SetActive(false); }
        public void LoadCommandsScreen() { _commandsScreen.gameObject.SetActive(true); }
        public void CloseCommandScreen() { _commandsScreen.gameObject.SetActive(false); }
        public void QuitApplication() { Application.Quit(); }
    }
}
