using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private GameObject tapToPlay;
    [SerializeField]
    private GameObject splash1;
    [SerializeField]
    private GameObject splash2;

    public void ShowSplash1()
    {
        tapToPlay.SetActive(false);
        splash1.SetActive(true);
    }

    public void ShowSplash2()
    {
        splash1.SetActive(false);
        splash2.SetActive(true);
    }

	public void LoadScene(string loadedScene){
		SceneManager.LoadScene (loadedScene, LoadSceneMode.Single);
    }

    public void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
