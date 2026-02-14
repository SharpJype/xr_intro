using UnityEngine;

public class GameObjectDuplicator : MonoBehaviour
{

    public GameObject prefab;
    public int objectArrayWidth = 10;
    public int objectArrayHeight = 1;
    public int objectArrayDepth = 10;
    public float objectXStep = 1.0f;
    public float objectYStep = 1.0f;
    public float objectZStep = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 newPosition = prefab.transform.position;
        for (int i=0; i<objectArrayWidth;i++)
        {
            for (int j=0; j<objectArrayHeight;j++)
            {
                for (int h=0; h<objectArrayDepth;h++)
                {
                    GameObject newObj = Instantiate(prefab, newPosition, prefab.transform.rotation);
                    newObj.transform.parent = this.transform;
                    newPosition.z += objectZStep;
                }
                newPosition.y += objectYStep;
                newPosition.z = prefab.transform.position.z;
            }
            newPosition.x += objectXStep;
            newPosition.y = prefab.transform.position.y;
        }
        
        
    }
}
