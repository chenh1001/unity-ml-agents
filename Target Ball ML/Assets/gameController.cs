using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PlayerWithBall;
    public Transform ball;
    Rigidbody ballRgd;
    public shoot_nn_script lastShooter;

    void Start()
    {
        PlayerWithBall = null;
        lastShooter = null;
        GameObject environment = gameObject.transform.parent.gameObject;
        ball.transform.localPosition = new Vector3(0, 5, 0);
        ballRgd = environment.transform.GetChild(1).GetComponent<Rigidbody>();
        ballRgd.angularVelocity = Vector3.zero;
        ballRgd.velocity = Vector3.zero;
    }

    public void outOfBounds()
    {
        ball.transform.localPosition = new Vector3(0, 5, 0);
        ballRgd.angularVelocity = Vector3.zero;
        ballRgd.velocity = Vector3.zero;
    }

    public void basketMade()
    {
        Debug.Log("BALL CALLED MADEBASKET");
        outOfBounds();
        //if(lastShooter!=null)
        //    lastShooter.Done();
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.transform.localPosition.z > 10 || ball.transform.localPosition.z < -10)
        {
            outOfBounds();
        }
        if (ball.transform.localPosition.y >= 16)
        {
            outOfBounds();
        }
        if (ball.transform.localPosition.x > 23 || ball.transform.localPosition.x < -23)
        {
            outOfBounds();
        }
    }
}
