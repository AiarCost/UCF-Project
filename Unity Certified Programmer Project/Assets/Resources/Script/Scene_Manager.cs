using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{

    float gameTimer = 0;
    float[] endLevelTimer = { 30, 30, 45 };
    int currentSceneNumber = 0;
    bool gameEnding = false;
    

    Scenes scenes;
    public enum Scenes
    {
        bootUp,
        title,
        shop,
        level1,
        level2,
        level3,
        gameOver
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (currentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            GetScene();
        }

        GameTimer();
    }

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        GetComponent<GameManager>().SetLivesDisplay(GameManager.playerLives);

        if (GameObject.Find("score"))
        {
            
            GameObject.Find("score").GetComponent<Text>().text = ScoreManager.playerScore.ToString();
        }
    }

    void GetScene()
    {
        scenes = (Scenes)currentSceneNumber;
    }

    void GameTimer()
    {
        switch (scenes)
        {
            case Scenes.level1: case Scenes.level2: case Scenes.level3:
                {
                    if(gameTimer < endLevelTimer[currentSceneNumber - 3])
                    {
                        //if level has not completed
                        gameTimer += Time.deltaTime;
                    }
                    else
                    {
                        //if level is completed
                        if (!gameEnding)
                        {
                            gameEnding = true;
                            if(SceneManager.GetActiveScene().name != "level3")
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransistion>().LevelEnds = true;
                            }
                            else
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransistion>().GameCompleted = true;
                            }

                            Invoke("NextLevel", 4);
                        }
                    }
                    break;
                }
        }
    }

    public void ResetScene()
    {
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene);
    }

    void NextLevel()
    {
        gameEnding = false;
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene + 1);
    }

   

    public void GameOver()
    {
        Debug.Log("ENDSCORE: " + GameManager.Instance.GetComponent<ScoreManager>().PlayersScore);
        SceneManager.LoadScene("gameOver");
    }

    public void BeginGame(int gameLevel)
    {
        SceneManager.LoadScene(gameLevel);
    }

}
