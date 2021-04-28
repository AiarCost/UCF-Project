using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSettingsStartup : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork||
           Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            RemoteSettings.Updated += () =>
            {
                GameManager.playerLives = RemoteSettings.GetInt("PlayerStartUpLives", GameManager.playerLives);
            };
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
