using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    //Audio
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    //Particles
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    AudioSource audioSource;

    bool isTransitioning = false; //to apply SFX twice
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        DebugKeys();
    }
    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;       //it will toggle collision
            
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) { return; }
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("This is Friendly");
                    break;
                case "Finish":
                    StartSuccessSequence();
                    break;
                case "Fuel":
                    Debug.Log("Fuel");
                    break;
                default:
                    StartCrashSequence();
                    break;
            }
        
    }
    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();                     //stop all sounds 
        successParticles.Play();
        audioSource.PlayOneShot(success);
        GetComponent<Moving>().enabled = false; //stop moving
        Invoke("LoadNextLevel", levelLoadDelay);// Invoke with delay
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(death);
        GetComponent<Moving>().enabled = false; //turn off moving
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void ReloadLevel() //Resets level if failed
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel() //Loads next level
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
