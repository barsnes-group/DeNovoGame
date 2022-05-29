using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonString;
    private GameController gc;
    private GameObject GameControllerObject;

    //System.Serializable makes it possible to store objects as text
    [System.Serializable]
    public class SerializedSlot
    {
        public int start_peak_index;
        public float start_peak_coord;
        public int end_peak_index;
        public float end_peak_coord;
        public List<float> intensity;

        public override String ToString()
        {
            return start_peak_index.ToString() + start_peak_coord.ToString() + end_peak_index.ToString() + end_peak_coord.ToString();
        }
    }
    [System.Serializable]
    public class AminoAcid
    {
        public string AminoAcidName;
        public float Mass;
        public float MassOriginal;
        public float MaxXValue;
        public float HighestPeakFromMGF;
        public SerializedSlot[] slots;
        public override String ToString()
        {
            return AminoAcidName;
        }
    }

    void Start()
    {
        GameControllerObject = gameObject;
        gc = GameControllerObject.GetComponent<GameController>();
        AminoAcid[] aminoAcids = JsonConvert.DeserializeObject<AminoAcid[]>(jsonString.text);
        gc.CreateAllBoxes(aminoAcids);
    }

}