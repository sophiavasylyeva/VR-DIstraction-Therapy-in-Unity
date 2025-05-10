using UnityEngine;

public class MouseSnowballThrow : MonoBehaviour
{
    private Rigidbody rb;
    private Camera mainCamera;
    private bool isGrabbed = false;
   
    // Distance from camera when grabbed
    private float objectZDistance;
   
    // Throw physics parameters
    public float throwForceMultiplier = 1.5f;
    public float maxThrowVelocity = 20f;
    public float dragSpeed = 10f;
   
    // Velocity tracking for throw calculation
    private Vector3 previousPosition;
    private Vector3 currentVelocity;
    private Vector3[] velocityFrames = new Vector3[5];
    private int frameIndex = 0;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
       
        // Initialize velocity tracking array
        for (int i = 0; i < velocityFrames.Length; i++)
        {
            velocityFrames[i] = Vector3.zero;
        }
    }
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to grab
        {
            TryGrab();
        }
        else if (Input.GetMouseButtonUp(0) && isGrabbed) // Release when mouse button is lifted
        {
            Release();
        }
       
        if (isGrabbed)
        {
            DragObject();
            TrackVelocity();
        }
    }
   
    void TryGrab()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
       
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                isGrabbed = true;
                rb.useGravity = false;
                rb.isKinematic = true; // Disable physics temporarily
               
                // Store the initial distance from the camera
                objectZDistance = Vector3.Distance(mainCamera.transform.position, transform.position);
               
                // Initialize position tracking for velocity calculation
                previousPosition = transform.position;
               
                // Reset velocity history
                for (int i = 0; i < velocityFrames.Length; i++)
                {
                    velocityFrames[i] = Vector3.zero;
                }
            }
        }
    }
   
    void DragObject()
    {
        Vector3 targetPosition = GetMouseWorldPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, dragSpeed * Time.deltaTime);
    }
   
    void TrackVelocity()
    {
        // Calculate and store velocity
        currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        velocityFrames[frameIndex] = currentVelocity;
        frameIndex = (frameIndex + 1) % velocityFrames.Length;
       
        // Update previous position for next frame
        previousPosition = transform.position;
    }
   
    Vector3 GetAverageVelocity()
    {
        Vector3 sum = Vector3.zero;
        foreach (Vector3 vel in velocityFrames)
        {
            sum += vel;
        }
        return sum / velocityFrames.Length;
    }
   
    void Release()
    {
        isGrabbed = false;
        rb.isKinematic = false;
        rb.useGravity = true;
       
        // Apply throw velocity based on mouse movement
        Vector3 throwVelocity = GetAverageVelocity() * throwForceMultiplier;
       
        // Clamp maximum velocity
        if (throwVelocity.magnitude > maxThrowVelocity)
        {
            throwVelocity = throwVelocity.normalized * maxThrowVelocity;
        }
       
        // Apply velocity to the snowball
        rb.velocity = throwVelocity;
       
        // Add a bit of random rotation for more natural throws
        rb.angularVelocity = new Vector3(
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f)
        );
    }
   
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = objectZDistance; // Keep the object at its original depth
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}