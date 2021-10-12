using UnityEngine;
using System.Collections.Generic;


public class AminoAcids : MonoBehaviour
{
    float width;
    float height;


    Dictionary<string, float> AminoAcidsDict = new Dictionary<string, float>() {
       /*  {"A", 71.037114f},
        {"R", 156.101111f},
        {"N", 114.042927f},
        {"D", 115.026943f},
        {"C", 103.009185f},
        {"E", 129.042593f},
        {"Q", 128.058578f},
        {"G", 57.021464f},
        {"H", 137.058912f},
        {"I", 113.084064f},
        {"L", 113.084064f},
        {"K", 128.094963f},
        {"M", 131.040485f},
        {"F", 147.068414f},
        {"P", 97.052764f},
        {"S", 87.032028f},
        {"T", 101.047679f},
        {"U", 150.95363f},
        {"W", 186.079313f},
        {"Y", 163.06332f},
        {"V", 99.068414f} */

        {"N", 114.042927f},
        {"V", 99.068414f}
    };



    void Start()
    {

        for (int i = 0; i < AminoAcidsDict.Count - 1; i++)
        {

            foreach (KeyValuePair<string, float> aminoacid in AminoAcidsDict)
            {
                DrawAminoAcids(aminoacid.Value / 10, aminoacid.Value);

            }

        }

    }

    void DrawAminoAcids(float width, float x)
    {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().material.color = new Color(17, 243, 129, 255);
        cube.transform.position = new Vector3(x, 20, 0);
        cube.transform.localScale = new Vector3(width, 5.0f, 0);

    }

}
