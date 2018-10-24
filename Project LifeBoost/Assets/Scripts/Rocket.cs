using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    Rigidbody rb;
    AudioSource aud;
    [SerializeField] float rcsThrust = 100f;
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
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {

            case "Friendly":
                print("OK");
                break;
            case "Fuel":
                print("Refueled");
                break;
            default:
                print("Dead");
                break;
        }
    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up *mainThrust);
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
