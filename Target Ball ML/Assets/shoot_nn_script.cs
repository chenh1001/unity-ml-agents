using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class shoot_nn_script : Agent
{
    public float speed = 600;
    public GameObject ball;
    Rigidbody ballRgd;
    public int counter = 2;
    public bool hasBall = true;
    public int right = 1;
    public Transform basket;

    public override void InitializeAgent()
    {
        GameObject environment = gameObject.transform.parent.gameObject;
        ballRgd = environment.transform.GetChild(2).GetComponent<Rigidbody>();
        basket = environment.transform.GetChild(right == 1 ? 3 : 4).GetChild(4);
    }

    public override void CollectObservations()
    {
        float x = right * ball.transform.localPosition.x;
        float z = right * ball.transform.localPosition.z;
        AddVectorObs(right * basket.localPosition.x - x);//relative x
        AddVectorObs(basket.localPosition.z - z);//relative z
        AddVectorObs(basket.localPosition.y - ball.transform.localPosition.y);//relative v
        Vector3 newVel = new Vector3(right * ballRgd.velocity.x, ballRgd.velocity.y, right * ballRgd.velocity.z);
        AddVectorObs(newVel);//velocity
        Vector3 newVel2 = new Vector3(right * ballRgd.angularVelocity.x, ballRgd.angularVelocity.y, right * ballRgd.angularVelocity.z);
        AddVectorObs(newVel2);//angularVelocity
    }

    public override void AgentReset()
    {
        hasBall = true;
        //GameObject environment = gameObject.transform.parent.gameObject;
        //transform.localPosition = new Vector3(Random.Range(3f, 18f), 1f, Random.Range(-9f, 9f));
        //ballRgd.angularVelocity = Vector3.zero;
        //ballRgd.velocity = Vector3.zero;
        //ball.transform.localPosition = new Vector3(transform.localPosition.x + right * 1.1f, transform.localPosition.y + 0.75f, transform.localPosition.z);
        //ballRgd.useGravity = false;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        ballRgd.useGravity = true;
        float x = vectorAction[0];
        float y = vectorAction[1] * 5;
        float z = vectorAction[2];
        ballRgd.AddForce(new Vector3(right * x, y, right * z) * speed);
        hasBall = false;
        counter++;
    }

    void FixedUpdate()
    {
        if (counter <= 1)
        {
            RequestDecision();
        }
        if (hasBall)
        {
            ball.transform.localPosition = new Vector3(transform.localPosition.x + right * 1.1f, transform.localPosition.y + 0.75f, transform.localPosition.z);
            ballRgd.angularVelocity = Vector3.zero;
            ballRgd.velocity = Vector3.zero;
        }
        if (ball.transform.localPosition.z > 10 || ball.transform.localPosition.z < -10)
        {
            AddReward(-1.0f);
            Done();
            return;
        }
        if (ball.transform.localPosition.y <= 0.6 || ball.transform.localPosition.y >= 16)
        {
            AddReward(-1.0f);
            Done();
            return;
        }
        if (ball.transform.localPosition.x > 23 || ball.transform.localPosition.x < -20)
        {
            AddReward(-1.0f);
            Done();
            return;
        }
    }

    public void shoot()
    {
        if (counter > 1 && hasBall)
        {
            counter = 0;
        }
    }

    public void basketMade()
    {
        Debug.Log("BALL CALLED MADEBASKET");
        AddReward(1.0f);
        Done();
        return;
    }
}
