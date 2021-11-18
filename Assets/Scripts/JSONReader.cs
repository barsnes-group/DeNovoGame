using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonString;

    [System.Serializable]
    public class Slot
    {
        public float x1;
        public float x2;
        public List<float> intensity;

        public override String ToString()
        {
            return x1.ToString() + x2.ToString();
        }
    }
    [System.Serializable]
    public class Wrapper
    {
        public string AminoAcid;
        public Slot[] slots;

    }

    string fixJson(string value)
    {
        value = "{\"Items\": " + value + " }";
        return value;
    }

    void Start()
    {
        //Slot[] slot = JsonHelper.FromJson<Slot>(fixJson(jsonString.text));

        //Debug.Log(slot[2].x1);
        //Debug.Log(slot[3].x1);
        createTestJson();

    }

    private void createTestJson()
    {

        Slot[] slots = new Slot[2];

        slots[0] = new Slot();
        slots[0].x1 = 1f;
        slots[0].x2 = 2f;
        slots[0].intensity = new List<float> { 1f, 2f };

        slots[1] = new Slot();
        slots[1].x1 = 1f;
        slots[1].x2 = 2f;
        slots[1].intensity = new List<float> { 1f, 2f };

        Wrapper[] wrap = new Wrapper[1];
        wrap[0] = new Wrapper();
        wrap[0].AminoAcid = "A";
        wrap[0].slots = slots;

        string slotToJson = JsonHelper.ToJson(wrap, true);
        Debug.Log(slotToJson);

        
        Wrapper[] wrappers = JsonHelper.FromJson<Wrapper>(slotToJson);
        print(wrappers[0].slots[0].x1);

    }
}