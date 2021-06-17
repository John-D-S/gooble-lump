using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DiagonalLightingSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite TopLeftLitSprite;
    [SerializeField]
    private Sprite TopRightLitSprite;
    [SerializeField, HideInInspector]
    private SpriteRenderer diagonalLightingSprite;

    private void OnValidate()
    {
        diagonalLightingSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (Vector2.Dot(gameObject.transform.right, Vector2.up) > 0)
        {
            diagonalLightingSprite.sprite = TopRightLitSprite;
        }
        else
        {
            diagonalLightingSprite.sprite = TopLeftLitSprite;
        }

        float lightingSpriteOpacity = Vector2.Angle(transform.right, Vector2.right) / 45;
        diagonalLightingSprite.color = new Color(1f, 1f, 1f, lightingSpriteOpacity);
    }
}
