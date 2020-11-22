using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _stonePrefabs;
    [SerializeField] private GameField _field;
    [SerializeField] private int _count = 20;

    private void Start()
    {
        Rect fieldRect = new Rect(_field.Rect.min.x - _field.Rect.width/2, _field.Rect.min.y, _field.Rect.width * 2, _field.Rect.height * 2);
        for (int i = 0; i < _count; i++)
        {
            Vector3 position = new Vector3(Random.Range(fieldRect.min.x, fieldRect.max.x), 0, Random.Range(fieldRect.min.y, fieldRect.max.y));
            position = _field.GetPositionOnTerrain(position);
            GameObject stone = Instantiate(_stonePrefabs[Random.Range(0, _stonePrefabs.Length)]);
            stone.transform.position = position;
            stone.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, Random.Range(0, 180)));

            float scale = Random.Range(2.0f, 4.0f);
            stone.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
