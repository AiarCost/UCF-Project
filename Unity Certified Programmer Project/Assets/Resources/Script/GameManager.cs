using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int playerLives = 3;

    static GameManager instance;
    public static int currentScene = 0;
    public static int gameLevelScene = 3;

    bool died = false;
    public bool Died
    {
        get { return died; }
        set { died = value; }
    }
    public static GameManager Instance 
    {
        get { return instance; }
    }

    private void Awake()
    {
        CheckGameManagerIsInTheScene();
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        LightandCameraSetup(currentScene);
    }

    void Start()
    {
        //CameraSetup();
        SetLivesDisplay(playerLives);
    }

    public void SetLivesDisplay(int players)
    {
        if (GameObject.Find("lives"))
        {
            GameObject lives = GameObject.Find("lives");

            if(lives.transform.childCount < 1)
            {
                for(int i=0; i<5; i++)
                {
                    GameObject life = GameObject.Instantiate(Resources.Load("Prefab/life")) as GameObject;
                    life.transform.SetParent(lives.transform);
                }

                //set visual lives
                for (int i = 0; i < lives.transform.childCount; i++)
                {
                    lives.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
                }

                //remove visual lives
                for(int i = 0; i <(lives.transform.childCount - players); i++)
                {
                    lives.transform.GetChild(lives.transform.childCount - i - 1).localScale = Vector3.zero;
                }
            }
        }



    }

    void CameraSetup(float camSpeed)
    {

        GameObject gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gameCamera.GetComponent<CameraMovement>().CamSpeed = camSpeed;



        //Camera Transform
        gameCamera.transform.position = new Vector3(0, 0, -300);
        gameCamera.transform.eulerAngles = new Vector3(0, 0, 0);

        //Camera Properties
        gameCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        gameCamera.GetComponent<Camera>().backgroundColor = new Color32(0, 0, 0, 255);
    }

    void LightSetup()
    {
        GameObject dirLight = GameObject.Find("Directional Light");
        dirLight.transform.eulerAngles = new Vector3(50, -30, 0);
        dirLight.GetComponent<Light>().color = new Color32(152, 204, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckGameManagerIsInTheScene()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    void LightandCameraSetup(int sceneNumber)
    {
        switch (sceneNumber)
        {
            //Level1, Level2, Level3
            case 3: case 4:  
                {
                    LightSetup();
                    CameraSetup(0);
                    break;
                }
            case 5:
                {
                    CameraSetup(150);
                    break;
                }
        }
    }

    public void LifeLost()
    {
        //lose life
        if(playerLives >= 1)
        {
            playerLives--;
            Debug.Log("Lives left: " + playerLives);
            GetComponent<Scene_Manager>().ResetScene();
        }
        else
        {
            playerLives = 3;
            GetComponent<Scene_Manager>().GameOver();
        }
    }
}
