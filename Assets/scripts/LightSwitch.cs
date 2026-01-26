using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public Light light;
    public InputActionReference action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //light = GetComponent<Light>();
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            if (light.color==Color.red)
            {
                light.color = Color.white;
            }
            else
            {
                light.color = Color.red;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
