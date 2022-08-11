using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotateThrust = 100f;

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
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop(); //stop our SFX when we aren't thrusting
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(rotateThrust);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(-rotateThrust);
        }
    }

    void ApplyRotation(float roataionThisFrame)
    {
        rb.freezeRotation = true; //freezing rotation so we can manually roatate
        transform.Rotate(Vector3.forward * Time.deltaTime * roataionThisFrame);
        rb.freezeRotation = false; //unfreezing rotation so phisic system can take over
    }


}
