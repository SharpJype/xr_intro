using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public Light targetLight;
    public InputActionReference action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //light = GetComponent<Light>();
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            if (targetLight.color==Color.red)
            {
                targetLight.color = Color.white;
            }
            else
            {
                targetLight.color = Color.red;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
