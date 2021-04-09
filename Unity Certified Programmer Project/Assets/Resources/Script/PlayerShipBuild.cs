using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;

public class PlayerShipBuild_OLD : MonoBehaviour
{
    string placementId_rewardedvideo = "rewardedVideo";
    string gameId = "1234567";


    [SerializeField]
    GameObject[] shopButtons;
    [SerializeField]
    GameObject[] visualWeapons;
    [SerializeField]
    SOActorModel defaultPlayerShip;

    GameObject playerShip;
    GameObject buyButton;
    GameObject bankObj;

    int bank = 600;
    bool purchaseMade = false;

    GameObject target;
    GameObject tmpSelection;

    GameObject textBoxPanel;


    // Start is called before the first frame update
    void Start()
    {
        TurnOffSelectionHighLights();
        textBoxPanel = GameObject.Find("textBoxPanel");
        buyButton = GameObject.Find("BUY ?");
        buyButton.SetActive(false);
        purchaseMade = false;
        bankObj = GameObject.Find("bank");
        bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
        TurnOffPlayerShipVisuals();
        PreparePlayerShipForUpgrade();
        CheckPlatform();


        Debug.Log(visualWeapons[1]);
        Debug.Log(visualWeapons[2]);
    }

    // Update is called once per frame
    void Update()
    {
        AttemptSelection();
    }

    void CheckPlatform()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameId = "REPLACE-THIS-TEXT-FOR-YOUR-GAME-ID";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            gameId = "REPLACE-THIS-TEXT-FOR-YOUR-GAME-ID";
        }
        Monetization.Initialize(gameId, false);
    }

    void TurnOffSelectionHighLights()
    {
        for(int i = 0; i < shopButtons.Length; i++)
        {
            shopButtons[i].SetActive(false);
        }
    }

    GameObject ReturnClickedObject (out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast (ray.origin, ray.direction * 100, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }

    void AttemptSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);

            if(target != null)
            {
                if (target.transform.Find("itemText"))
                {
                    TurnOffSelectionHighLights();
                    Select();
                    UpateDescriptionBox();

                    // IF NOT already sold
                     if (target.transform.Find("itemText").GetComponent<TextMesh>().text != "SOLD")
                    {
                        //can afford
                        Affordable();

                        //can not afford
                        LackOfCredits();
                    }
                     else if (target.transform.Find("itemText").GetComponent<TextMesh>().text == "SOLD")
                    {
                        SoldOut();
                    }
                }

                else if (target.name == "WATCH AD")
                {
                    WatchAdvert();
                }
                else if (target.name == "BUY ?")
                {
                    BuyItem();
                }
                else if (target.name == "START")
                {
                    StartGame();
                }
            }
        }
    }

    void StartGame()
    {
        if (purchaseMade)
        {
            playerShip.name = "UpgradedShip";
            if(playerShip.transform.Find("energy +1(Clone)"))
            {
                playerShip.GetComponent<Player>().Health = 2;

            }
            DontDestroyOnLoad(playerShip);
        }
        GameManager.Instance.GetComponent<Scene_Manager>().BeginGame(GameManager.gameLevelScene);
    }

    void BuyItem()
    {
        Debug.Log("purchased");
        purchaseMade = true;
        buyButton.SetActive(false);
        tmpSelection.SetActive(false);

        for(int i = 0; i < visualWeapons.Length; i++)
        {
            if(visualWeapons[i].name == tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName)
            {
                visualWeapons[i].SetActive(true);
                
            }
        }
        Debug.Log(tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName);
        UpgradeToShip(tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName);

        bank = bank - System.Int32.Parse(tmpSelection.transform.parent.GetComponent<ShopPiece>().ShopSelection.cost);

        bankObj.transform.Find("bankText").GetComponent<TextMesh>().text = bank.ToString();
        tmpSelection.transform.parent.transform.Find("itemText").GetComponent<TextMesh>().text = "SOLD";

    }

    void UpgradeToShip(string upgrade)
    {
        GameObject shipItem = GameObject.Instantiate(Resources.Load("Prefab/Player/" +upgrade)) as GameObject;

        shipItem.transform.SetParent(playerShip.transform);
        shipItem.transform.localPosition = Vector3.zero;
    }

    void Affordable()
    {
        if(bank >= System.Int32.Parse(target.transform.GetComponent<ShopPiece>().ShopSelection.cost))
        {
            Debug.Log("Can buy!");
            buyButton.SetActive(true);

        }
    }

    void LackOfCredits()
    {
        if(bank < System.Int32.Parse(target.transform.Find("itemText").GetComponent<TextMesh>().text))
        {
            Debug.Log("Can't Buy");
        }
    }

    void SoldOut()
    {
        Debug.Log("Sold Out!");
    }

    void WatchAdvert()
    {
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            ShowRewardedAds();
        }
    }

    void ShowRewardedAds()
    {
        StartCoroutine(WaitForAd());
    }

    IEnumerator WaitForAd()
    {
        string placementId = placementId_rewardedvideo;
        while(!Monetization.IsReady(placementId))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;
        if(ad != null)
        {
            ad.Show(AdFinished);
        }
    }

    void AdFinished(ShowResult result)
    {
        if(result == ShowResult.Finished)
        {
            bank += 300;
            bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
            TurnOffSelectionHighLights();
        }
    }

    void TurnOffPlayerShipVisuals()
    {
        for(int i = 0; i< visualWeapons.Length; i++)
        {
            visualWeapons[i].gameObject.SetActive(false);
        }
    }

    void PreparePlayerShipForUpgrade()
    {
        playerShip = GameObject.Instantiate(Resources.Load("Prefab/Player/Player_Ship")) as GameObject;
        Debug.Log(playerShip.name);
        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.localPosition = new Vector3(0, 10000, 0);
        playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
        Debug.Log(playerShip.transform.position);
    }

    void UpateDescriptionBox()
    {
        textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().
            ShopSelection.iconName;

        textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().
    ShopSelection.description;
    }

    void Select()
    {
        tmpSelection = target.transform.Find("SelectionQuad").gameObject;
        tmpSelection.SetActive(true);
    }
}
