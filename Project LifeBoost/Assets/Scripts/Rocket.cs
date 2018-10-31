using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{

    //Game Config
    int scene;
    enum State { Alive, Dead, Transcending }
    Rigidbody rb;
    AudioSource aud;
    AudioSource audThrust;

    // Game initialization
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] State state = State.Alive;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip levelLoad;

    [SerializeField] ParticleSystem thrustEffect;
    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] ParticleSystem levelLoadEffect;
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
            RespondToThrust();
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
                break;
            case "Finish":
                LoadTheNextLevel();
                break;
            default:
                StartDeathEffect();
                break;
        }
    }

    private void StartDeathEffect()
    {
        state = State.Dead;
        aud.Stop();
        thrustEffect.Stop();
        aud.PlayOneShot(death);
        deathEffect.Play();
        Invoke("LoadTheScene", 2f);
    }

    private void LoadTheNextLevel()
    {
        state = State.Transcending;
        aud.Stop();
        thrustEffect.Stop();
        aud.PlayOneShot(levelLoad);
        levelLoadEffect.Play();
        Invoke("LoadTheScene", 1f);
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

    private void RespondToThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust);
            ApplyThrust();
        }
        else
        {
            aud.Stop();
        }
    }

    private void ApplyThrust()
    {
        if (!aud.isPlaying)
        {
            aud.PlayOneShot(mainEngine);
            
        }
        thrustEffect.Play();

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
