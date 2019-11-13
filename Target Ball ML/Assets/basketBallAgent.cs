using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class basketBallAgent : Agent
{
    public float speed = 600;
    public int range = 10;
    public int counter = 0;
    public bool testing = false;
    Rigidbody ballRgd;
    public bool allReadyHit = false;
    public Transform basket;
    public Transform player;
    public override void InitializeAgent()
    {
        ballRgd = gameObject.GetComponent<Rigidbody>();
        GameObject environment = gameObject.transform.parent.gameObject;
        basket = environment.transform.GetChild(4);
        player = environment.transform.GetChild(2);
        Debug.Log(basket);
        Debug.Log(player);
        gameObject.transform.localPosition = new Vector3(player.localPosition.x + 0.5f, player.localPosition.y + 0.4f, player.localPosition.z);

    }
    public override void CollectObservations()
    {
        AddVectorObs(basket.localPosition.x-gameObject.transform.localPosition.x);//relative x
        AddVectorObs(basket.localPosition.z-gameObject.transform.localPosition.z);//relative z
        AddVectorObs(basket.localPosition.y - gameObject.transform.localPosition.y);//relative v
        AddVectorObs(gameObject.transform.localPosition);
        AddVectorObs(basket.localPosition);//basket position
        AddVectorObs(ballRgd.velocity);//velocity
        AddVectorObs(ballRgd.angularVelocity);//angularVelocity
    }
    
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        allReadyHit = true;
        ballRgd.useGravity = true;
        vectorAction[0] = Mathf.Clamp(vectorAction[0], 0f, 1f);
        vectorAction[1] = Mathf.Clamp(vectorAction[1], 0f, 1f);
        vectorAction[2] = Mathf.Clamp(vectorAction[2], -1f, 1f);
        float x = vectorAction[0];
        float y = vectorAction[1]*5;
        float z = vectorAction[2];
        ballRgd.AddForce(new Vector3(x,y,z) * speed);
   
        counter++;
    }
    public override void AgentReset()
    {
        counter = 0;
        allReadyHit = false;
        GameObject environment = gameObject.transform.parent.gameObject;
        player.localPosition = new Vector3(Random.Range(-5f, -10f), 9.6f, Random.Range(-35f,-27f));
        //player.localPosition = new Vector3(-6, 9.6f, -31.4f);
        ballRgd.angularVelocity = Vector3.zero;
        ballRgd.velocity = Vector3.zero;
        //gameObject.GetComponent<SphereCollider>().isTrigger = true;
        gameObject.transform.localPosition = new Vector3(player.localPosition.x + 0.5f, player.localPosition.y + 0.4f, player.localPosition.z);
        ballRgd.useGravity = true;
    }
    void FixedUpdate()
    {
        //AddReward(-0.0001f);
        if (counter<=1)
        {
            RequestDecision();
        }
        //float dis = Mathf.Sqrt(Mathf.Pow(this.transform.localPosition.x - basket.transform.localPosition.x, 2) + (Mathf.Pow(this.transform.localPosition.z - basket.transform.localPosition.z, 2)));
        if (this.transform.localPosition.x >=basket.localPosition.x+0.5f)
        {
            float dis = Vector3.Distance(this.gameObject.transform.localPosition, basket.localPosition);
            if(dis>=1f)
            {
                dis = 1f;
            }
            if (basket.localPosition.y > this.gameObject.transform.localPosition.y)
                dis = 1f;
            AddReward(-dis);
            Done();
            return;
        }
        if (gameObject.transform.localPosition.z>-27||gameObject.transform.localPosition.z<-36)
        {
            AddReward(-1.0f);
            Done();
            return;
        }
        if (gameObject.transform.localPosition.y <= 9.5 || gameObject.transform.localPosition.y >= 16)
        {
            AddReward(-1.0f);
            Done();
            return;
        }
        if (gameObject.transform.localPosition.x > 0 || gameObject.transform.localPosition.x < -10)
        {
            AddReward(-1.0f);
            Done();
            return;
        }
        if (gameObject.transform.localPosition.x <player.localPosition.x)
        {
            AddReward(-1.0f);
            Done();
            return;
        }
    }
    

   
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("target"))
        {
            Debug.Log("BASKET!");
            AddReward(1.0f);
            Done();
            return;
        }
       
    }

    public override void AgentOnDone()
    {
        Debug.Log("done");
    }

}
