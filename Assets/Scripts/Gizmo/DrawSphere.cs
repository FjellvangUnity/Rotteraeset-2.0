using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSphere : MonoBehaviour
{
    public Color Color = Color.yellow;
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color;
        Gizmos.DrawSphere(transform.position, .25f);
    }
}
