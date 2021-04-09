using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    SOActorModel actorModel;
    GameObject playerShip;
    bool upgradedShip = false;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        GetComponentInChildren<Player>().enabled = true;
    }

    void CreatePlayer()
    {
        //been shopping
        if (GameObject.Find("UpgradedShip"))
        {
            upgradedShip = true;
        }

        //not shopped or died
        //default ship build

        if(!upgradedShip || GameManager.Instance.Died)
        {
            GameManager.Instance.Died = false;
            actorModel = Object.Instantiate(Resources.Load("Script/ScriptableObject/Player_Default")) as SOActorModel;
            playerShip = GameObject.Instantiate(actorModel.actor, transform.position, Quaternion.Euler(270, 180, 0)) as GameObject;
            playerShip.GetComponent<IActorTemplate>().ActorStats(actorModel);
        }
        else
        {
            playerShip = GameObject.Find("UpgradedShip");
        }

        //Create Player
        //actorModel = Object.Instantiate(Resources.Load("Script/ScriptableObject/Player_Default")) as SOActorModel;
        //playerShip = GameObject.Instantiate(actorModel.actor) as GameObject;
        //playerShip.GetComponent<Player>().ActorStats(actorModel);

        // Set Player Up
        playerShip.transform.rotation = Quaternion.Euler(0, 180, 0);
        playerShip.transform.localScale = new Vector3(60, 60, 60);
        playerShip.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(25, 25, 25);
        playerShip.name = "Player";
        playerShip.transform.SetParent(this.transform);
        playerShip.transform.position = Vector3.zero ;
        Debug.Log(playerShip.transform.position);
        playerShip.GetComponent<PlayerTransistion>().enabled = true;
    }

}
