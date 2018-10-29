using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{

    //Game Config
    int scene;
    enum State { Alive, Dead, Transcending }
    Rigidbody rb;
    AudioSource aud;

    // Game initialization
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] State state = State.Alive;

    void Start()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {

            case "Friendly":
                print("OK");
                break;
            case "Finish":
                print("Congrats");
                state = State.Transcending;
                Invoke("LoadTheScene", 1f);
                break;
            default:
                print("Dead");
                state = State.Dead;
                Invoke("LoadTheScene", 2f);
                break;
        }
    }
    void LoadTheScene()
    {
        if(state == State.Dead)
        {
            SceneManager.LoadScene(scene);
        }
        else if(state == State.Transcending)
        {
            SceneManager.LoadScene(scene+1);
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
