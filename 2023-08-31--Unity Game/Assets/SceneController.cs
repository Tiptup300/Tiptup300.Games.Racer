using System;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject PlayerGameObject;

    private int _cameraIndex;
    private Camera[] _cameras;
    private Vector3 _cameraOffset;

    private bool foundCamera => _cameras.Length > 0;
    private Camera _camera => _cameras[_cameraIndex];


    // Start is called before the first frame update
    void Start()
    {

        // get cameras
        _cameras = FindObjectsOfType<Camera>();

        _cameraOffset = _camera.transform.position - PlayerGameObject.transform.position;

        // create player
        //PlayerGameObject = Instantiate(PlayerGameObject);

    }

    // Update is called once per frame
    void Update()
    {
        //  _camera.transform.LookAt(PlayerGameObject.transform);

        // Calculate the direction from the camera to the target object
        Vector3 directionToTarget = PlayerGameObject.transform.position - _camera.transform.position;

        // Rotate the camera to face the target object
        _camera.transform.rotation = Quaternion.LookRotation(directionToTarget); 
        Vector3 desiredPosition = PlayerGameObject.transform.position + _cameraOffset;

        // Use Lerp to smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(_camera.transform.position, desiredPosition, 0.5f * Time.deltaTime);

        // Apply the new position to the camera
        _camera.transform.position = smoothedPosition;

       // PlayerGameObject.transform.position += new Vector3(0f,0f, 0.025f);


        updateGamepad();

    }

    private void updateGamepad()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Keyboard (A/D or Left/Right arrow)
        float verticalInput = Input.GetAxis("Vertical");     // Keyboard (W/S or Up/Down arrow)

        // Combine keyboard and gamepad inputs
        float finalHorizontalInput= horizontalInput;
        float finalVerticalInput =  verticalInput;

        // Calculate the movement vector
        Vector3 movement = new Vector3(finalHorizontalInput, 0f, finalVerticalInput) * 50f * Time.deltaTime;

        // Apply the movement to the Rigidbody
        PlayerGameObject.transform.position += movement;
    }
}
