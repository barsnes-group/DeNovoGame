using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonString;
    private GameController gc;
    private GameObject GameControllerObject;


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
        GameControllerObject = gameObject;
        gc = GameControllerObject.GetComponent<GameController>();
        AminoAcid[] aminoAcids = JsonConvert.DeserializeObject<AminoAcid[]>(jsonString.text);
        //createSlots(slot1)
        gc.CreateSlots(aminoAcids);

        //foreach (AminoAcid s in aminoAcids)
        //{
        //    Debug.Log(s.AminoAcidName + s.Mass );

        //}

    }

}