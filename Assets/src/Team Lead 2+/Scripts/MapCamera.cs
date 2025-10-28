using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Panning/Scrolling functionality for the map camera.</summary>
[RequireComponent(typeof(Camera))]
public class MapCamera : MonoBehaviour
{
    public float ScrollSpeed = 15f;
    public float YMin;
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

    private void ClampPosition()
    {
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, YMin, YMax),
            transform.position.z
        );
    }

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

    private void UpdateScroll()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            transform.position += Vector3.up * (scroll * ScrollSpeed * Time.deltaTime);
        }
    }

    private Vector3 MouseWorldPosition()
    {
        var pos = Mouse.current.position.ReadValue();
        return cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cam.nearClipPlane));
    }
}
