using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class rollerAgent : Agent
{
    public Transform target;
    public float speed = 10;
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void AgentReset()
    {
        if(this.transform.position.y<0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3(0, 3f, 0);
        }
        target.position = new Vector3(Random.value * 8 - 4, 2f, Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        AddVectorObs(target.position);
        AddVectorObs(this.transform.position);
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
        AddVectorObs(rBody.velocity.y);
    }

    public override void AgentAction(float[] vectorAction,string textAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        controlSignal.y = vectorAction[2];
        if (rBody.velocity.y != 0)
            controlSignal.y = 0;
        else if(controlSignal.y>0)
            controlSignal.y = 30;
        rBody.AddForce(controlSignal * speed);
        float distanceToTarget = Vector3.Distance(this.transform.position,target.position);
        if (this.transform.position.y < 0)
        {
            SetReward(-1.0f);
            Done();
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("target"))
        {
            SetReward(1.0f);
            Done();
        }
    }
}
