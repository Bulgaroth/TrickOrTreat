using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    
    #region Attributes
    public float radius = 1;
    public Vector3 regionSize = Vector3.one;
    public int rejectionSamples = 30;
    public float displayRadius =1;

    List<Vector3> points;
    
    #endregion
    
    #region Unity API

    private void Awake()
    {
        Instance = this;
        
    }

    #endregion
    
    

    void OnValidate() {
        points = PoissonDiscSampling.Get3DPointsFrom2DPointList(radius, new Vector2(regionSize.x, regionSize.z), rejectionSamples);
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, regionSize);
        if (points != null) {
            foreach (Vector3 point in points) {
                Gizmos.DrawSphere(transform.position + point, displayRadius);
            }
        }
    }
}
