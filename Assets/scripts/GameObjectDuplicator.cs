using UnityEngine;

public class GameObjectDuplicator : MonoBehaviour
{

    public GameObject prefab;

    public Vector3Int arrayShape;
    public Vector3 arrayStep;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (arrayShape.x==0) arrayShape.x = 1;
        if (arrayShape.y==0) arrayShape.y = 1;
        if (arrayShape.z==0) arrayShape.z = 1;
        Vector3 newPosition = prefab.transform.position;
        for (int i=0; i<arrayShape.x;i++)
        {
            for (int j=0; j<arrayShape.y;j++)
            {
                for (int h=0; h<arrayShape.z;h++)
                {
                    if ((i+j+h)>0)
                    {
                        GameObject newObj = Instantiate(prefab, newPosition, prefab.transform.rotation);
                        newObj.transform.parent = this.transform;
                    }
                    newPosition.z += arrayStep.z;
                }
                newPosition.y += arrayStep.y;
                newPosition.z = prefab.transform.position.z;
            }
            newPosition.x += arrayStep.x;
            newPosition.y = prefab.transform.position.y;
        }
        
        
    }
}
