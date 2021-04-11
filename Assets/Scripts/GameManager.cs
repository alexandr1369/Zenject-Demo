using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        RUNNING = 1,
        PAUSED = 2
    }

    private GameState _currentGameState = GameState.RUNNING;
    private GameDifficulty _gameDifficulty;
    public GameDifficulty GameDifficulty { get => _gameDifficulty; }

    private void Init()
    {
        if (PlayerPrefs.HasKey("GAME_DIFFICULTY"))
        {
            string gameDifficulty = PlayerPrefs.GetString("GAME_DIFFICULTY");
            _gameDifficulty = gameDifficulty == "HARD" ? GameDifficulty.HARD : GameDifficulty.NORMAL;
        }
        else
        {
            _gameDifficulty = GameDifficulty.NORMAL;
            PlayerPrefs.SetString("GAME_DIFFICULTY", _gameDifficulty.ToString());
        }
        print(_gameDifficulty);
    }

    public TextAsset GetJsonLocationData()
    {
        Init();

        string dataPath = "Textures/Location/";
        switch (_gameDifficulty)
        {
            case GameDifficulty.NORMAL: dataPath += "testing_views_settings_normal_level"; break;
            case GameDifficulty.HARD: dataPath += "testing_views_settings_hard_level"; break;
        }
        return Resources.Load<TextAsset>(dataPath);
    }
    public void ToggleGameState()
    {
        _currentGameState = _currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING;
        Time.timeScale = _currentGameState == GameState.RUNNING ? 1 : 0;
    }
    public void ChangeGameDifficulty(GameDifficulty gameDifficulty)
    {
        PlayerPrefs.SetString("GAME_DIFFICULTY", gameDifficulty.ToString());

        // delay for completing button sound
        StartCoroutine(WaitBeforeAction(ReloadCurrentScene, .05f));
    }
    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region Utils

    private System.Collections.IEnumerator WaitBeforeAction(System.Action action, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }

    #endregion
}