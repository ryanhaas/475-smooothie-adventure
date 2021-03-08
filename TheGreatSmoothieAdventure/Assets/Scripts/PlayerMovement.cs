using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public GameObject mainCam;
    public Vector3 cameraOffset;
    public float speed = 2;
    public float jumpAmount = 5;
    public int levelNum = 0;
    public RuntimeAnimatorController idleAnim;
    public RuntimeAnimatorController jumpAnim;
    public RuntimeAnimatorController runAnim;

    Rigidbody2D rb;
    Animator animator;

    private bool canJump;
    private bool gotFruit;

    private bool incTime;
    private float time;
    private float highscore;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        canJump = false;
        gotFruit = false;
        highscore = PlayerPrefs.GetFloat("level" + levelNum + "score");

        // Variable to start only if the player moved
        incTime = false;
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(incTime)
            time += Time.deltaTime;

        // Let the player jump
        if(canJump && Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpAmount);
            if(!incTime && time == 0.0f)
                incTime = true;
        }

        // Player horizontal movement
        float xMove = 0.0f;
        if(Input.GetKey(KeyCode.A)) {
            xMove = -speed;
        }
        else if(Input.GetKey(KeyCode.D)) {
            xMove = speed;
        }

        // Animation control
        // Honestly it's disgusting
        animator.speed = 1.0f;
        if(rb.velocity.y != 0.0f) {
            animator.speed = 0.0f;
            animator.runtimeAnimatorController = jumpAnim;
        }
        else if(xMove != 0.0f) {
            animator.runtimeAnimatorController = runAnim;
            Vector3 scaleTemp = transform.localScale;
            if(xMove > 0.0f)
                scaleTemp.x = Mathf.Abs(scaleTemp.x);
            else
                scaleTemp.x = -Mathf.Abs(scaleTemp.x);
            transform.localScale = scaleTemp;
        }
        else {
            animator.runtimeAnimatorController = idleAnim;
        }
        
        // Move player
        transform.Translate(xMove * Time.deltaTime, 0.0f, 0.0f);
        mainCam.transform.position = new Vector3 (transform.position.x + cameraOffset.x, transform.position.y + cameraOffset.y, mainCam.transform.position.z);

        if(!incTime && time == 0.0f && xMove != 0.0f) {
            incTime = true;
        }

        // If user presses escape key, return to menu
        if(Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("Menu");
    }

    void OnTriggerEnter2D (Collider2D other) {
        if(other.tag == "Platform") {
            if(rb.velocity.y == 0.0f)
                canJump = true;
        }
        if(other.tag == "Fruit") {
            gotFruit = true;
            incTime = false;
            Destroy(other.gameObject);
            saveData();

            // Load next level after 3 seconds
            Invoke("loadNext", 3);
        }
        if(other.tag == "Ant") {
            // Check to see if player is on top of ant (check if bottom of player is above ant)
            float bottomX = transform.position.y - GetComponent<Collider2D>().bounds.size.y - rb.velocity.y;
            if(bottomX >= other.transform.position.y) {
                Destroy(other.gameObject);
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void OnTriggerStay2D (Collider2D other) {
        if(other.tag == "Platform") {
            if(rb.velocity.y == 0.0f)
                canJump = true;
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        if(other.tag == "Platform")
            canJump = false;
    }

    void OnGUI() {
        float roundedTime = (float)System.Math.Round(time, 2);
        if(gotFruit) {
            Rect box = new Rect(Screen.width/2 - 50, Screen.height/2 - 20, 250, 40);
            GUI.Box(box, "You got the ingredient in " + roundedTime + " seconds!");
            if(time < highscore) {
                Rect highBox = new Rect(Screen.width/2 - 50, Screen.height/2 + 20, 250, 40);
                GUI.Box(highBox, "New Highscore!");
            }
        }

        // Draw time count
        Rect timeBox = new Rect(10, 10, 120, 25);
        GUI.Box(timeBox, "Time: " + roundedTime);

        // Draw highscore
        Rect highscoreBox = new Rect(10, 35, 120, 25);
        GUI.Box(highscoreBox, "Highschore: " + highscore);
    }

    void saveData() {
        float roundedTime = (float)System.Math.Round(time, 2);
        // Check to see if current time is higher than saved time, OR
        // if highscore is 0, then the level has not been played so save the score
        if(roundedTime < highscore || highscore == 0.0f) {
            PlayerPrefs.SetFloat("level" + levelNum + "score", roundedTime);
        }
    }

    void loadNext() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
