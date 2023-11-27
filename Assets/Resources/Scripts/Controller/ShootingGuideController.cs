using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGuideController : MonoBehaviour
{
    public GameObject bulletItem;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletItem == null)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}
