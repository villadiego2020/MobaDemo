using UnityEngine;

public class SpawnTransform : MonoBehaviour
{
    [SerializeField] private Vector3 _OffsetPosition;
    [SerializeField] private Vector3 _OffsetRotation;

    public Vector3 Position
    {
        get
        {
            return _OffsetPosition;
        }
    }

    public Vector3 Rotation
    {
        get
        {
            return _OffsetRotation;
        }
    }
}