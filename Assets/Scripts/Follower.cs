using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;

    private void Update()
    {
        if (Target == null)
            return;

        transform.position = Target.position + Offset;
    }
}