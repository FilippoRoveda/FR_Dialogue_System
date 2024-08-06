
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class MainMenuManager : Singleton<MainMenuManager>
    {
        [SerializeField] private GameObject creditsScreen;

        public void LoadDemoScene() { SceneManager.LoadScene("02_DemoScene"); }
        public void LoadCreditsScreen() { creditsScreen.gameObject.SetActive(true); }
        public void CloseCreditsScreen() { creditsScreen.gameObject.SetActive(false); }
        public void QuitApplication() { Application.Quit(); }
    }
}
