using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingCrawlScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float crawlSpeed = 20f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + crawlSpeed * Time.deltaTime, transform.position.z);
    }
}
