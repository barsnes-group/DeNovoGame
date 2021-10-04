using UnityEngine;
using System.Collections.Generic;


public class AminoAcids : MonoBehaviour
{
    float width;
    float height;


    Dictionary<string, float> AminoAcidsDict = new Dictionary<string, float>() {
        {"V", 99.068414f},
        {"N", 114.042927f}};

    void Start()
    {
        foreach(KeyValuePair<string, float> aminoacid in AminoAcidsDict) {
            DrawAminoAcids(aminoacid.Value/10);
        }
        
    }

    void DrawAminoAcids(float width)
    {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(50, 10, 0);
        cube.transform.localScale = new Vector3(width, 14.0f, 0);
        
        var cubeRenderer = cube.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", Color.green);
    }

}
