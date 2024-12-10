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
    public float scrollSmoothTime = 0.1f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f;
    public float minRotationX = 60f;
    public float maxRotationX = 90f;
    public float minRotationY = -90f;
    public float maxRotationY = 90f;
    public float defaultRotationX = 80f;
    public float defaultRotationY = 0f;

    [Header("Initial Settings")]
    public Vector3 initialPosition = new Vector3(0f, 20f, 0f);

    private Vector3 _dragOrigin;
    private bool _isDragging;
    private float _currentRotationX;
    private float _currentRotationY;

    private float _targetHeight;
    private float _heightVelocity;

    void Start()
    {
        // Встановлюємо початкову позицію камери
        transform.position = initialPosition;

        // Встановлюємо початкове обертання камери
        transform.rotation = Quaternion.Euler(defaultRotationX, defaultRotationY, 0f);
        _currentRotationX = defaultRotationX;
        _currentRotationY = defaultRotationY;
        _targetHeight = transform.position.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // Згладжений рух по висоті
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _targetHeight -= scroll * scrollSpeed;
        _targetHeight = Mathf.Clamp(_targetHeight, minHeight, maxHeight);
        pos.y = Mathf.SmoothDamp(pos.y, _targetHeight, ref _heightVelocity, scrollSmoothTime);

        // Рух камери в площині, враховуючи напрямок обертання
        float currentSpeed = basePanSpeed * Mathf.InverseLerp(minHeight, maxHeight, pos.y);
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            pos += forward * currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            pos -= forward * currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            pos -= right * currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            pos += right * currentSpeed * Time.deltaTime;

        // Перетягування камери
        if (Input.GetMouseButtonDown(2))
        {
            _dragOrigin = Input.mousePosition;
            _isDragging = true;
        }

        if (Input.GetMouseButton(2) && _isDragging)
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
            pos -= right * difference.x * currentSpeed;
            pos -= forward * difference.y * currentSpeed;
            _dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(2))
            _isDragging = false;

        // Ротація камери
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            _currentRotationX -= rotationX;
            _currentRotationX = Mathf.Clamp(_currentRotationX, minRotationX, maxRotationX);

            float rotationY = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            _currentRotationY += rotationY;
            _currentRotationY = Mathf.Clamp(_currentRotationY, minRotationY, maxRotationY);

            transform.rotation = Quaternion.Euler(_currentRotationX, _currentRotationY, 0f);
        }

        // Обмеження руху в межах панорами
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        // Застосування нової позиції
        transform.position = pos;
    }
}
