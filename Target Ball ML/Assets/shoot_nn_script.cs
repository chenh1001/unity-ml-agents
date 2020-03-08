using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class shoot_nn_script : Agent
{
    public float speed = 600;
    public GameObject ball;
    Rigidbody ballRgd;
    public int counter = 0;
    public int right = 1;
    public Transform basket;
    public gameController gc;

    public override void InitializeAgent()
    {
        GameObject environment = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        ballRgd = environment.transform.GetChild(1).GetComponent<Rigidbody>();
        basket = environment.transform.GetChild(right == 1 ? 3 : 4).GetChild(4);
        ballRgd.angularVelocity = Vector3.zero;
        ballRgd.velocity = Vector3.zero;
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
        //hasBall = true;
        
        //GameObject environment = gameObject.transform.parent.gameObject;
        //transform.localPosition = new Vector3(Random.Range(3f, 18f), 1f, Random.Range(-9f, 9f));
        //ballRgd.angularVelocity = Vector3.zero;
        //ballRgd.velocity = Vector3.zero;
        //ball.transform.localPosition = new Vector3(transform.localPosition.x + right * 1.1f, transform.localPosition.y + 0.75f, transform.localPosition.z);
        //ballRgd.useGravity = false;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float x = vectorAction[0];
        float y = vectorAction[1] * 5;
        float z = vectorAction[2];
        ballRgd.AddForce(new Vector3(right * x, y, right * z) * speed);
        GetComponent<BasketBallShooterPlayer>().hasBall = false;
        counter++;
        if (counter > 1)
            Done();
    }

    void FixedUpdate()
    {
        if (counter <= 1)
        {
            RequestDecision();
        }
    }

    public void shoot()
    {
        if (counter > 1 && GetComponent<BasketBallShooterPlayer>().hasBall)
        {
            counter = 0;
            GetComponent<BasketBallShooterPlayer>().timer = 1;
            gc.PlayerWithBall = null;
        }
    }

    public void basketMade()
    {
        Debug.Log("BALL CALLED MADEBASKET");
        AddReward(1.0f);
        gc.outOfBounds();
        Done();
        return;
    }
}
