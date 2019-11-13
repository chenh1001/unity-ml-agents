using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainScript : MonoBehaviour
{
    public GameObject shot;
    float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-13f, 13f));
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime; 
        if(time<=0)
        {
            summonHazard();
            time = 1f;
        }
    }

    void summonHazard()
    {
        transform.position = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-13f, 13f));
        transform.Rotate(new Vector3(0,Random.Range(-180,180),0));
        GameObject shotFired = Instantiate(shot, gameObject.transform.position, transform.rotation);
        shotFired.transform.Rotate(new Vector3(90, 0, 0));
        shotFired.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
        shotFired.GetComponent<shot>().shooter = gameObject;
    }
}
