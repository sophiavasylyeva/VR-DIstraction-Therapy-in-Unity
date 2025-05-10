using UnityEngine;

public class Penguin : MonoBehaviour
{
    public ScoreManager scoreManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Snowball"))
        {
            scoreManager.AddScore(1);  // increment score
        }
    }
}
