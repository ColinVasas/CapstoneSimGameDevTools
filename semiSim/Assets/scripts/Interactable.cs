using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject lid;
    public GameObject triggerZoneChuck;
    public GameObject triggerZoneWafer;
    public GameObject triggerZonePMMA;
    [SerializeField] private bool hasWafer = true;
    [SerializeField] private bool hasChuck = true;
    [SerializeField] private bool hasPMMA = true;

    private enum SpinCoaterState
    {
        OpenSpinCoater,
        PlaceChuck,
        PlaceWafer,
        TurnOnVacuum,
        PlacePMMA,
        CloseSpinCoater,
        Done
    }

    private SpinCoaterState currentState = SpinCoaterState.OpenSpinCoater;
    private void Awake()
    {
        triggerZoneChuck.SetActive(false);
        triggerZoneWafer.SetActive(false);
        triggerZonePMMA.SetActive(false);
    }

    public void Interact()
    {
        switch(currentState)
        {
            case SpinCoaterState.OpenSpinCoater:
                Debug.Log("Spincoater opened");
                lid.SetActive(false);
                currentState = SpinCoaterState.PlaceChuck;
                triggerZoneChuck.SetActive(true);
                break;

            case SpinCoaterState.PlaceChuck:
                if(hasChuck)
                {
                    //place model animate here
                    Debug.Log("Chuck Placed");
                    currentState = SpinCoaterState.PlaceWafer;
                    triggerZoneWafer.SetActive(true);
                    triggerZoneChuck.SetActive(false);
                }
                else
                {
                    Debug.Log("not holding chuck");
                }
                break;
            
            case SpinCoaterState.PlaceWafer:
                if(hasWafer)
                {
                    //place model animate here
                    Debug.Log("Wafer Placed");
                    currentState = SpinCoaterState.TurnOnVacuum;
                    triggerZoneWafer.SetActive(false);
                }
                else
                {
                    Debug.Log("not holding wafer");
                }
                break;

            case SpinCoaterState.TurnOnVacuum:
                //place model animate here
                Debug.Log("Vacuum on");
                currentState = SpinCoaterState.PlacePMMA;
                triggerZonePMMA.SetActive(true);
                break;

            case SpinCoaterState.PlacePMMA:
                if(hasPMMA)
                {
                    //place model animate here
                    Debug.Log("PMMA on");
                    currentState = SpinCoaterState.CloseSpinCoater;
                    triggerZonePMMA.SetActive(false);
                }
                else
                {
                    Debug.Log("not holding PMMA");
                }
                break;

            case SpinCoaterState.CloseSpinCoater:
                //place model animate here
                Debug.Log("close spin");
                currentState = SpinCoaterState.Done;
                break;

            case SpinCoaterState.Done:
                Debug.Log("done.");
                break;
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
}
