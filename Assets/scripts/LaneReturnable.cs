using UnityEngine;

public class LaneReturnable : MonoBehaviour
{
    private Vector3 returnPosition;

    void Start()
    {
        returnPosition = transform.position;
    }

    public Vector3 GetReturnPosition()
    {
        return returnPosition;
    }
}
