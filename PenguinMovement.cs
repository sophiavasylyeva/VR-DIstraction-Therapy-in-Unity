using System.Collections;
using UnityEngine;

public class PenguinMovement : MonoBehaviour
{
    public float speed = 2f;              // Not needed for forward motion anymore, but you can use it to control zigzag speed
    public float zigzagFrequency = 1f;    // Frequency of zigzag movement
    public float zigzagAmplitude = 2f;    // Amplitude of the zigzag movement

    private Vector3 startPosition;
    private float timeElapsed;

    void Start()
    {
        startPosition = transform.position;
        timeElapsed = 0f;
    }

    void Update()
    {
        MoveInZigzagInPlace();
    }

    void MoveInZigzagInPlace()
    {
        // Update time
        timeElapsed += Time.deltaTime;

        // Calculate horizontal (X) offset using sine wave
        float offsetX = Mathf.Sin(timeElapsed * zigzagFrequency) * zigzagAmplitude;

        // Set position: only modify the X axis, keep Y and Z the same as start
        transform.position = new Vector3(startPosition.x + offsetX, startPosition.y, startPosition.z);
    }

    // Detect collision with a snowball (or projectile)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snowball"))
        {
            DestroyPenguin();
        }
    }

    void DestroyPenguin()
    {
        // Play destruction effects here
        Debug.Log("Penguin hit by snowball!");
        Destroy(gameObject);
    }
}
