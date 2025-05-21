using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public GameObject lid;
    public Material newMaterial;
    public GameObject triggerZoneChuck;
    public GameObject triggerZoneWafer;
    public GameObject triggerZoneHotPlate;
    public GameObject triggerZonePMMA;
    public GameObject triggerZonePI;
    public GameObject triggerZoneEtc;
    public Transform chuckLocation;
    public GameObject chuck;
    public Transform waferLocation;
    public GameObject wafer;
    public GameObject coldWafer;
    public GameObject curWafer;
    [SerializeField] private bool hasWafer = true;
    [SerializeField] private bool hasChuck = true;
    [SerializeField] private bool hasPMMA = true;
    [SerializeField] private bool hasPI = true;
    [SerializeField] private bool done = false;
    private float doneTimer = 0f;
    private float maxDoneTimer = 5f;

    public TextMeshProUGUI equipText; // Single TMP UI element for all messages
    public float fadeDuration = 0.5f;
    public float displayDuration = 1.5f;

    private Coroutine equipMessageCoroutine;
    private int currentMessageIndex = 0;

    private string[] equipMessages = new string[]
    {
        "Go to the spincoater,\nthen press the red button.",
        "Grab a chuck (large black plate) from\nthe table to the right and\nput it in the spincoater.",
        "Grab the spray gun from\nthe table to the right and\nclean a wafer (small black plate).\nThen place the wafer in\nthe spincoater.",
        // "Now take the wafer and\nplace it in the spincoater.",
        "Grab a pipette from the table\nand dip it in the PMMA.",
        "Press the big red button again.",
        "Apply the PMMA to the spincoater.",
        "Start the spincoater\n(via the red button).",
        "Open the spincoater.",
        "Take the wafer out and\nplace it on the hotplate.",
        "Now take the wafer and\nplace it in the spincoater again.",
        "Grab another pipette and dip\nit in the PI",
        "Apply the PI to the spincoater",
        "Start the spincoater\n(via the red button).",
        "All done here! Head to\nthe Wet Etching room!\nLook for the door."
    };

    private enum SpinCoaterState
    {
        OpenSpinCoater,
        PlaceChuck,
        PlaceWafer,
        TurnOnVacuum,
        PlacePMMA,
        TurnOnSpinCoater,
        PlaceWaferBack,
        HotPlate,
        PlacePI,
        TurnOnSpinCoaterAgain,
        CloseSpinCoater,
        Done
    }

    private SpinCoaterState currentState = SpinCoaterState.OpenSpinCoater;

    void Start()
    {
        //// kill process of any current message
        //if (equipMessageCoroutine != null)
        //{
        //    StopCoroutine(equipMessageCoroutine);
        //}
        //// queue up current message
        //if (currentMessageIndex < equipMessages.Length)
        //{
        //    equipMessageCoroutine = StartCoroutine(DisplayTextMessage(equipMessages[currentMessageIndex++]));
        //}
        equipText.text = equipMessages[0];
        equipText.gameObject.SetActive(true);
        equipMessageCoroutine = StartCoroutine(DisplayTextMessage(equipMessages[currentMessageIndex++]));
        //StopCoroutine(equipMessageCoroutine);
        //equipMessageCoroutine = StartCoroutine(DisplayTextMessage(equipMessages[currentMessageIndex++]));
    }

    // yoinked from equipManager.cs
    private IEnumerator DisplayTextMessage(string message)
    {
        equipText.text = message;
        equipText.gameObject.SetActive(true);

        yield return FadeText(0f, 1f, fadeDuration); // Fade in
        yield return new WaitForSeconds(displayDuration);
        yield return FadeText(1f, 0f, fadeDuration); // Fade out

        equipText.gameObject.SetActive(false);
    }
    // also yoinked from equipManager.cs
    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = equipText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            equipText.color = color;
            yield return null;
        }

        color.a = endAlpha;
        equipText.color = color;
    }

    private void Awake()
    {
        triggerZoneChuck.SetActive(false);
        triggerZoneWafer.SetActive(false);
        triggerZonePMMA.SetActive(false);
        triggerZoneEtc.SetActive(true);
        triggerZoneHotPlate.SetActive(false);
        triggerZonePI.SetActive(false);
    }

    public void Interact()
    {
        switch(currentState)
        {
            case SpinCoaterState.OpenSpinCoater:
                Debug.Log("Spincoater opened");
                lid.SetActive(false);
                currentState = SpinCoaterState.PlaceChuck;
                triggerZoneEtc.SetActive(false);
                triggerZoneChuck.SetActive(true);
                break;

            case SpinCoaterState.PlaceChuck:
                if(hasChuck)
                {
                    Debug.Log("Chuck Placed");
                    currentState = SpinCoaterState.PlaceWafer;
                    triggerZoneWafer.SetActive(true);
                    triggerZoneChuck.SetActive(false);
                    Quaternion cusRotation = Quaternion.Euler(90, 0, 0);
                    Instantiate(chuck, chuckLocation.position, cusRotation);
                }
                else
                {
                    Debug.Log("not holding chuck");
                }
                break;
            
            case SpinCoaterState.PlaceWafer:
                if(hasWafer)
                {
                    Debug.Log("Wafer Placed");
                    currentState = SpinCoaterState.TurnOnVacuum;
                    triggerZoneWafer.SetActive(false);
                    triggerZoneEtc.SetActive(true);
                    // cusRotation = Quaternion.Euler(0, 0, 0);
                    curWafer = Instantiate(wafer, waferLocation.position, Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    Debug.Log("not holding wafer");
                }
                break;
            

            case SpinCoaterState.TurnOnVacuum:
                //place model animate here
                Debug.Log("Vacuum on");
                // lid.SetActive(true);
                // done = true;
                //triggerZoneEtc.SetActive(false);
                triggerZonePMMA.SetActive(true);
                currentState = SpinCoaterState.PlacePMMA;
                break;

            // case SpinCoaterState.HotPlate:
            //     break;
            case SpinCoaterState.PlacePMMA:
                if(hasPMMA)
                {
                    //place model animate here
                    Debug.Log("PMMA on");
                    currentState = SpinCoaterState.TurnOnSpinCoater;
                    triggerZonePMMA.SetActive(false);
                    lid.SetActive(true);
                    triggerZoneEtc.SetActive(true);
                }
                else
                {
                    Debug.Log("not holding PMMA");
                }
                break;
            case SpinCoaterState.TurnOnSpinCoater:
                done = true;
                // currentState = SpinCoaterState.HotPlate;
                break;
            case SpinCoaterState.HotPlate:
                Debug.Log("wafer here");
                lid.SetActive(false);
                triggerZoneHotPlate.SetActive(true);
                Destroy(curWafer);
                // Quaternion cusRotation = Quaternion.Euler(0, 0, 0);
                Instantiate(coldWafer, waferLocation.position, Quaternion.Euler(0, 0, 0));
                currentState = SpinCoaterState.PlaceWaferBack;
                break;
            case SpinCoaterState.PlaceWaferBack:
                triggerZoneHotPlate.SetActive(false);
                triggerZonePI.SetActive(true);
                //cusRotation = Quaternion.Euler(0, 0, 0);
                curWafer = Instantiate(wafer, waferLocation.position, Quaternion.Euler(0, 0, 0));
                Renderer renderer = curWafer.GetComponent<Renderer>();
                if(renderer != null)
                {
                    Material[] mats = renderer.materials;
                    if(mats.Length >= 2)
                    {
                        mats[0] = newMaterial;
                        mats[1] = newMaterial;
                    }
                    renderer.materials = mats;
                }
                currentState = SpinCoaterState.PlacePI;
                break;
            case SpinCoaterState.PlacePI:
                if(hasPI)
                {
                    //place model animate here
                    Debug.Log("PI on");
                    currentState = SpinCoaterState.TurnOnSpinCoaterAgain;
                    triggerZonePI.SetActive(false);
                    lid.SetActive(true);
                    triggerZoneEtc.SetActive(true);
                }
                else
                {
                    Debug.Log("not holding PMMA");
                }
                break;
            case SpinCoaterState.TurnOnSpinCoaterAgain:
                done = true;
                // currentState = SpinCoaterState.HotPlate;
                break;

            case SpinCoaterState.Done:
                lid.SetActive(false);
                Debug.Log("done.");
                break;
        }

        // this was ported from equipManager.cs but doesn't seem to work,
        // I may need to try a different approach...

        // kill process of any current message
        if (equipMessageCoroutine != null)
        {
            StopCoroutine(equipMessageCoroutine);
        }
        // queue up current message
        if (currentMessageIndex < equipMessages.Length)
        {
            equipMessageCoroutine = StartCoroutine(DisplayTextMessage(equipMessages[currentMessageIndex++]));
        }
    }

    public void ActivateChuck()
    {
        hasChuck = true;
    }

    public void ActivateWafer()
    {
        hasWafer = true;
    }

    void Update()
    {
        if(done == true) {
            if(doneTimer < maxDoneTimer)
            {
                curWafer.transform.Rotate(0, 300f * Time.deltaTime, 0);
                doneTimer += Time.deltaTime;
            }
            else
            {
                done = false;
                doneTimer = 0f;
                if(currentState == SpinCoaterState.TurnOnSpinCoater)
                {
                    currentState = SpinCoaterState.HotPlate;
                }
                else
                {
                    currentState = SpinCoaterState.Done;
                }
            }
        }
    }
}
