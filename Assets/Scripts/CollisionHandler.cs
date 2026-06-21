using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 1.5f;
    [SerializeField] AudioClip CrashSfx;
    [SerializeField] AudioClip SuccessSfx;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem CrashParticles;

    bool isControllrable = true;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isControllrable)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "friendly":
                Debug.Log("nice");
                break;
            case "finish":
                isControllrable = false;
                audioSource.Stop();
                audioSource.PlayOneShot(SuccessSfx);
                successParticles.Play();
                GetComponent<Movement>().enabled = false;
                Invoke("LoadNextLevel", levelLoadDelay);
                break;
            default:
                StartCrashSequence();
                Invoke("ReloadScene", 1.5f);
                break;

 
        }
    }

    private void StartCrashSequence()
    {
        isControllrable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(CrashSfx);
        CrashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadScene", levelLoadDelay); 
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextscene = currentScene + 1;

        if (nextscene == SceneManager.sceneCountInBuildSettings)
        {
            nextscene = 0;
        }

        SceneManager.LoadScene(nextscene);
    }

    void ReloadScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

   
}
