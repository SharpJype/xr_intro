using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{

    public InputActionReference pointAction;
    public InputActionReference gripAction;
    public InputActionReference grabAction;
    public float animationSpeed = 1.0f;

    private float gripTarget;
    private float grabTarget;
    private float pointTarget;


    Animator animator;
    private float gripCurrent;
    private float grabCurrent;
    private float pointCurrent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        grabTarget = grabAction.action.ReadValue<float>();
        pointTarget = pointAction.action.ReadValue<float>();
        gripTarget = gripAction.action.ReadValue<float>();
        AnimateHand();
    }


    void AnimateHand()
    {
        if (gripTarget != gripCurrent)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat("Grip", gripCurrent);
        }
        if (grabTarget != grabCurrent)
        {
            grabCurrent = Mathf.MoveTowards(grabCurrent, grabTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat("Grab", grabCurrent);
        }
        if (pointTarget != pointCurrent)
        {
            pointCurrent = Mathf.MoveTowards(pointCurrent, pointTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat("Point", pointCurrent);
        }
    }
}
