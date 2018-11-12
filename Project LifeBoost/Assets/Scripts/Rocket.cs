using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{

    //Game Config
    Scene scene;
    int sceneIndex;
    enum State { Alive, Dead, Transcending }
    Rigidbody rb;
    AudioSource aud;
    AudioSource audThrust;
    
    // Game initialization
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] State state = State.Alive;

    [SerializeField] bool collisionsAreDisabled = false;
    
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip levelLoad;

    [SerializeField] ParticleSystem thrustEffect;
    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] ParticleSystem levelLoadEffect;
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;
        if (state == State.Alive)
        {
            RespondToThrust();
            Rotate();
            
        }
        if(Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive || collisionsAreDisabled)
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
        Invoke("LoadTheScene", levelLoadDelay);
    }

    private void LoadTheNextLevel()
    {
        state = State.Transcending;
        aud.Stop();
        thrustEffect.Stop();
        aud.PlayOneShot(levelLoad);
        levelLoadEffect.Play();
        Invoke("LoadTheScene", levelLoadDelay);
    }

    void LoadTheScene()
    {
        if(state == State.Dead)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else if(state == State.Transcending)
        {
            int nextScene = sceneIndex + 1;
            if (nextScene == SceneManager.sceneCountInBuildSettings)//check if last scene
            {
                nextScene = 0;
            }
            SceneManager.LoadScene(nextScene);
        }
        
    }

    private void RespondToThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust);
            ApplyThrust();
        }
        else if(Input.GetKey(KeyCode.L))
        {
            state = State.Transcending;
        }
        
        else
        {
            thrustEffect.Stop();
            aud.Stop();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsAreDisabled = !collisionsAreDisabled;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadTheNextLevel();
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
