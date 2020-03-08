using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBallShooterPlayer : MonoBehaviour
{
    public float speed = 600;
    public float agentSpeed = 5;
    public GameObject ball;
    Rigidbody ballRgd;
    public bool hasBall = false;
    public int right = 1;
    public Transform basket;
    public gameController gc;
    public float timer = 0;

    void Start()
    {
        GameObject environment = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        ballRgd = environment.transform.GetChild(1).GetComponent<Rigidbody>();
        basket = environment.transform.GetChild(right == 1 ? 3:4).GetChild(4);
        //transform.localPosition = new Vector3(Random.Range(11f, 13f), 1f, Random.Range(-5f, 5f));
    }

    public void jump()
    {
        if(transform.position.y<=1)
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 440, 0));
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (hasBall)
        {
            ball.transform.localPosition = new Vector3(transform.localPosition.x + right * 1.1f, transform.localPosition.y + 0.75f, transform.localPosition.z);
            ballRgd.angularVelocity = Vector3.zero;
            ballRgd.velocity = Vector3.zero;
        }

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
        if (Input.GetKey(KeyCode.S) && hasBall)
        {
            gameObject.GetComponent<shoot_nn_script>().shoot();
        }
    }

    public void madeBasket()
    {
        GetComponent<shoot_nn_script>().basketMade();
        return;
    }
   
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("shot"))
        {
            if (gc.PlayerWithBall == null)
            {
                timer = 1;
                gc.PlayerWithBall = gameObject;
                hasBall = true;
            }
            else if (!gc.PlayerWithBall.Equals(gameObject) && gc.PlayerWithBall.GetComponent<BasketBallShooterPlayer>().timer <= 0)
            {                
                gc.PlayerWithBall.GetComponent<BasketBallShooterPlayer>().hasBall = false;
                timer = 1;
                gc.PlayerWithBall = gameObject;
                hasBall = true;
            }
        }
    }
}
