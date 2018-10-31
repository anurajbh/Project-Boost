using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[DisallowMultipleComponent]
public class RocketOscillator : MonoBehaviour
{
    const float tau = Mathf.PI * 2;
    [SerializeField] Vector3 movementPlatform = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    Vector3 startingPos;
    Vector3 offset;
    [Range(0,1)] [SerializeField] float movementFactor;
	// Use this for initialization
	void Start ()
    {
        startingPos = transform.position;
         
	}
	
	// Update is called once per frame
	void Update ()
    {
        Oscillate();
    }

    private void Oscillate()
    {
        float cycles = Time.time / period;
        float sineWave = Mathf.Sin(cycles * tau);
        movementFactor = sineWave / 4f + 0.5f;
        offset = movementFactor * movementPlatform;
        transform.position = startingPos + offset;
    }
}
