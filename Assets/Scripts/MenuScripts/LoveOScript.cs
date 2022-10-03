using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveOScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateRotation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdateRotation()
    {
        while (true)
        {
            transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z * -1);
            yield return new WaitForSeconds(Random.Range(.33f, .8f));
        }
    }

}
