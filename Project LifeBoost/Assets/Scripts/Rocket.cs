using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource aud;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up);
            if (!aud.isPlaying)
            {
                aud.Play();
            }
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            if (aud.isPlaying)
            {
                aud.Stop();
            }
        }
    }
    private void Rotate()
    {
        rb.freezeRotation = true; //taking control of rotation
        float rcsThrust = 10f;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.D)))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.A)))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rb.freezeRotation = false; //resuming physics control of rotation
    }

}
