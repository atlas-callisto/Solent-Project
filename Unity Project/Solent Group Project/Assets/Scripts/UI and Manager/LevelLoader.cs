using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    int currentScene = 0;
    float sceneTransitionDelay = 0f;
    float restartSceneDelay = 2f;
    string playerSpwanLocationName;
    GameObject playerRef;
    public static LevelLoader myLevelLoader;
    public GameObject loadingScreenCanvas;
    public Slider loadingSliderBar;
    private void Awake()
    {
        myLevelLoader = this;
        if (FindObjectsOfType<LevelLoader>().Length > 1) Destroy(this.gameObject);
        else DontDestroyOnLoad(this.gameObject);
    }
    public void NewGame() // Load new game
    {
        Time.timeScale = 1;
        playerSpwanLocationName = "GameStartPoint";
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("VerticalSlice"); 
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
        UpdateMusicBox();
        if (SceneManager.GetActiveScene().name == "MainMenu" ||
           SceneManager.GetActiveScene().name == "OptionsMenu" ||
           SceneManager.GetActiveScene().name == "HelpMenu" ||
           SceneManager.GetActiveScene().name == "CreditsMenu") return;
        LevelTransistion.canTransitionn = true;
        playerRef = FindObjectOfType<Player>().gameObject;
        if (GameObject.Find(playerSpwanLocationName)) playerRef.transform.position = GameObject.Find(playerSpwanLocationName).transform.position;     
        loadingScreenCanvas.SetActive(false);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(currentScene);
    }
    public void RestartLevelAfterAPause() // After player dies, this is called
    {
        StartCoroutine(RestartLevelAfterPause());
    }
    private IEnumerator RestartLevelAfterPause()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        yield return new WaitForSeconds(restartSceneDelay);
        Player.currentHealth = Player.maxHealth;
        Player.currentWolfBar = Player.maxWolfBar;
        Time.timeScale = 1;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(currentScene);
    }
   
    public void LoadLevelWithName(string levelName , string spawnLocation)
    {
        Time.timeScale = 1;
        loadingScreenCanvas.SetActive(true);
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
            if (sceneName == levelName)
            {
                currentScene = SceneUtility.GetBuildIndexByScenePath(path);
            }
        }
        playerSpwanLocationName = spawnLocation;
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(LoadSceneAsync());
    }
    private IEnumerator LoadNextLevel(string levelName)
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(sceneTransitionDelay);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(levelName);
    }
    private IEnumerator LoadSceneAsync()
    {
        Time.timeScale = 1;
        AsyncOperation operation = SceneManager.LoadSceneAsync(currentScene);
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp(operation.progress / 0.9f, 0, 1);
            loadingSliderBar.value = progress;
            yield return (null);
        }
    }
    public void FinishGame()
    {
        StartCoroutine(FinishGameTimer());
    }
    private IEnumerator FinishGameTimer()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Cursor.visible = true;
        SceneManager.LoadScene("CreditsMenu");
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadOptionsMenu()
    {
        Time.timeScale = 1;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("OptionsMenu");
    }   
    public void LoadHelpMenu()
    {
        Time.timeScale = 1;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("HelpMenu");
    }
    public void LoadCreditsMenu()
    {
        Time.timeScale = 1;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("CreditsMenu");
    }
    IEnumerator LoadGameOver()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(restartSceneDelay);
        SceneManager.LoadScene("GameOverScene");
    }

    private void UpdateMusicBox()
    {
        MusicBox.myMusicBox.UpdateAudioSource();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
