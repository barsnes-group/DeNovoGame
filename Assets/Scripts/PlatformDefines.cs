using UnityEngine;
using System.Collections;

public class PlatformDefines : MonoBehaviour
{
    void Start()
    {
        #if UNITY_STANDALONE
        Debug.Log("Unity Standalone");
        #endif
    }
}