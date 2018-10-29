using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{

    public GameObject controller;
    public GameObject player;
    public GameObject mouseHole;
    public bool hole = false;
    // Use this for initialization
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Controller");
        player = GameObject.FindGameObjectWithTag("Player");
        mouseHole = GameObject.Find("mousehole");
    }

    // Update is called once per frame
    void Update()
    {
        if (hole)
        {
            print("Why do you do this? This is the update method");
            Teleport();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (controller.tag == "Controller")
        {
            print("Why do you do this");
                hole = true;

        }
    }
    void Teleport ()
    {
        player.transform.localPosition = new Vector3(16f, 1.32f, 24.6f);
        print("Collided");
        mouseHole.GetComponent<Transporter>().enabled = false;
    }
}