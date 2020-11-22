using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _borderLinePrefab;
    [SerializeField] private Collider _borderWallColliderPrefab;
    [SerializeField] private ObstacleConfig[] _obstacles;

    private Collider _frontWall, _backWall, _leftWall, _rightWall;
    private SpriteRenderer _frontBorder, _backBorder, _leftBorder, _rightBorder;

    private void Awake()
    {
        _frontWall = Instantiate(_borderWallColliderPrefab);
        _backWall = Instantiate(_borderWallColliderPrefab);
        _leftWall = Instantiate(_borderWallColliderPrefab);
        _rightWall = Instantiate(_borderWallColliderPrefab);
        _frontBorder = Instantiate(_borderLinePrefab);
        _backBorder = Instantiate(_borderLinePrefab);
        _leftBorder = Instantiate(_borderLinePrefab);
        _rightBorder = Instantiate(_borderLinePrefab);
    }

    public void SpawnObstacles(ObstacleSpawner spawner)
    {
        foreach (var obstacleConfig in _obstacles)
            spawner.SpawnObstacle(obstacleConfig.Prefab, obstacleConfig.Row, obstacleConfig.Col, 1, 1);
    }

    public void UpdatePlayerArea(GameField field)
    {
        UpdateWallsTransform(field);
        UpdatePlayerAreaLines(field);
    }

    private void UpdateWallsTransform(GameField field)
    {
        _frontWall.transform.position = new Vector3(field.PlayerRect.center.x, 0, field.PlayerRect.max.y);
        _frontWall.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        _frontWall.transform.localScale = new Vector3(field.PlayerRect.width / 10, 1, 1);

        _backWall.transform.position = new Vector3(field.PlayerRect.center.x, 0, field.PlayerRect.min.y);
        _backWall.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        _backWall.transform.localScale = new Vector3(field.PlayerRect.width / 10, 1, 1);

        _leftWall.transform.position = new Vector3(field.PlayerRect.min.x, 0, field.PlayerRect.center.y);
        _leftWall.transform.rotation = Quaternion.Euler(new Vector3(90, 90, 0));
        _leftWall.transform.localScale = new Vector3(field.PlayerRect.height / 10, 1, 1);

        _rightWall.transform.position = new Vector3(field.PlayerRect.max.x, 0, field.PlayerRect.center.y);
        _rightWall.transform.rotation = Quaternion.Euler(new Vector3(90, -90, 0));
        _rightWall.transform.localScale = new Vector3(field.PlayerRect.height / 10, 1, 1);
    }

    private void UpdatePlayerAreaLines(GameField field)
    {
        _frontBorder.transform.position = new Vector3(field.PlayerRect.center.x, 0.01f, field.PlayerRect.max.y);
        _frontBorder.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        _frontBorder.size = new Vector2(field.PlayerRect.width, _frontBorder.size.y);

        _backBorder.transform.position = new Vector3(field.PlayerRect.center.x, 0.01f, field.PlayerRect.min.y);
        _backBorder.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        _backBorder.size = new Vector2(field.PlayerRect.width, _backBorder.size.y);

        _leftBorder.transform.position = new Vector3(field.PlayerRect.min.x, 0.01f, field.PlayerRect.center.y);
        _leftBorder.transform.rotation = Quaternion.Euler(new Vector3(90, 90, 0));
        _leftBorder.size = new Vector2(field.PlayerRect.height, _leftBorder.size.y);

        _rightBorder.transform.position = new Vector3(field.PlayerRect.max.x, 0.01f, field.PlayerRect.center.y);
        _rightBorder.transform.rotation = Quaternion.Euler(new Vector3(90, 90, 0));
        _rightBorder.size = new Vector2(field.PlayerRect.height, _rightBorder.size.y);
    }
}
