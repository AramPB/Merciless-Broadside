using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform followTransform;
    public Transform cameraTransform;
    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;
    //TODO: isLocked funcional
    private bool isLocked;

    public Vector3 newPosition;
    public Vector3 savedTransform;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    [SerializeField]
    private float zoomInMax = 20,
        zoomOutMax = 300,
        mapSizeMaxX = 500,
        mapSizeMaxZ = 500,
        mapLimiterHeigh = 100;

    private bool isAlt = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (followTransform != null)
        {
            isLocked = true;
            transform.position = followTransform.position;
            savedTransform = followTransform.position;
        }
        else
        {
            if (isLocked)
            {
                newPosition = savedTransform;
                transform.position.Set(savedTransform.x, savedTransform.y, savedTransform.z);
            }
            isLocked = false;
            followTransform = null;
        }

        HandleAlterInput();
        HandleMouseInput();
        HandleMovementInput();
        
        //if (Input.GetKeyDown(KeyCode.Escape))
        if (Input.GetButtonDown(Constants.ESCAPE))
        {
            if (isLocked)
            {
                newPosition = savedTransform;
                transform.position.Set(savedTransform.x, savedTransform.y, savedTransform.z);
            }
            isLocked = false;
            followTransform = null;
        }
    }

    void HandleAlterInput()
    {
        if (Input.GetButtonDown(Constants.ALTERNATE))
        {
            isAlt = true;
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetButtonUp(Constants.ALTERNATE))
        {
            isAlt = false;
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
    }

    void HandleMouseInput()
    {

        //ZOOM
        //if(Input.mouseScrollDelta.y != 0 && CanZoom(Input.mouseScrollDelta.y))
        if (Input.GetAxis(Constants.ZOOM_INPUT) != 0 && CanZoom(Input.GetAxis(Constants.ZOOM_INPUT)))
        {
            newZoom += Input.GetAxis(Constants.ZOOM_INPUT) * zoomAmount;
            //newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        //if (Input.GetMouseButtonDown(1))
        if(Input.GetButtonDown(Constants.CAMERA_POS) && !isAlt)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        //MOVEMENT
        //if (Input.GetMouseButton(1) && !isLocked)
        if (Input.GetButton(Constants.CAMERA_POS) && !isLocked && !isAlt)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                Vector3 auxPos;
                auxPos = transform.position + dragStartPosition - dragCurrentPosition;
                if (CanMove(auxPos)) {
                    newPosition = auxPos;
                }
            }
        }
        //ROTATE
        //if (Input.GetMouseButtonDown(2))
        if (Input.GetButtonDown(Constants.CAMERA_POS) && isAlt)
        {
            rotateStartPosition = Input.mousePosition;
        }
        //if (Input.GetMouseButton(2))
        if (Input.GetButton(Constants.CAMERA_POS) && isAlt)
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            
            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    void HandleMovementInput()
    {
        //TMP
        //SPEED
        if (Input.GetButton(Constants.SPEED))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }
        //MOVEMENT
        if (!isLocked)
        {
            /*
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            */
            /*
             Vector3 auxPos = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                auxPos += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                auxPos += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                auxPos += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                auxPos += (transform.right * -movementSpeed);
            }
            if (CanMove(newPosition + auxPos)) {
                newPosition += auxPos;
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
             */
            Vector3 auxPos = new Vector3(0, 0, 0);

            if (Input.GetButton(Constants.KEY_W))
            {
                auxPos += (transform.forward * movementSpeed);
            }
            if (Input.GetButton(Constants.KEY_S))
            {
                auxPos += (transform.forward * -movementSpeed);
            }
            if (Input.GetButton(Constants.KEY_D))
            {
                auxPos += (transform.right * movementSpeed);
            }
            if (Input.GetButton(Constants.KEY_A))
            {
                auxPos += (transform.right * -movementSpeed);
            }
            if (CanMove(newPosition + auxPos)) {
                newPosition += auxPos;
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);

        }
        //ROTATION
        /*if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }*/
        if (Input.GetButton(Constants.ROTATE_L))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetButton(Constants.ROTATE_R))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        //ZOOM
        /*if (Input.GetKey(KeyCode.R) && CanZoom(1))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F) && CanZoom(-1))
        {
            newZoom -= zoomAmount;
        }*/
        if (Input.GetButton(Constants.ZOOM_IN) && CanZoom(1))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetButton(Constants.ZOOM_OUT) && CanZoom(-1))
        {
            newZoom -= zoomAmount;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }


    private bool CanZoom(float mouse)
    {
        if ((newZoom.y < zoomOutMax && mouse < 0) || (mouse > 0 && newZoom.y > zoomInMax))
        {
            return true;
        }
        return false;
    }

    private bool CanMove(Vector3 pos)
    {
        if (pos.x >= mapSizeMaxX/2 || pos.x <= -mapSizeMaxX/2)
        {
            return false;
        }
        if (pos.z >= mapSizeMaxZ/2 || pos.z <= -mapSizeMaxZ/2)
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(mapSizeMaxX, mapLimiterHeigh, mapSizeMaxZ));
    }
}
