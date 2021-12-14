using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AminoAcids : MonoBehaviour
{
    public SlotDeserializer[] D;
}
/*
    float width;
    float height;

    public TextAsset JSONFile;
    public float x1;

    public float x2;

    public List<float> intensity;

    void Start()
    {
        
        PlayerInfor(JSONFile);
        for (int i = 0; i < AminoAcidsDict.Count - 1; i++)
        {

            foreach (KeyValuePair<string, float> aminoacid in AminoAcidsDict)
            {
                DrawAminoAcids(aminoacid.Value / 10, aminoacid.Value);

            }

        } 

    }

    public static AminoAcidInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AminoAcidInfo>(jsonString);
    }

    void DrawAminoAcids(float width, float x)
    {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().material.color = new Color(17, 243, 129, 255);
        cube.transform.position = new Vector3(x, 20, 0);
        cube.transform.localScale = new Vector3(width, 5.0f, 0);

    }

}
 */