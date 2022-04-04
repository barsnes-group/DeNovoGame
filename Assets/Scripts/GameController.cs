using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public TextAsset CsvFile;
    public GameObject boxPrefab;
    public GameObject peakPrefab;
    public GameObject slotPrefab;
    public GameObject warningObject;
    public int validSlotsCount;
    public bool lastBoxPlacedOccupied;
    private List<Slot> highlightedSlots = new List<Slot>();
    private List<Slot> validSlots = new List<Slot>();
    public int occupiedSlotsCount = 0;
    [SerializeField]
    private GameObject scoreObject;

    [Header("Sizes")]
    public float slotAndBoxScaling;
    public float peaksYPos = 15;
    public float boxYPos = 13;
    public float scaleWidth = 0.1f;
    public int rightBoxMarginSpace = 5;
    [SerializeField]
    GameObject PeakContainer;

    void Start()
    {
        if (scoreObject == null)
        {
            throw new Exception("score obj. null");
        }

        DrawLine();
        string[] array = CsvFile.text.Split('\n');
        float previousX = 0;
        float xCoord = 5.0f; //= float.Parse(rows[0]);
        for (int i = 0; i <= array.Length - 1; i++)
        {
            string[] rows = array[i].Split(',');
            CreatePeakPrefab(xCoord, peaksYPos, 0.2f, 1, xCoord - previousX, i);
            xCoord += 7;
            previousX = xCoord;
        }
    }

    private void Update()
    {
        Score scoreComponent = scoreObject.GetComponent<Score>();
        scoreComponent.CalculateScore(GetAllBoxes());
        validSlotsCount = validSlots.Count;
        //scoreComponent.UpdateHighScore(); //TODO: flytt til en gamecontroller i menu
    }

    Slot CreateSlotPrefab(float pos_y, float intensity, Peak startPeak, Peak endPeak, bool valid)
    {
        float pos_x1 = startPeak.transform.position.x;
        float pos_x2 = endPeak.transform.position.x;
        if (startPeak == null || endPeak == null)
        {
            throw new NullReferenceException();
        }
        if (pos_x1 == pos_x2)
        {
            throw new ArgumentException();
        }
        GameObject slotObject = Instantiate(slotPrefab, new Vector3(pos_x1, pos_y, 0), Quaternion.identity);
        slotObject.transform.SetParent(GameObject.Find("ValidSlotsContainer").transform);

        Slot slot = slotObject.GetComponent<Slot>();
        slot.startpeak = startPeak;
        slot.endpeak = endPeak;
        slot.SetScale(MathF.Abs(pos_x2 - pos_x1), intensity);

        if (!valid)
        {
            slot.SetColor(new Color32(229, 143, 30, 100));
        }

        //TODO: check if this can be removed
        if (pos_x1 > pos_x2)
        {
            slot.SetPos(pos_x2, pos_y);
        }
        else
        {
            slot.SetPos(pos_x1, pos_y);
        }
        return slot;
    }

    Peak CreatePeakPrefab(float pos_x, float pos_y, float scale_x, float scale_y, float width_to_prev, int index)
    {
        GameObject peakObject = Instantiate(peakPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        peakObject.transform.SetParent(GameObject.Find("PeakContainer").transform);

        Peak peak = peakObject.GetComponent<Peak>();
        peak.SetImageScale(scale_x * slotAndBoxScaling, scale_y * slotAndBoxScaling);
        peak.SetPos(pos_x * scaleWidth, pos_y);
        peak.SetText(index + "\n" + width_to_prev.ToString());
        peak.index = index;
        return peak;
    }

    DraggableBox CreateBoxPrefab(float pos_x, float pos_y, float scale_x, float scale_y)
    {
        GameObject boxObject = Instantiate(boxPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        boxObject.transform.SetParent(GameObject.Find("BoxContainer").transform);
        DraggableBox box = boxObject.GetComponent<DraggableBox>();
        box.width = scale_x;
        box.SetScale(scale_x * scaleWidth, scale_y * slotAndBoxScaling);
        box.SetPos(pos_x * scaleWidth, pos_y);
        box.posX = pos_x;
        box.SetScoreObject(scoreObject);
        return box;
    }

    internal void HighlightValidSlots(List<int> startIndexes, List<int> endIndexes)
    {
        if (startIndexes.Count != endIndexes.Count)
        {
            throw new Exception("different size of index lists");
        }
        print("start highlighted Slots of count " + startIndexes.Count);
        foreach (var startAndEndIndexes in startIndexes.Zip(endIndexes, Tuple.Create))
        {
            Peak startPeak = GetPeak(startAndEndIndexes.Item1);
            Peak endPeak = GetPeak(startAndEndIndexes.Item2);
            float avgIntensity = (startPeak.intensity + endPeak.intensity) / 2;
            if (!SlotOccupied(startPeak.index, endPeak.index, GetAllBoxes()))
            {
                Slot slot = CreateSlotPrefab(peaksYPos, avgIntensity / 5, startPeak, endPeak, true);
                if (slot == null)
                {
                    print("slot in HighlightValidSlots() is null");
                }
                highlightedSlots.Add(slot);
                validSlots.Add(slot);
            }
            else //highlight slots som ikke er occupied og ikke valid 
            {
                print("highlight unvalid slots");
                Slot slot = CreateSlotPrefab(peaksYPos, avgIntensity / 5, startPeak, endPeak, false);
                highlightedSlots.Add(slot);
            }
        }
    }

    public void ClearSlots()
    {
        highlightedSlots.Clear();
        validSlots.Clear();
        List<Slot> allValidSlots;
        GameObject container = GameObject.Find("ValidSlotsContainer");
        allValidSlots = container.GetComponentsInChildren<Slot>().ToList();

        foreach (Slot slot in allValidSlots)
        {
            Destroy(slot.gameObject);
        }
    }

    /*
    box was placed on a valid slot
    */
    internal Slot BoxPlaced(int score, DraggableBox draggableBox, Peak startpeak)
    {
        print("highlightedSlots: " + highlightedSlots.Count);

        Slot selectedSlot = FindMatchingSlot(startpeak, draggableBox);

        draggableBox.SetScale(selectedSlot.GetSlotScaleX(), selectedSlot.GetSlotScaleY());
        draggableBox.placedStartPeak = selectedSlot.startpeak;
        draggableBox.placedEndPeak = selectedSlot.endpeak;

        lastBoxPlacedOccupied = SlotOccupied(selectedSlot.startpeak.index, selectedSlot.endpeak.index, GetAllBoxes());

        if (lastBoxPlacedOccupied)
        {
            HandleInvalidBoxPlacement(draggableBox);
        }
        else
        {
            SpawnNewBox(draggableBox);
        }

        return selectedSlot;
    }

    public void HandleInvalidBoxPlacement(DraggableBox box)
    {
        Warnings warningComponent = warningObject.GetComponent<Warnings>();
        print("Warning! You can't place box on top of another box");
        //TODO: legg til lyd?
        warningComponent.SetText("You can't place box on top of another box");
        box.ReturnToStartPos();
    }

    /*
    change the scale of the box to the scale of the slot it is placed on
    */
    private Slot FindMatchingSlot(Peak startpeak, DraggableBox draggableBox)
    {
        foreach (Slot slot in highlightedSlots)
        {
            //TODO: sjekk at den passer itillegg
            if (slot.startpeak.index == startpeak.index)// && slot.GetWidth() == Int32.Parse(draggableBox.width))
            {
                print("set box to slot scale: " + slot.GetSlotScaleX() + " , y: " + slot.GetSlotScaleY());
                print("slot width: " + slot.GetWidth() + " box width: " + draggableBox.width);
                return slot;
            }
        }
        throw new NullReferenceException("selected slot is null");
    }

    /*
    spawn a new box if there are available slots for that box type
    */
    private void SpawnNewBox(DraggableBox previousBox)
    {
        JSONReader.SerializedSlot[] possibleSlots = previousBox.aminoAcidChar.slots;
        print("possibleSlots length: " + possibleSlots.Length);
        for (int i = 0; i < possibleSlots.Length - 1; i++)
        {
            JSONReader.SerializedSlot serializedSlot = possibleSlots[i];
            int start_peak_index = serializedSlot.start_peak_index;
            int end_peak_index = serializedSlot.end_peak_index;
            if (!SlotOccupied(start_peak_index, end_peak_index, GetAllBoxes()))
            {
                DraggableBox newBox = CreateBox(previousBox.aminoAcidChar, previousBox.posX);
                //the box always has the same color
                newBox.SetColor(previousBox.GetColor());
                //exit the loop when one new box is created
                return;
            }
        }
    }

    /*
    true if slot between (slotstart, slotend) is occupied by any other box
    */
    public bool SlotOccupied(int slotStart, int slotEnd, DraggableBox[] draggableBoxes)
    {
        for (int i = 0; i < draggableBoxes.Length; i++)
        {
            if (slotStart == null && slotEnd == null)
            {
                throw new NullReferenceException("slot start or end is null in SlotOccupied()");
            }
            DraggableBox box = draggableBoxes[i];
            if (box.GetIsPlaced())
            {
                int boxStart = box.GetStartPeak().index;
                int boxEnd = box.GetEndPeak().index;
                if (boxStart == null && boxEnd == null)
                {
                    throw new NullReferenceException("box start or end is null in SlotOccupied()");
                }

                bool overlaps = Overlap(boxStart, boxEnd, slotStart, slotEnd);
                if (overlaps)
                {
                    print("box " + i + " overlaps bs: " + boxStart + " be: " + boxEnd + " ss: " + slotStart + " se: " + slotEnd);
                    return true;
                }
            }
        }
        return false;
    }

    private bool Overlap(int boxStart, int boxEnd, int slotStart, int slotEnd)
    {
        if (boxStart >= slotEnd)
        {
            return false;
        }
        else if (boxEnd <= slotStart)
        {
            return false;
        }
        return true;
    }

    public DraggableBox[] GetAllBoxes()
    {
        return GameObject.Find("BoxContainer").GetComponentsInChildren<DraggableBox>();
    }

    public Peak GetPeak(int index)
    {
        GameObject peakIndex = PeakContainer.transform.GetChild(index).gameObject;
        Peak peak = peakIndex.GetComponent<Peak>();

        if (peakIndex == null || peak == null)
        {
            throw new Exception("peak index or peak = null");
        }
        return peak;
    }

    public void CreateAllBoxes(JSONReader.AminoAcid[] aminoAcids)
    {
        int rightBoxMargin = 0;
        foreach (JSONReader.AminoAcid aminoAcidChar in aminoAcids)
        {
            validSlotsCount = aminoAcidChar.slots.Length;
            if (aminoAcidChar.slots.Length > 0)
            {
                CreateBox(aminoAcidChar, aminoAcidChar.Mass + rightBoxMargin);
                rightBoxMargin += rightBoxMarginSpace;
            }
        }
    }

    private DraggableBox CreateBox(JSONReader.AminoAcid aminoAcidChar, float xPos)
    {
        //DraggableBox box = CreateBoxPrefab(xPos, boxYPos, aminoAcidChar.Mass, aminoAcidChar.Mass);
        DraggableBox box = CreateBoxPrefab(xPos, boxYPos, 3, 3);
        box.aminoAcidChar = aminoAcidChar;
        box.SetColor(new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255));
        box.SetText(validSlotsCount.ToString() + " / " + aminoAcidChar.slots.Length.ToString());

        foreach (JSONReader.SerializedSlot slot in aminoAcidChar.slots)
        {
            box.startPeakNumbers.Add(slot.start_peak_index);
            box.endPeakNumbers.Add(slot.end_peak_index);
            box.SwitchStartAndEndIndexes();

            //add intensity to peaks
            Peak startPeak = GetPeak(slot.start_peak_index);
            Peak endPeak = GetPeak(slot.end_peak_index);
            float startPeakIntensity = slot.intensity[0];
            float endPeakIntensity = slot.intensity[1];
            startPeak.intensity = startPeakIntensity;
            endPeak.intensity = endPeakIntensity;
            startPeak.coord = slot.start_peak_coord;
        }
        return box;
    }

    private void DrawLine()
    {
        LineRenderer l = gameObject.AddComponent<LineRenderer>();
        List<Vector3> pos = new List<Vector3>
        {
            new Vector3(-500, peaksYPos, 0),
            new Vector3(1000, peaksYPos, 0)
        };

        l.startWidth = 0.1f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
    }
}