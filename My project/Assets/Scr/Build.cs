using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    public Renderer renderer;
    public Vector2Int size;
    
    public void SetTransform(bool bulding)
    {
        if (bulding == true)
        {
            renderer.material.color = Color.green;
        }
        else
        {
            renderer.material.color = Color.red;
        }
    }

    public void SetBuild()
    {
        renderer.material.color= Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        for(int x=0; x<size.x; x++)
        {
            for (int y=0; y<size.y; y++)
            {
                Gizmos.color = Color.yellow;

                Gizmos.DrawCube(transform.position + new Vector3(x, 0.05f, y), new Vector3(1,0.1f,1));
            }
        }
    }

    
}
