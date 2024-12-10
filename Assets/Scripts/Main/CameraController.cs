using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pan Settings")]
    public float basePanSpeed = 10f;
    public Vector2 panLimit = new Vector2(50f, 50f);

    [Header("Scroll Settings")]
    public float scrollSpeed = 10f;
    public float minHeight = 10f;
    public float maxHeight = 50f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f;
    public float minRotationX = 60f;
    public float maxRotationX = 90f;
    public float minRotationY = -90f;
    public float maxRotationY = 90f;
    public float defaultRotationX = 80f;
    public float defaultRotationY = 0f;

    private Vector3 _dragOrigin;
    private bool _isDragging;
    private float _currentRotationX;
    private float _currentRotationY;

    void Start()
    {
        transform.rotation = Quaternion.Euler(defaultRotationX, defaultRotationY, 0f);
        _currentRotationX = defaultRotationX;
        _currentRotationY = defaultRotationY;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        
        float currentSpeed = basePanSpeed * Mathf.InverseLerp(minHeight, maxHeight, pos.y) ;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            pos.z += currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            pos.z -= currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            pos.x -= currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            pos.x += currentSpeed * Time.deltaTime;
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed;
        pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
        
        if (Input.GetMouseButtonDown(2))
        {
            _dragOrigin = Input.mousePosition;
            _isDragging = true;
        }

        if (Input.GetMouseButton(2) && _isDragging)
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
            pos.x -= difference.x * currentSpeed;
            pos.z -= difference.y * currentSpeed;
            _dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(2))
            _isDragging = false;
        
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            _currentRotationX -= rotationX;
            _currentRotationX = Mathf.Clamp(_currentRotationX, minRotationX, maxRotationX);
            transform.rotation = Quaternion.Euler(_currentRotationX, _currentRotationY, 0f);
        }
        
        if (Input.GetMouseButton(1))
        {
            float rotationY = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            _currentRotationY += rotationY;
            _currentRotationY = Mathf.Clamp(_currentRotationY, minRotationY, maxRotationY);
            transform.rotation = Quaternion.Euler(_currentRotationX, _currentRotationY, 0f);
        }
        
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
