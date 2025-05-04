using Cysharp.Threading.Tasks;
using EVERY;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    public static GameState GameState { get { return instance.gameState; } }

    public static GameManager instance;
    public GameState gameState = GameState.Ready;
    

    private void Awake()
    {
        instance = (!instance) ? this : instance;

        SetGameState(GameState.Go, 2f).Forget();     
       
    }

    private void Start()
    {
        Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height/ 2, true);
    }

    public async UniTaskVoid SetGameState(GameState newGameState,float delay)
    {
        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        gameState = newGameState;
    }

    public async UniTaskVoid SetCompleteLevel(bool active, float delay)
    {
        if (gameState is GameState.Finish)
            return;

        SetGameState(GameState.Finish, 0).Forget();
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        if (active)
        {
            UIManager.instance.Win();
        }
        else
        {
            UIManager.instance.Fail();
        }
    }

    public void WinControl()
    {
        int count = EnemyManager.instance.GetEnemyCount();

        if(count == 0)
        {
            SetCompleteLevel(active: true, delay: 1).Forget();
        }
    }

    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
