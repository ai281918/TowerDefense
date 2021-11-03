using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SurfaceToAirMissile))]
public class SurfaceToAirMissileEditor : Editor
{
    private void OnSceneGUI() {
        SurfaceToAirMissile surfaceToAirMissile = (SurfaceToAirMissile)target;

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(surfaceToAirMissile.transform.position, new Vector3(0, 0, 1), surfaceToAirMissile.radius);
    }
}
