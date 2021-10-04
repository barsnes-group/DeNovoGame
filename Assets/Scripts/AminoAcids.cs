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

        for (int i = 0; i < AminoAcidsDict.Count-1; i++)
        {

        foreach(KeyValuePair<string, float> aminoacid in AminoAcidsDict) {
            DrawAminoAcids(aminoacid.Value/10, aminoacid.Value);

        }
            
        }
        
    }

    void DrawAminoAcids(float width, float x)
    {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().material.color = new Color(0,1,0,1);
        cube.transform.position = new Vector3(x, 20, 0);
        cube.transform.localScale = new Vector3(width, 5.0f, 0);

    }

}
