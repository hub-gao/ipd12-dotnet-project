using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLevel2 : MonoBehaviour {

    public GameObject LastCoin;

    void start()
    {
        LastCoin.SetActive(true);
    }

    void OnTriggerEnter()
    {
        LastCoin.SetActive(false);
        Application.LoadLevel("Level-02");
    }
}
