using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ronHalohemAgent : Agent
{
    public int rotationSpeed = 100;
    public int speed = 10;
    public int shotSpeed = 2000;
    public GameObject shot;
    public GameObject academy;
    float cooldown = 1f;
    public float shotCD = 0;
    private RayPerception3D rayPer;
    //public GameObject target;
    public int health = 5;
    public int powerUpNum = 1;
    float powerCooldown = 10;
    public float powerCD = 0;
    float speedTimer = -1;
    int isPowerOn = 0;
    int speedBoost = 10;
    int startingSpeed = 10;
    int startingRotSpeed = 100;
    public override void InitializeAgent()
    {
        rayPer = GetComponent<RayPerception3D>();
        academy = GameObject.Find("Academy");
    }

    public override void CollectObservations()
    {
        float rayDistance = 100f;
        //float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        float[] rayAngles = { 10f,20f,30f,40f,50f,60f,70f,80f,90f,100f,110f,120f,130f,140f,150f,160f,170f,180f,190f,200f,210f,220f,230f,240f,250f,260f,270f,280f,290f,300f,310f,320f,330f,340f,350f,360f};

        string[] detectableObjects = { "wall", "Player", "shot","target"};
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        int hasShot = 0;
        if (shotCD <= 0)
            hasShot = 1;
        AddVectorObs(hasShot);
        int hasPowerUp = 0;
        if (powerCD <= 0)
            hasPowerUp = 1;
        AddVectorObs(hasPowerUp);
        AddVectorObs(powerUpNum);
        AddVectorObs(isPowerOn);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        int rotate = 0;
        bool shoot = false;
        bool usePower = false;
        if (vectorAction[0] == 0)
        {
            rotate = 0;
        }
        else if (vectorAction[0] == 1)
        {
            rotate = -1;
        }
        else if (vectorAction[0] == 2)
        {
            rotate = 1;
        }
        transform.Rotate(new Vector3(0, rotate, 0) * Time.deltaTime * rotationSpeed);
        if (vectorAction[1] == 1)
        {
            shoot = true;
        }
        if (shoot&&shotCD<=0)
        {
            agentShoot();
        }

        if (vectorAction[2] == 1)
        {
            usePower = true;
        }
        if (usePower && powerCD <= 0)
        {
            agentUsePower();
        }
    }

    public override void AgentReset()
    {
        //gameObject.SetActive(true);
        health = 5;
        isPowerOn = 0;
        speed = startingSpeed;
        rotationSpeed = startingRotSpeed;
        rayPer = GetComponent<RayPerception3D>();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.localPosition = new Vector3(Random.Range(-40f,40f),1,Random.Range(-26f,26f));
        //target.transform.localPosition = new Vector3(Random.Range(-40f, 40f), 0.5f, Random.Range(-26f, 26f));
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
    }
    void Update()
    {
        if(speedTimer<=0&& isPowerOn==1)
        {
            speed = startingSpeed;
            rotationSpeed = startingRotSpeed;
            isPowerOn = 0;
        }
        //AddReward(-0.01f);
        powerCD -= Time.deltaTime;
        shotCD -= Time.deltaTime;
        speedTimer -= Time.deltaTime;
    }

    public void agentUsePower()
    {
        if(powerUpNum==1)
        {
            agentCastReverse();
        }
        if(powerUpNum==2)
        {
            agentCastSpeed();
        }
        powerCD = powerCooldown;
    }
    public void agentCastSpeed()
    {
        isPowerOn = 1;
        speed += speedBoost;
        rotationSpeed += speedBoost;
        speedTimer = 2;
    }
    public void agentCastReverse()
    {
        GameObject enemy = null;
        for (int i = 0; i < academy.GetComponent<RollerAcademy>().players.Length; i++)
        {
            enemy=academy.GetComponent<RollerAcademy>().players[i];
            if(gameObject!=enemy)
            {
                enemy.transform.Rotate(new Vector3(0,180,0));
            }
        }
    }

    void agentShoot()
    {
        AddReward(-0.2f);
        GameObject shotFired=Instantiate(shot, gameObject.transform.GetChild(0).transform.GetChild(0).position,transform.rotation);
        shotFired.transform.Rotate(new Vector3(90, 0, 0));
        shotFired.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
        shotFired.GetComponent<Rigidbody>().AddForce(transform.forward * shotSpeed);
        shotCD = cooldown;
        shotFired.GetComponent<shot>().shooter = gameObject;
        //Destroy(shotFired, 10f);
    }
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "wall")
        {
            Vector3 rot = transform.rotation.eulerAngles;
            Vector3 rotWall = col.gameObject.transform.rotation.eulerAngles;
            float wallRealRot = Mathf.Abs(rotWall.y);
            float retRotation = 360 - 2 * wallRealRot - 2 * rot.y;
            transform.Rotate(new Vector3(0, retRotation, 0));
            health--;
            //AddReward(-0.3f);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag=="wall")
        {
            Vector3 rot = transform.rotation.eulerAngles;
            Vector3 rotWall = col.gameObject.transform.rotation.eulerAngles;
            float wallRealRot = Mathf.Abs(rotWall.y);
            float retRotation = 360 - 2 * wallRealRot - 2 * rot.y;
            transform.Rotate(new Vector3(0,retRotation,0));
            health--;
            AddReward(-0.3f);
        }
        if (col.gameObject.tag == "shot")
        {
            if (col.gameObject.GetComponent<shot>().shooter!= gameObject)
            {
                health--;
                AddReward(-0.5f);
                return;
            }
        }
    }
}
