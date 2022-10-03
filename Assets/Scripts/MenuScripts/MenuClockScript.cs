using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClockScript : MonoBehaviour
{

    public GameObject littleHand;
    float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        int sign = Random.Range(0, 2);
        if (sign == 0)
        {
            sign = -1;
        }
        rotationSpeed = Random.Range(100, 360) * sign;

        transform.rotation = Quaternion.Euler(0, 0,
            Random.Range(0, 360));
        littleHand.transform.rotation = Quaternion.Euler(0, 0,
            Random.Range(0, 360));
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0,
            transform.rotation.eulerAngles.z + Time.deltaTime * rotationSpeed);
        littleHand.transform.rotation = Quaternion.Euler(0, 0,
            littleHand.transform.rotation.eulerAngles.z + (Time.deltaTime * rotationSpeed)/10f);
    }
}
