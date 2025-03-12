using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject lid;
    public GameObject triggerZoneChuck;
    public GameObject triggerZoneWafer;
    public GameObject triggerZonePMMA;
    public GameObject triggerZoneEtc;
    public Transform chuckLocation;
    public GameObject chuck;
    public Transform waferLocation;
    public GameObject wafer;
    private GameObject curWafer;
    [SerializeField] private bool hasWafer = true;
    [SerializeField] private bool hasChuck = true;
    [SerializeField] private bool hasPMMA = true;
    [SerializeField] private bool done = false;

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
        triggerZoneEtc.SetActive(true);
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
                    //place model animate here
                    Debug.Log("Wafer Placed");
                    currentState = SpinCoaterState.TurnOnVacuum;
                    triggerZoneWafer.SetActive(false);
                    triggerZoneEtc.SetActive(true);
                    Quaternion cusRotation = Quaternion.Euler(0, 0, 0);
                    curWafer = Instantiate(wafer, waferLocation.position, cusRotation);
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
                triggerZoneEtc.SetActive(false);
                break;

            case SpinCoaterState.PlacePMMA:
                if(hasPMMA)
                {
                    //place model animate here
                    Debug.Log("PMMA on");
                    currentState = SpinCoaterState.CloseSpinCoater;
                    triggerZonePMMA.SetActive(false);
                    triggerZoneEtc.SetActive(true);
                }
                else
                {
                    Debug.Log("not holding PMMA");
                }
                break;

            case SpinCoaterState.CloseSpinCoater:
                lid.SetActive(true);
                Debug.Log("close spin");
                currentState = SpinCoaterState.Done;
                break;

            case SpinCoaterState.Done:
                done = true;
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

    void Update()
    {
        if(done == true) {
            curWafer.transform.Rotate(0, 300f * Time.deltaTime, 0);
        }
    }
}
