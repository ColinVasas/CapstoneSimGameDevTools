using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject triggerZone;
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

    public void Interact()
    {
        switch(currentState)
        {
            case SpinCoaterState.OpenSpinCoater:
                //place model animate here
                Debug.Log("Spincoater opened");
                currentState = SpinCoaterState.PlaceChuck;
                break;

            case SpinCoaterState.PlaceChuck:
                if(hasChuck)
                {
                    //place model animate here
                    Debug.Log("Chuck Placed");
                    currentState = SpinCoaterState.PlaceWafer;
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
                break;

            case SpinCoaterState.PlacePMMA:
                if(hasPMMA)
                {
                    //place model animate here
                    Debug.Log("PMMA on");
                    currentState = SpinCoaterState.CloseSpinCoater;
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
