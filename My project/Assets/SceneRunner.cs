using UnityEngine;

public class SceneRunner : MonoBehaviour
{
    public GameObject Player;
    private Camera _camera;
    public float moveSpeed = 30.0f;
    //public float rotationSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindObjectsOfType<Camera>()[0];
    }

    private float momentumBuild;

    private float verticalVelocity = 0f;
    public float gravity = 9.81f;

    public bool wasAPressed = true;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float inputPower = Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput);

        if (Input.GetKey("g"))
        {
            if (wasAPressed == false)
            {
                verticalVelocity = 10f;
            }
            wasAPressed = true;
        }
        else
            wasAPressed = false;

        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;

        const float MOMENTUM_POWER_INCREASE = 1.15f;
        float momentumPower = 0;


        if (inputPower > 0.75f)
        {
            momentumBuild = Mathf.Clamp(momentumBuild * 1.1f + (5f * Time.deltaTime), 0f, 1f);
            momentumPower = EaseInOut(momentumBuild);
        }
        else
        {
            momentumBuild = 0;
        }
        verticalVelocity -= gravity * Time.deltaTime;

        // Ensure the vectors are parallel to the ground
        cameraForward.y = 0;
        cameraRight.y = 0;
        Vector3 moveDirection = cameraRight.normalized * horizontalInput + cameraForward.normalized * verticalInput;
        Player.transform.Translate((moveDirection * moveSpeed * Time.deltaTime) + (moveDirection * moveSpeed * momentumPower * MOMENTUM_POWER_INCREASE * Time.deltaTime), Space.World);
        Player.transform.position = new Vector3(Player.transform.position.x, Mathf.Clamp(Player.transform.position.y + verticalVelocity * Time.deltaTime, 0f, 1000000f), Player.transform.position.z);



        const float PLAYER_ROTATION_SCALE = 12.5f;
        float targetAngle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
        if (inputPower > 0.125f)
        {
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, targetRotation, PLAYER_ROTATION_SCALE * Time.deltaTime);

        }

    }

    float EaseInOut(float t)
    {
        return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }

    //public float rotationSpeed = 5.0f;

    //void Update()
    //{
    //    float horizontalInput = Input.GetAxis("Horizontal");
    //    float verticalInput = Input.GetAxis("Vertical");

    //    // Calculate the angle in radians based on input values
    //    float targetAngle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;

    //    // Create a rotation based on the calculated angle
    //    Quaternion targetRotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);

    //    // Smoothly rotate the player's transform towards the target rotation
    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

}
