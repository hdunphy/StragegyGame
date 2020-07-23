using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public float HoverMultiplier;

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * HoverMultiplier;
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * HoverMultiplier;
    }
}
