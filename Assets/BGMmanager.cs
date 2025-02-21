using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMmanager : MonoBehaviour
{
    public AudioSource GameBGM;
    public AudioSource VictoryBGM;

    private bool isVictory = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayGameBGM();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckPlayer();
    }

    public void PlayGameBGM()
    {
        StopAllBGM();
        GameBGM.Play();
    }

    public void PlayVictryBGM()
    {
        StopAllBGM();
        VictoryBGM.Play();
    }
    
    public void StopAllBGM()
    {
        GameBGM.Stop();
        VictoryBGM.Stop();
    }

    //private void CheckPlayer()
    //{
    //    GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");

    //    int activePlayer = 0;

    //    foreach(GameObject player in Players)
    //    {
    //        if(player.activeSelf)
    //        {
    //            activePlayer++;
    //        }
    //    }

    //    if (activePlayer == 1 && !isVictory == false)
    //    {
    //        PlayVictryBGM();
    //        isVictory = true;
    //    }
    //}

   
}
