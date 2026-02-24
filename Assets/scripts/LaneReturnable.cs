using UnityEngine;

public class LaneReturnable : MonoBehaviour
{
    private Vector3 returnPosition;
    public bool targetedByGutter = false;

    void Start()
    {
        returnPosition = transform.position;
    }

    public Vector3 GetReturnPosition()
    {
        return returnPosition;
    }
}
