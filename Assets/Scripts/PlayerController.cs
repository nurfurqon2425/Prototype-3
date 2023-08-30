using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    private MoveLeft moveLeftScript;
    private SpawnManager spawnManagerScript;

    public float jumpForce;
    public float gravityModifier;
    public float playerPositionX;
    private int score;

    public bool isOnGround;
    public bool isSingleJump;
    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        moveLeftScript = FindObjectOfType<MoveLeft>();
        spawnManagerScript = FindObjectOfType<SpawnManager>();
        Physics.gravity *= gravityModifier;
        playerRb.transform.position = new Vector3(-8, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.transform.position.x < 0)
        {
            playerRb.transform.Translate(Vector3.forward * Time.deltaTime * 5);
            playerPositionX = playerRb.transform.position.x;
        }
        else if (playerRb.transform.position.x >= 0)
        {
            playerAnim.SetFloat("Speed_f", 1);
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !isSingleJump && !gameOver)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                isSingleJump = true;
                playerAnim.SetTrigger("Jump_trig");
                dirtParticle.Stop();
                playerAudio.PlayOneShot(jumpSound, 1.0f);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && isSingleJump && !gameOver)
            {
                playerRb.AddForce(Vector3.up * jumpForce / 2, ForceMode.Impulse);
                isSingleJump = false;
                playerAnim.SetTrigger("Jump_trig");
                dirtParticle.Stop();
                playerAudio.PlayOneShot(jumpSound, 1.0f);
            }
            else
            {
                playerRb.AddForce(Vector3.down * 2, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.LeftShift) && !gameOver)
            {
                moveLeftScript.speedModifier = 2;
                playerAnim.speed = 3.0f;
                score += 2;
            }
            else if (!gameOver)
            {
                moveLeftScript.speedModifier = 1;
                playerAnim.speed = 1.5f;
                score += 1;
            }
        }

        Debug.Log("Score: " + score);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            isSingleJump = false;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }    
    }
}
