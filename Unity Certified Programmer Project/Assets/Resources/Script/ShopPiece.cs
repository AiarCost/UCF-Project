using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ShopPiece : MonoBehaviour
{
    [SerializeField]
    SOShopSelection shopSelection;
    public SOShopSelection ShopSelection
    { 
        get { return shopSelection; }
        set { shopSelection = value; }
    }

    void Awake()
    {
        if(transform.GetChild(3).GetComponent<Image>() != null)
        {
            transform.GetChild(3).GetComponent<Image>().sprite = shopSelection.icon;
        }
        if (transform.Find("itemText"))
        {
            GetComponentInChildren<Text>().text = shopSelection.cost.ToString();
        }
    }
}
