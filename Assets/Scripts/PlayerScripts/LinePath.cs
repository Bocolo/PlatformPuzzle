using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePath : MonoBehaviour
{
    List<Vector3> playerPositions = new List<Vector3>();
    Vector3 lastPos;
    LineRenderer lineRenderer;
    [SerializeField] float lineWidth = .5f;
    private void Start()
    {
         lastPos = transform.position;
        lineRenderer = GetComponent<LineRenderer>();
       // lineRenderer.SetWidth(0.2F, 0.2F);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
    private void Update()
    {
       

       if (lastPos != transform.position)
        {
            playerPositions.Add(lastPos);

            lastPos = transform.position;
        }
        lineRenderer.positionCount=playerPositions.Count;

        for (int i =0; i< playerPositions.Count; i++)
        {
            lineRenderer.SetPosition(i, playerPositions[i]);
        }

    }
}
