using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ballScript : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("target"))
        {
            Debug.Log("BASKET!");
            player.GetComponent<BasketBallShooter>().madeBasket();
            return;
        }
    }
}
