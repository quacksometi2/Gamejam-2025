using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource footstepsSource;
    public AudioSource effectsSource;

    public AudioClip walkLoop;
    public AudioClip runLoop;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip wallRunLoop;   // <-- ny lyd til wall run

    private CharacterController controller;
    private bool wasGrounded;
    private bool hasJumped;

    // Denne bool kan du sætte fra dit wall?running script
    public bool isWallRunning;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        wasGrounded = controller.isGrounded;
        hasJumped = false;
    }

    void Update()
    {
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        // Gå/Løb/Wall?run lyd
        if (isWallRunning)
        {
            if (!footstepsSource.isPlaying || footstepsSource.clip != wallRunLoop)
            {
                footstepsSource.loop = true;
                footstepsSource.clip = wallRunLoop;
                footstepsSource.Play();
            }
        }
        else if (controller.isGrounded && isMoving)
        {
            AudioClip desiredClip = Input.GetKey(KeyCode.LeftShift) ? runLoop : walkLoop;

            if (!footstepsSource.isPlaying)
            {
                footstepsSource.loop = true;
                footstepsSource.clip = desiredClip;
                footstepsSource.Play();
            }
            else if (footstepsSource.clip != desiredClip)
            {
                footstepsSource.clip = desiredClip;
                footstepsSource.Play();
            }
        }
        else if (footstepsSource.isPlaying)
        {
            footstepsSource.Stop();
        }

        // Hop lyd
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            PlayJump();
            hasJumped = true;
        }

        // Landing lyd
        if (hasJumped && !wasGrounded && controller.isGrounded)
        {
            PlayLanding();
            hasJumped = false;
        }

        wasGrounded = controller.isGrounded;
    }

    void PlayJump()
    {

        print("PlayJump called");
        if (jumpSound != null && effectsSource != null)
        {
            effectsSource.PlayOneShot(jumpSound);
            print("Jump sound played");
        }
    }

    void PlayLanding()
    {
        if (landSound != null && effectsSource != null)
        {
            effectsSource.PlayOneShot(landSound);
        }
    }
}
    