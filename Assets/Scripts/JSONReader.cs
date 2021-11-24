using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonString;
    private GameController gc;
    public GameObject GameControllerObject;


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
    public class AminoAcid
    {
        public string AminoAcidName;
        public float Mass;
        public Slot[] slots;

    }


    void Start()
    {
        gc = GameControllerObject.GetComponent<GameController>();

        AminoAcid[] aminoAcids = JsonConvert.DeserializeObject<AminoAcid[]>(jsonString.text);
        Debug.Log(aminoAcids);
        //createSlots(slot1)
        gc.createSlots(aminoAcids);
        //foreach (AminoAcid s in aminoAcids)
        //{
        //    Debug.Log(s);

        //}

    }

}