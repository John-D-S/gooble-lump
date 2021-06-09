using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringyThingyController : MonoBehaviour
{
    [Header("-- Parts --")]
    [SerializeField]
    private Rigidbody2D halfA;
    [SerializeField]
    private Rigidbody2D halfB;

    [Header("-- Physics Settings --")]
    [SerializeField]
    private float extendedLength = -2.5f;
    [SerializeField]
    private float springForce = 1f;
    [SerializeField]
    private float spinningTorque = 1f;
    [SerializeField, Range(0, 1)]
    float AerodynamicAffect = 0.75f;

    [SerializeField, HideInInspector]
    private RelativeJoint2D joint;
    [SerializeField, HideInInspector]
    private float defaultJointLength;

    private Vector2 averagePosition;
    public Vector2 AveragePosition
    {
        get => Vector2.Lerp(halfA.position, halfB.position, 0.5f);
        set { }
    }

    private bool extended;
    private bool Extended
    {
        get => extended;
        set
        {
            joint.linearOffset = value ? Vector2.up * extendedLength : Vector2.up * defaultJointLength;
            extended = value;
        }
    }
    float currentTorque;

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

    private void Update()
    {
        //jumping
        Extended = Input.GetAxisRaw("Vertical") == 1;

        //torqueing
        currentTorque = 0;
        currentTorque -= Input.GetAxisRaw("Horizontal") * spinningTorque;
    }

    void ApplyTorque()
    {
        float torqueToAdd = currentTorque * 0.5f * Vector2.Distance(halfA.position, halfB.position);
        halfA.AddTorque(torqueToAdd);
        halfB.AddTorque(torqueToAdd);
    }

    private void FixedUpdate()
    {
        ApplyTorque();
    }
}