using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DiagonalLightingSprite : MonoBehaviour
{
    [Header("-- Lighting Sprites --")]
    [SerializeField, Tooltip("The top left lit sprite")]
    private Sprite TopLeftLitSprite;
    [SerializeField, Tooltip("The top right lit sprite")]
    private Sprite TopRightLitSprite;
    [SerializeField, HideInInspector]
    private SpriteRenderer diagonalLightingSprite;

    private void OnValidate()
    {
        diagonalLightingSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //if the gameobject is tilting right, use the top left Lit sprite and visa versa
        if (Vector2.Dot(gameObject.transform.right, Vector2.up) > 0)
        {
            diagonalLightingSprite.sprite = TopRightLitSprite;
        }
        else
        {
            diagonalLightingSprite.sprite = TopLeftLitSprite;
        }

        //set the opacity of the diagonal lit sprite to scale with the gameobject's angle.
        float lightingSpriteOpacity = Vector2.Angle(transform.right, Vector2.right) / 45;
        diagonalLightingSprite.color = new Color(1f, 1f, 1f, lightingSpriteOpacity);
    }
}
