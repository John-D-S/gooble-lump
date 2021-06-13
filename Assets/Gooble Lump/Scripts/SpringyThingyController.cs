using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlScheme
{
    ArrowKeys,
    Mouse
}

public class SpringyThingyController : MonoBehaviour
{
    [Header("-- Parts --")]
    [SerializeField]
    private Rigidbody2D halfA;
    [SerializeField]
    private Rigidbody2D halfB;
    [SerializeField]
    private GameObject directionIndicator;

    [Header("-- Physics Settings --")]
    [SerializeField]
    private float extendedLength = -2.5f;
    [SerializeField]
    private float springForce = 1f;
    [SerializeField]
    private float MaxTorque = 1f;
    [SerializeField]
    float AerodynamicAffect = 0.75f;

    [SerializeField, HideInInspector]
    private RelativeJoint2D joint;
    [SerializeField, HideInInspector]
    private float defaultJointLength;

    [SerializeField]
    private ControlScheme controlScheme = ControlScheme.ArrowKeys;

    private bool IndicateDirection
    {
        get
        {
            if (directionIndicator)
                return directionIndicator.activeSelf;
            return false;
        }
        set
        {
            if (directionIndicator)
                directionIndicator.SetActive(value);
        }
    }

    //this will return the position in the middle of both halves of the ball
    public Vector2 AveragePosition
    {
        get => Vector2.Lerp(halfA.position, halfB.position, 0.5f);
        set { }
    }

    public float AverageZRotation
    {
        get => Mathf.Lerp(halfA.transform.rotation.eulerAngles.z, halfB.transform.rotation.eulerAngles.z, 0.5f);
        set { }
    }

    public Vector2 averageForwardDirection
    {
        get => (halfA.position - halfB.position).normalized;
    }

    private bool extended;
    //this controls whether the ball is extended
    private bool Extended
    {
        get => extended;
        set
        {
            //setting the linear offset of the joint causes the halves of the ball to spring apart/ together.
            joint.linearOffset = value ? Vector2.up * extendedLength : Vector2.up * defaultJointLength;
            extended = value;
        }
    }
    float currentTorque;

    void ApplyAerodynamics()
    {
        if (Extended)
        {
            Vector2 averageVelocity = Vector2.Lerp(halfA.velocity, halfB.velocity, 0.5f);
            Vector2 directionPerpandicularToWing = Vector2.Perpendicular((halfB.position - halfA.position).normalized);
            //this is the angle between the average velocity of the wing and the direction perpandicular to it.
            float angle = Vector2.Angle(averageVelocity, directionPerpandicularToWing);
            float forceToApply = -averageVelocity.magnitude * Mathf.Cos(Mathf.Deg2Rad * angle) * AerodynamicAffect;
            halfA.AddForce(forceToApply * directionPerpandicularToWing);
            halfB.AddForce(forceToApply * directionPerpandicularToWing);
            Debug.DrawLine(AveragePosition, AveragePosition + averageVelocity, Color.green);
            Debug.DrawLine(AveragePosition, AveragePosition + forceToApply * directionPerpandicularToWing, Color.red);
        }
    }

    void AddTorqueToBothHalves(float torqueToAdd)
    {
        halfA.AddTorque(torqueToAdd);
        halfB.AddTorque(torqueToAdd);
    }

    private float RotateTowardsTarget(Vector2 target)
    {
        Vector2 directionToTarget = (target - AveragePosition).normalized;
        Vector2 forwardDirection = averageForwardDirection;
        //if the thing rotates to be away from the mouse, change this to rightdirection and add a negative beforet the v2.perp
        Vector2 leftDirection = Vector2.Perpendicular(forwardDirection);
        float angularVelocity = Mathf.Lerp(halfA.angularVelocity, halfB.angularVelocity, 0.5f);
        float turnDirection = Mathf.Sign(Vector2.Dot(forwardDirection, directionToTarget) * Vector2.Dot(leftDirection, directionToTarget));
        float dotProductToTarget = Mathf.Abs(Vector2.Dot(leftDirection, directionToTarget));
        float torqueToAdd = Mathf.Lerp(-angularVelocity * 0.005f, /*dotProductToTarget */ turnDirection, dotProductToTarget) * MaxTorque;
        Debug.Log(torqueToAdd);
        return torqueToAdd;

        //float torqueToAdd = Vector2.Dot(-rb.transform.right, (Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.transform.position).normalized) * rotationSpeed;
        //rb.AddTorque(Vector2.Dot(-rb.transform.right, (Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.transform.position).normalized) * rotationSpeed);
    }

    void ApplyTorque()
    {
        float torqueToAdd = currentTorque * 0.5f * Vector2.Distance(halfA.position, halfB.position);
        AddTorqueToBothHalves(torqueToAdd);
        //currentTorque decays back down to zero.
        currentTorque = Mathf.Lerp(currentTorque, 0, 25f);
    }

    private void ArrowKeysControlsUpdate()
    {
        //jumping
        Extended = Input.GetAxisRaw("Vertical") == 1;

        //torqueing
        currentTorque -= Input.GetAxisRaw("Horizontal") * MaxTorque;
    }

    private void MouseControlsUpdate()
    {
        if (Input.GetMouseButton(1))
        {

            Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AddTorqueToBothHalves(RotateTowardsTarget(mousePositionInWorld));
        }

        Extended = Input.GetMouseButton(0);
    }

    private void OnValidate()
    {
        if (halfA && halfB)
        {
            RelativeJoint2D halfAHasJoint = halfA.GetComponent<RelativeJoint2D>();
            if (halfAHasJoint)
                joint = halfAHasJoint;
            else
                joint = halfB.GetComponent<RelativeJoint2D>();
        }

        if (joint)
        {
            defaultJointLength = joint.linearOffset.y;
            joint.maxForce = springForce;
        }
    }

    private void Awake()
    {
        StaticObjectHolder.player = this;
    }

    private void Update()
    {
        switch (controlScheme)
        {
            case ControlScheme.ArrowKeys:
                ArrowKeysControlsUpdate();
                break;
            case ControlScheme.Mouse:
                MouseControlsUpdate();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        ApplyAerodynamics();
        ApplyTorque();
    }
}