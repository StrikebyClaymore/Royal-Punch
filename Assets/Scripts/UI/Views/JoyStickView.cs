using UnityEngine;

public class JoyStickView : UIView
{
    [SerializeField] private RectTransform _outer;
    [SerializeField] private RectTransform _inner;
    private float _outerRadius;
    private float _innerRadius;

    private void Awake()
    {
        _outerRadius = _outer.rect.width * _outer.localScale.x / 2;
        _innerRadius = _inner.rect.width * _inner.localScale.x / 2;
    }

    public void SetControllerPosition(Vector3 touchPosition)
    {
        Show();
        _outer.localPosition = CenteringPosition(touchPosition);
    }

    public Vector3 UpdateInnerPosition(Vector3 touchPosition)
    {
        touchPosition = CenteringPosition(touchPosition);
        var center = _outer.localPosition;
        var direction = (touchPosition - center).normalized;
        var dist = Vector3.Distance(touchPosition, center);

        if (dist <= _outerRadius)
            _inner.localPosition = touchPosition;
        else
            _inner.localPosition = center + direction * Mathf.Min(_outerRadius, dist);

        var start = new Vector3(center.x, 0, center.y);
        var end = new Vector3(touchPosition.x, 0, touchPosition.y);
        direction = (end - start).normalized;

        return direction;
    }

    private Vector3 CenteringPosition(Vector3 position)
    {
        return new Vector3(position.x - Screen.width / 2f,position.y - Screen.height / 2f, 0);
    }
}
