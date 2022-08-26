using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;
    public Vector3 OffsetPosition;
    public Vector3 OffsetRotation;

    public void Poke()
    {
        transform.rotation = Quaternion.Euler(OffsetRotation.x, OffsetRotation.y, OffsetRotation.z);
    }

    private void Update()
    {
        if (Target == null)
            return;

        transform.position = Target.position + OffsetPosition;
    }
}