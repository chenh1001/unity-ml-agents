using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAcademy : Academy
{
    public GameObject player;
    public GameObject[] players;
    public int playersAmount = 3;
    private int realPlayersAmount = 1;
    public PlayerBrain playerBrain;
    public override void InitializeAcademy()
    {
        players = new GameObject[playersAmount+realPlayersAmount];
        for(int i=0;i<playersAmount;i++)
        {
            GameObject prefab = Instantiate(player);
            prefab.GetComponent<ronHalohemAgent>().AgentReset();
            prefab.GetComponent<ronHalohemAgent>().InitializeAgent();
            players[i] = prefab;
            players[i].GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        for (int i = playersAmount; i < realPlayersAmount + playersAmount; i++)
        {
            GameObject prefab = Instantiate(player);
            prefab.GetComponent<ronHalohemAgent>().brain= playerBrain;
            prefab.GetComponent<ronHalohemAgent>().AgentReset();
            prefab.GetComponent<ronHalohemAgent>().InitializeAgent();
            players[i] = prefab;
        }
    }

    public override void AcademyReset()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].activeInHierarchy == true)
            {
                players[i].GetComponent<ronHalohemAgent>().AddReward(1f);
            }
            players[i].GetComponent<ronHalohemAgent>().powerUpNum = Random.Range(1, 3);
        }
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(true);
            players[i].GetComponent<ronHalohemAgent>().AgentReset();
        }
        /*for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<ronHalohemAgent>().AgentReset();
        }*/
    }
    
    public override void AcademyStep()
    {
        int alive = 0;
        for(int i=0;i<players.Length;i++)
        {
            if (players[i].GetComponent<ronHalohemAgent>().health <= 0 && players[i].activeInHierarchy)
            {
                players[i].GetComponent<ronHalohemAgent>().AddReward(-1f);
                players[i].SetActive(false);
            }
            else if(players[i].activeInHierarchy)
                alive++;
        }
        if(alive<=1)
        {
            for(int i = 0; i < players.Length; i++)
            {
                if(players[i].activeInHierarchy==true)
                {
                    players[i].GetComponent<ronHalohemAgent>().AddReward(1f);
                }
                players[i].GetComponent<ronHalohemAgent>().Done();
            }
            AcademyReset();
        }
    }
}
