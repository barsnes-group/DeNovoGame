using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonString;

    [System.Serializable]
    public class Slot
    {
        public float x1;
        public float x2;
        public List<float> intensity;
    }

    [System.Serializable]
    public class Slots
    {
        public Slot[] D;
    }

    public Slots d = new Slots();

    void Start()
    {

        d = JsonUtility.FromJson<Slots>(jsonString.text);
        Debug.Log("JSON: " + d);

    }

}
