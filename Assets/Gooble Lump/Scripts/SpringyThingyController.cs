using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringyThingyController : MonoBehaviour
{
    //all the parts that make up the player
    [Header("-- Parts --")]
    [SerializeField, Tooltip("The Top Half of the ball")]
    private Rigidbody2D halfA;
    [SerializeField, Tooltip("The Bottom Half of the ball")]
    private Rigidbody2D halfB;
    [SerializeField, Tooltip("The Gameobject that indicates the direction of the ball")]
    private GameObject directionIndicator;
    [SerializeField, Tooltip("The linerenderer that will show when the wing is activated")]
    private LineRenderer wingLine;

    [Header("-- Physics Settings --")]
    [SerializeField, Tooltip("The distance between the two halves when extended")]
    private float extendedLength = -2.5f;
    [SerializeField, Tooltip("The force in the joint between the two halves.")]
    private float springForce = 1f;
    [SerializeField, Tooltip("Scales the amount of torque applied to the player")]
    private float torqueMultiplier = 7.5f;
    [SerializeField, Tooltip("How much the extended wing is affected by aerodynamics")]
    float AerodynamicAffect = 0.75f;
    
    //the joint connecting the two halves
    [SerializeField, HideInInspector]
    private RelativeJoint2D joint;
    //the default distance between halves
    [SerializeField, HideInInspector]
    private float defaultJointLength;

    /// <summary>
    /// determines whether or not the direction indicator is active
    /// </summary>
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

    /// <summary>
    /// this will return the position in the middle of both halves of the ball
    /// </summary>
    public Vector2 AveragePosition
    {
        get => Vector2.Lerp(halfA.position, halfB.position, 0.5f);
        set { }
    }

    /// <summary>
    /// returns the average z rotation of both halves
    /// </summary>
    public float AverageZRotation
    {
        get => Vector2.SignedAngle(Vector3.up, halfA.position - halfB.position);
        set { }
    }

    /// <summary>
    /// returns the direction from the back half to the front half
    /// </summary>
    public Vector2 averageForwardDirection
    {
        get => (halfA.position - halfB.position).normalized;
    }

    private bool extended;
    /// <summary>
    /// this controls whether or not the ball is extended
    /// </summary>
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

    /// <summary>
    /// returns whether or not the ball is currently extended
    /// </summary>
    public bool isExtended
    {
        get => extended;
    }

    // how much torque is currently being applied
    float currentTorque;
    #region Torque Stuff
    /// <summary>
    /// appies a force to each side in opposite directions to rotate the player
    /// </summary>
    void AddTorqueToBothHalves(float torqueToAdd)
    {
        Vector2 forceDirection = Vector2.Perpendicular(averageForwardDirection);
        halfA.AddForce(forceDirection * torqueToAdd);
        halfB.AddForce(forceDirection * -torqueToAdd);
    }

    /// <summary>
    /// returns the torque required to rotate the player so that they are facing a target.
    /// </summary>
    private float RotateTowardsTarget(Vector2 target)
    {
        Vector2 directionToTarget = (target - AveragePosition).normalized;
        Vector2 forwardDirection = averageForwardDirection;
        //this is used to calculate whether to turn left or right
        Vector2 leftDirection = Vector2.Perpendicular(forwardDirection);
        float angularVelocity = Mathf.Lerp(halfA.angularVelocity, halfB.angularVelocity, 0.5f);
        float turnDirection = Mathf.Sign((Vector2.Dot(forwardDirection, directionToTarget) + 1) * 0.5f * Vector2.Dot(leftDirection, directionToTarget));
        float dotProductToTarget = Mathf.Abs(Vector2.Dot(leftDirection, directionToTarget));
        //the -angularVelocity here stops the player from overshooting and swinging back and fourth
        float torqueToAdd = Mathf.Lerp(-angularVelocity * 0.0025f, turnDirection, dotProductToTarget) * torqueMultiplier;
        return torqueToAdd;
    }

    /// <summary>
    /// applies currentTorque and lerps it down to 0
    /// </summary>
    void ApplyTorque()
    {
        AddTorqueToBothHalves(currentTorque * 15);
        //currentTorque decays back down to zero.
        currentTorque = Mathf.Lerp(currentTorque, 0, 25f);
    }
    #endregion

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

    /// <summary>
    /// updates the Physics based on the mouse controls
    /// </summary>
    private void MouseControlsUpdate()
    {
        Extended = Input.GetMouseButton(0);

        Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentTorque = RotateTowardsTarget(mousePositionInWorld);
        if (!extended)
            IndicateDirection = true;
        else
            IndicateDirection = false;
    }

    /// <summary>
    /// Updates the Wing lineRenderer
    /// </summary>
    private void UpdateWingLineRenderer()
    {
        if (wingLine)
        {
            if (Extended)
            {
                wingLine.enabled = true;
                wingLine.SetPosition(0, halfA.position);
                wingLine.SetPosition(1, halfB.position);
            }
            else
            {
                wingLine.enabled = false;
            }
        }
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
        MouseControlsUpdate();
        UpdateWingLineRenderer();
    }

    private void FixedUpdate()
    {
        ApplyAerodynamics();
        ApplyTorque();
    }
}