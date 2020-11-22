using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class EnemyDirectionView : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] RollerSpawner _rollerSpawner;
    [SerializeField] Image _icon;

    private List<Image> _icons = new List<Image>();

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _icons.Add(_icon);
    }

    private Vector2 GetInRectPosition(Vector3 position)
    {
        if (position.z < 0)
        {
            position.y = 1 - position.y;
            position.x = 1 - position.x;
        }
        if (position.x >= 0 && position.x <= 1)
            position.y = 0;

        return new Vector2(
            Mathf.Min(Mathf.Max(position.x, 0.1f), 0.9f) * _rectTransform.rect.width,
            Mathf.Min(Mathf.Max(position.y, 0.1f), 0.9f) * _rectTransform.rect.height
        );
    }

    private void Update()
    {
        List<Vector3> positions = _rollerSpawner.RollersInCameraPositions;
        for (int i = 0; i < positions.Count; i++)
        {
            if (i > _icons.Count - 1)
                _icons.Add(Instantiate(_icon, transform));

            _icons[i].rectTransform.position = GetInRectPosition(positions[i]);
            _icons[i].enabled = true;
        }

        for (int i = positions.Count; i < _icons.Count; i++)
            _icons[i].enabled = false;
    }
}
