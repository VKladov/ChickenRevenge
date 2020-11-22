using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class IKFabric : MonoBehaviour
{
    [SerializeField] private int _chainLength;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _pole;
    [SerializeField] private int _iterations = 10;
    [SerializeField] private float _delta = 0.01f;

    protected float[] BonesLength;
    protected float CompleteLength;
    protected Transform[] Bones;
    protected Vector3[] Positions;
    protected Vector3[] StartDirectionSucc;
    protected Quaternion[] StartRotationBone;
    protected Quaternion StartRotationTarget;
    protected Quaternion StartRotationRoot;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Bones = new Transform[_chainLength + 1];
        Positions = new Vector3[_chainLength + 1];
        BonesLength = new float[_chainLength];
        StartDirectionSucc = new Vector3[_chainLength + 1];
        StartRotationBone = new Quaternion[_chainLength + 1];

        CompleteLength = 0;
        var current = transform;
        for (int i = Bones.Length - 1; i >= 0; i--)
        {
            Bones[i] = current;
            StartRotationBone[i] = current.rotation;

            if (i == Bones.Length - 1)
            {
                StartDirectionSucc[i] = _target.position - current.position;
            }
            else
            {
                StartDirectionSucc[i] = Bones[i + 1].position - current.position;
                BonesLength[i] = (Bones[i + 1].position - current.position).magnitude;
                CompleteLength += BonesLength[i];
            }
            current = current.parent;
        }
    }

    private void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        if (_target == null)
            return;

        if (BonesLength.Length != _chainLength)
            Init();

        for (int i = 0; i < Bones.Length; i++)
            Positions[i] = Bones[i].position;

        if ((_target.position - Bones[0].position).sqrMagnitude >= CompleteLength * CompleteLength)
        {
            Vector3 direction = (_target.position - Positions[0]).normalized;
            for (int i = 1; i < Positions.Length; i++)
                Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
        }
        else
        {
            for (int iteration = 0; iteration < _iterations; iteration++)
            {
                for (int i = Positions.Length - 1; i > 0; i--)
                {
                    if (i == Positions.Length - 1)
                        Positions[i] = _target.position;
                    else
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i];
                }

                for (int i = 1; i < Positions.Length; i++)
                    Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i - 1];

                if ((Positions[Positions.Length - 1] - _target.position).sqrMagnitude < _delta * _delta)
                    break;
            }
        }

        if (_pole != null)
        {
            for (int i = 1; i < Positions.Length - 1; i++)
            {
                Plane plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(_pole.position);
                var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1], plane.normal);
                Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];
            }
        }

        for (int i = 0; i < Positions.Length; i++)
        {
            if (i == Positions.Length - 1)
                Bones[i].rotation = _target.rotation * Quaternion.Inverse(StartRotationTarget) * StartRotationBone[i];
            else
                Bones[i].rotation = Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) * StartRotationBone[i];
            Bones[i].position = Positions[i];
        }
    }

    private void OnDrawGizmos()
    {
        var current = transform;
        for (int i = 0; i < _chainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
}
