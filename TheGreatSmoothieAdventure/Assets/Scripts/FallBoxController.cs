using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallBoxController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        // If the player hits the fall box, restart the level
        if(other.name == "Player") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // If ant hits the fall box, destroy the ant
        if(other.tag == "Ant") {
            Destroy(other.gameObject);
        }
    }
}
