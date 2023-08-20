using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineFreeLookZoom : MonoBehaviour
{
    private CinemachineFreeLook freelook;
    private CinemachineFreeLook.Orbit[] originalOrbits;

    float mouseScrolle;
 
        //[Range(0.5F, 1F)]
        //public float zoomPercent;
    void Awake()
    {
        freelook = GetComponentInChildren<CinemachineFreeLook>();
        originalOrbits = new CinemachineFreeLook.Orbit[freelook.m_Orbits.Length];
 
        for (int i = 0; i < freelook.m_Orbits.Length; i++)
        {
            originalOrbits[i].m_Height = freelook.m_Orbits[i].m_Height;
            originalOrbits[i].m_Radius = freelook.m_Orbits[i].m_Radius;
        }
    }
 
    void Update()
    {
        mouseScrolle -= Input.GetAxis("Mouse ScrollWheel");
        mouseScrolle = Mathf.Clamp(mouseScrolle, 0.5f, 1f);

        for (int i = 0; i < freelook.m_Orbits.Length; i++)
        {
            freelook.m_Orbits[i].m_Height = originalOrbits[i].m_Height * mouseScrolle;
            freelook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * mouseScrolle;
        }
    }
}
