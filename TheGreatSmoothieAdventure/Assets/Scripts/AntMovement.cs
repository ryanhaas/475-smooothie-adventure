using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{
    public float speed = 1.0f;

    private bool onPlatform;
    private bool turnedAround = false;

    void Start()
    {
        onPlatform = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(onPlatform) {
            transform.Translate(-speed * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Platform")
            onPlatform = true;
        if(other.tag == "AntWall" && !turnedAround) {
            speed = -speed;
            turnedAround = true;
            Vector3 scaleTemp = transform.localScale;
            scaleTemp.x = -scaleTemp.x;
            transform.localScale = scaleTemp;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Platform")
            onPlatform = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Platform")
            onPlatform = false;
        if(other.tag == "AntWall") {
            speed = -speed;
            turnedAround = false;
        }
    }
}
