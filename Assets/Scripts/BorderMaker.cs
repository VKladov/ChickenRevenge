using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderMaker : MonoBehaviour
{
    [SerializeField] private GameField _field;
    [SerializeField] private GameObject _borderPrefab;

    private void Awake()
    {
        CreatePlane(new Vector3(_field.PlayerRect.center.x, 0, _field.PlayerRect.max.y), Quaternion.Euler(0, -90, 90), _field.PlayerRect.width);
        CreatePlane(new Vector3(_field.PlayerRect.center.x, 0, _field.PlayerRect.min.y), Quaternion.Euler(0, 90, 90), _field.PlayerRect.width);
        CreatePlane(new Vector3(_field.PlayerRect.min.x, 0, _field.PlayerRect.center.y), Quaternion.Euler(0, 180, 90), _field.PlayerRect.height);
        CreatePlane(new Vector3(_field.PlayerRect.max.x, 0, _field.PlayerRect.center.y), Quaternion.Euler(0, 0, 90), _field.PlayerRect.height);
    }

    private void CreatePlane(Vector3 position, Quaternion rotation, float size)
    {
        GameObject border = Instantiate(_borderPrefab, position, Quaternion.identity);
        if (border.TryGetComponent(out Collider collider))
        {
            float frontBorderScale = size / (collider.bounds.extents.x * 2);
            border.transform.localScale = new Vector3(1, 1, frontBorderScale);
            border.transform.rotation = rotation;
        }
    }
}
