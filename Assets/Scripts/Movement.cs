using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] AudioClip MainEngineSfx;
    [SerializeField] ParticleSystem MainEngineParticle;
    [SerializeField] ParticleSystem LeftEngineParticle;
    [SerializeField] ParticleSystem RightEngineParticle;

    Rigidbody rb;
    AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        Thrust();
        Rotation();
    }

    private void Thrust()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(MainEngineSfx);
            }

            if (!MainEngineParticle.isPlaying)
            {
                MainEngineParticle.Play();
            }
           
            
        }
        else
        {
            audioSource.Stop();
            MainEngineParticle.Stop();
        }
    }

    private void Rotation()
    {
       float rotationInput =  rotation.ReadValue<float>();

        if(rotationInput < 0)
        {
            ApplyRotation(rotationStrength);


            if (!RightEngineParticle.isPlaying)
            {
                LeftEngineParticle.Stop();
                RightEngineParticle.Play();


            }
        }


        else if (rotationInput > 0)
        {
            ApplyRotation(-rotationStrength);

            if (!LeftEngineParticle.isPlaying)
            {
                RightEngineParticle.Stop();
                 LeftEngineParticle.Play();


            }

        }

        else
        {
            RightEngineParticle.Stop();
            LeftEngineParticle.Stop();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
