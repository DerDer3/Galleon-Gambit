using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Panning/Scrolling functionality for the map camera.</summary>
[RequireComponent(typeof(Camera))]
public class MapCamera : MonoBehaviour
{
    /// <summary>The speed at which scrolling moves the camera.</summary>
    [SerializeField] private float ScrollSpeed = 15f;
    /// <summary>The minimum y position the camera may not surpass.</summary>
    public float YMin;
    /// <summary>The maximum y position the camera may not surpass.</summary>
    public float YMax;

    private Camera cam;
    private Vector3 dragStart;
    private bool isDragging;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Mouse.current == null) return;
        UpdateDrag();
        UpdateScroll();
        ClampPosition();
    }

    /// <summary>Ensure the y-position lands within the `YMin` and `YMax` range.</summary>
    private void ClampPosition()
    {
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, YMin, YMax),
            transform.position.z
        );
    }

    /// <summary>Updates map panning.</summary>
    private void UpdateDrag()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            dragStart = MouseWorldPosition();
            isDragging = true;
        }
        else if (Mouse.current.rightButton.isPressed && isDragging)
        {
            Vector3 diff = dragStart - MouseWorldPosition();
            transform.position += new Vector3(0f, diff.y, 0f);
        }
        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }
    }

    /// <summary>Updates map scrolling.</summary>
    private void UpdateScroll()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            transform.position += Vector3.up * (scroll * ScrollSpeed * Time.deltaTime);
        }
    }

    /// <summary>Returns the mouse position in world coordinates.</summary>
    private Vector3 MouseWorldPosition()
    {
        var pos = Mouse.current.position.ReadValue();
        return cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cam.nearClipPlane));
    }
}
