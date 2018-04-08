using UnityEngine;
using System.Collections;

public class Level2Script : MonoBehaviour
{

    public GameObject NextLevelObject;

    void start()
    {
        NextLevelObject.SetActive(true);
    }

    void OnTriggerEnter()
    {
        NextLevelObject.SetActive(false);
        Application.LoadLevel("Level-02");
    }

}




