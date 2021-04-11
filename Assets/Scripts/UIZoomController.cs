using UnityEngine;
using UnityEngine.EventSystems;

public class UIZoomController : MonoBehaviour, IScrollHandler
{
    private Vector3 initScale;

    [SerializeField] 
    private float zoomSpeed = .1f;
    [SerializeField] 
    private float maxZoom = 10f;


    private void Start()
    {
        initScale = transform.localScale;
    }

    public void OnScroll(PointerEventData eventData)
    {
        Vector3 delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        Vector3 desiredScale = transform.localScale + delta;

        desiredScale = ClampDesiredScale(desiredScale);

        transform.localScale = desiredScale;
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initScale, desiredScale);
        desiredScale = Vector3.Min(initScale * maxZoom, desiredScale);
        return desiredScale;
    }
}
