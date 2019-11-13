using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour
{
    public GameObject shooter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject!=shooter&&col.gameObject.tag=="Player")
        {
            if (shooter.name != "trainObject")
            {
                shooter.GetComponent<ronHalohemAgent>().AddReward(0.6f);
            }
            Destroy(gameObject);
        }
        if(col.gameObject != shooter&&col.gameObject.name!="Plane")
        {
            if (col.gameObject.tag != "Player" && col.gameObject.tag != "shot")
            {
                //Debug.Log(col.gameObject);
                shooter.GetComponent<ronHalohemAgent>().AddReward(-0.2f);
            }
            Destroy(gameObject);
        }
    }

}
