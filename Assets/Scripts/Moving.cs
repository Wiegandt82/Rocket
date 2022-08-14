using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in the editor
    // CACHE - e.g. references for readibility or speed
    // STATE - private instance (member) variables

    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotateThrust = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainThrustParticle;
    [SerializeField] ParticleSystem ThrustParticleLeft;
    [SerializeField] ParticleSystem ThrustParticleRight;

    Rigidbody rb;
    AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainThrust);
            
            if(!audioSource.isPlaying) //to make sure we only play if we aren't already playing
            {
                audioSource.PlayOneShot(mainEngine);
            }

            if (!mainThrustParticle.isPlaying)
            {
                mainThrustParticle.Play();
            }
            
        }
        else
        {
            audioSource.Stop(); //stop our SFX when we aren't thrusting
            mainThrustParticle.Stop();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!ThrustParticleRight.isPlaying)
            {
                ThrustParticleRight.Play();
            }

            ApplyRotation(rotateThrust);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!ThrustParticleLeft.isPlaying)
            {
                ThrustParticleLeft.Play();
            }
            
            ApplyRotation(-rotateThrust);
        }
        else
        {
            ThrustParticleRight.Stop();
            ThrustParticleLeft.Stop();
        }
    }

    void ApplyRotation(float roataionThisFrame)
    {
        rb.freezeRotation = true; //freezing rotation so we can manually roatate
        transform.Rotate(Vector3.forward * Time.deltaTime * roataionThisFrame);
        rb.freezeRotation = false; //unfreezing rotation so phisic system can take over
    }


}
