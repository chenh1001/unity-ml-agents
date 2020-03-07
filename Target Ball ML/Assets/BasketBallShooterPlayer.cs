using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class BasketBallShooterPlayer : Agent
{
    public float speed = 600;
    public float agentSpeed = 5;
    public GameObject ball;
    Rigidbody ballRgd;
    public int counter = 2;
    public bool hasBall = true;
    public int right = 1;
    public Transform basket;

    void Start()
    {
        GameObject environment = gameObject.transform.parent.gameObject;
        ballRgd = environment.transform.GetChild(2).GetComponent<Rigidbody>();
        basket = environment.transform.GetChild(right == 1 ? 3:4).GetChild(4);

        ballRgd.useGravity = false;
        ball.transform.localPosition = new Vector3(transform.localPosition.x + 1f, transform.localPosition.y + 0.75f, transform.localPosition.z);
        transform.localPosition = new Vector3(Random.Range(11f, 13f), 1f, Random.Range(-5f, 5f));
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

    public void jump()
    {
        if(transform.position.y<=1)
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 440, 0));
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

    public override void AgentReset()
    {
        /*hasBall = true;
        GameObject environment = gameObject.transform.parent.gameObject;
        //transform.localPosition = new Vector3(Random.Range(3f, 18f), 1f, Random.Range(-9f, 9f));
        ballRgd.angularVelocity = Vector3.zero;
        ballRgd.velocity = Vector3.zero;
        ball.transform.localPosition = new Vector3(transform.localPosition.x + right * 1.1f, transform.localPosition.y + 0.75f, transform.localPosition.z);
        ballRgd.useGravity = false;*/
    }

    void FixedUpdate()
    {
        /*if(hasBall)
            ball.transform.localPosition = new Vector3(transform.localPosition.x + right * 1.1f, transform.localPosition.y + 0.75f, transform.localPosition.z);
        if (counter <= 1)
        {
            RequestDecision();
        }*/
        /*if (ball.transform.localPosition.z > 10 || ball.transform.localPosition.z < -10)
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
        }*/

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * agentSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * agentSpeed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * agentSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * agentSpeed);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            jump();
        }
        if(Input.GetKey(KeyCode.S))
        {
            //hasBall = false;
            GetComponent<shoot_nn_script>().shoot();
            //counter = 0;
        }
    }

    public void madeBasket()
    {
        GetComponent<shoot_nn_script>().basketMade();
        return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
