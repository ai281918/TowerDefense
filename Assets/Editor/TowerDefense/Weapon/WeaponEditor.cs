using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    private void OnSceneGUI() {
        Weapon weapon = (Weapon)target;

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(weapon.transform.position, new Vector3(0, 0, 1), weapon.radius);
    }
}
