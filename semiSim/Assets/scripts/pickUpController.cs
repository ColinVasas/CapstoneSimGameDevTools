using System.Collections;
using TMPro;
using UnityEngine;

public class pickUpController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    private GameObject hoveredObj;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;
    [SerializeField] private float rotationSpeed = 100.0f;

    public bool isRotating { get; private set; } = false;

    [Header("UI")]
    public TextMeshProUGUI equipText;
    public float fadeDuration = 0.5f;
    public float displayDuration = 3.0f;
    private Coroutine messageCoroutine;
    private Coroutine fadeCoroutine;
    private Coroutine persistentCoroutine;
    private deskCanvasText introText;
    private KeyboardInteractable keyboardInteractable;

    private void Start()
    {
        introText = FindFirstObjectByType<deskCanvasText>();

        equipText.gameObject.SetActive(false);
        var c = equipText.color;
        c.a = 0f;
        equipText.color = c;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
                {
                    // gowning room equip text tutorial 
                    if (PickupObject(hit.transform.gameObject) && hit.transform.CompareTag("equip"))
                    {
                        StopMessage();
                        messageCoroutine = StartCoroutine(ShowPersistentMessage("Press 'E' to equip"));
                    }
                    // picking up a chuck
                    else if (PickupObject(hit.transform.gameObject) && heldObj.name == "chuck")
                    {
                        StopMessage();
                        messageCoroutine = StartCoroutine(DisplayTextMessage("This is a chuck"));
                    }
                    // picking up a wafer
                    else if (PickupObject(hit.transform.gameObject) && (heldObj.name == "wafer" || hit.transform.CompareTag("wafer") || 
                    hit.transform.CompareTag("coldWafer") || heldObj.name == "wafer (2)" || heldObj.name == "WaferPrefabTesting"
                    || heldObj.name == "wafer 1"))
                    {
                        StopMessage();
                        messageCoroutine = StartCoroutine(DisplayTextMessage("This is a wafer"));
                    }
                    // picking up a pipet
                    else if (PickupObject(hit.transform.gameObject) && (heldObj.name == "pipette_cap" || heldObj.name == "pipette_cap (1)"))
                    {
                        StopMessage();
                        messageCoroutine = StartCoroutine(DisplayTextMessage("This is a pipet"));
                    }
                    // picking up a spay gun
                    else if (PickupObject(hit.transform.gameObject) && heldObj.name == "spray gun")
                    {
                        StopMessage();
                        messageCoroutine = StartCoroutine(DisplayTextMessage("This is a spray gun"));
                    }
                    // not displaying any text for beaker
                    else if (PickupObject(hit.transform.gameObject) && heldObj.name == "Beaker")
                    {
                        //StopMessage();
                        //messageCoroutine = StartCoroutine(DisplayTextMessage("This is a beaker"));
                    }
                    // not displaying any text for digital clock
                    else if (PickupObject(hit.transform.gameObject) && heldObj.name == "DigitalClock")
                    { 
                        //StopMessage();
                        //messageCoroutine = StartCoroutine(DisplayTextMessage("This is a digital clock"));
                    }
                    // not displaying any text for bottles
                    else if (PickupObject(hit.transform.gameObject) && (heldObj.name == "bottle" || heldObj.name == "Bottle" || 
                    heldObj.name == "Bottle (4)" || heldObj.name == "Bottle (5)" || heldObj.name == "Bottle (6)" || 
                    heldObj.name == "Bottle (7)"))
                    {
                        StopMessage();
                        messageCoroutine = StartCoroutine(DisplayTextMessage("Tip: Hold 'R' and move mouse to rotate.\n(desktop only)"));
                    }
                    // picking up tweezers
                    else if (PickupObject(hit.transform.gameObject) && (heldObj.name == "Tweezers"))
                    {
                        StopMessage();
                        messageCoroutine = StartCoroutine(DisplayTextMessage("These are tweezers"));
                    }
                    // picking up a wafer holder
                    else if (PickupObject(hit.transform.gameObject) && (heldObj.name == "WaferHolder"))
                    {
                        StopMessage();
                        messageCoroutine = StartCoroutine(DisplayTextMessage("This is the Wafer Holder"));
                    }
                    // This is for the gowning room procedures. Specifically, if the user picks up an item in the wrong equip order,
                    // it will bring up a message telling the user they're doing things wrong.
                    else if (heldObj != null)
                    {
                        // This code has been commented out because it reacts to ALL unknown objects. When enough "else if" cases
                        // have been typed to account for all objects that can be picked up, the code below can be uncommented.


                        //StopMessage();
                        //Debug.Log(heldObj.name);
                        //messageCoroutine = StartCoroutine(ShowPersistentMessage("Whoops! Wrong object!\nLeft click to put down"));
                    }
                }
            }
            else
            {
                DropObject();
            }
        }

        // code for rotating object when 'R' is pressed
        if (Input.GetKey(KeyCode.R) && heldObj != null)
        {
            isRotating = true;
            RotateObject();
        }
        else
        {
            isRotating = false;
            if (heldObj != null) MoveObject();
        }

        // code for equipping object when 'E' is pressed (gowning room only)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryEquipHeldObject();
        }

        HoverManager();
    }

    // code for picking up object with left click
    private bool PickupObject(GameObject pickObj)
    {
        Rigidbody rb = pickObj.GetComponent<Rigidbody>();
        if (rb == null) return false;

        introText?.StopIntro();

        heldObjRB = rb;
        heldObjRB.useGravity = false;
        heldObjRB.linearDamping = 10;
        heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
        pickObj.layer = LayerMask.NameToLayer("TransparentFX");
        heldObjRB.transform.parent = holdArea;
        heldObj = pickObj;

        keyboardInteractable = pickObj.GetComponent<KeyboardInteractable>();
        keyboardInteractable?.OnPickUpEnter();

        return true;
    }

    // code for dropping object with left click
    private void DropObject()
    {
        StopMessage();

        if (heldObjRB != null)
        {
            heldObjRB.useGravity = true;
            heldObjRB.linearDamping = 1;
            heldObjRB.constraints = RigidbodyConstraints.None;
            heldObjRB.transform.parent = null;
            heldObj.layer = LayerMask.NameToLayer("Default");
        }

        keyboardInteractable?.OnPickUpExit();

        heldObj = null;
        heldObjRB = null;
        keyboardInteractable = null;
    }

    // code for moving an object that a player has grabbed
    private void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = holdArea.position - heldObj.transform.position;
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    // R + mouse movements = rotation
    private void RotateObject()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        heldObjRB.angularVelocity = Vector3.zero;
        heldObj.transform.Rotate(Vector3.up, -mouseX, Space.World);
        heldObj.transform.Rotate(Vector3.right, mouseY, Space.World);
    }

    private void TryEquipHeldObject()
    {
        // only objects with the "equip" tag can be equipped
        // as the user goes through the gowning room procedures, the next object to be equipped
        // gets the "equip" tag
        if (heldObj != null && heldObj.CompareTag("equip"))
        {
            StopMessage();
            EquipObject();
        }
    }

    private void EquipObject()
    {
        Debug.Log("The object has been equipped.");

        equipManager manager = FindFirstObjectByType<equipManager>();
        manager?.HandleEquip(heldObj);

        heldObj.SetActive(false);
        heldObj = null;
    }

    private void HoverManager()
    {
        if (heldObj != null) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
        {
            var newInteractable = hit.transform.GetComponent<KeyboardInteractable>();
            if (newInteractable != null)
            {
                if (hoveredObj == null || hoveredObj != hit.transform.gameObject)
                {
                    hoveredObj?.GetComponent<KeyboardInteractable>()?.OnHoverExit();
                    newInteractable.OnHoverEnter();
                    hoveredObj = hit.transform.gameObject;
                }
            }
            else if (hoveredObj != null)
            {
                hoveredObj.GetComponent<KeyboardInteractable>()?.OnHoverExit();
                hoveredObj = null;
            }
        }
        else if (hoveredObj != null)
        {
            hoveredObj.GetComponent<KeyboardInteractable>()?.OnHoverExit();
            hoveredObj = null;
        }
    }

    // stops a popup text message
    private void StopMessage()
    {
        if (messageCoroutine != null) StopCoroutine(messageCoroutine);
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (persistentCoroutine != null) StopCoroutine(persistentCoroutine);

        messageCoroutine = null;
        fadeCoroutine = null;
        persistentCoroutine = null;

        if (equipText == null)
            return;
        equipText.gameObject.SetActive(false);
        var c = equipText.color;
        c.a = 0f;
        equipText.color = c;
    }

    // supposed to be for showing a text popup that doesn't fade away, but it's kinda buggy
    private IEnumerator ShowPersistentMessage(string msg)
    {
        StopFadeCoroutine();

        equipText.text = msg;
        equipText.gameObject.SetActive(true);
        yield return FadeText(0, 1, fadeDuration);

        while (true) yield return null;
    }

    // displays a text message that will fade after a period of time
    public IEnumerator DisplayTextMessage(string message)
    {
        StopFadeCoroutine();

        equipText.text = message;
        equipText.gameObject.SetActive(true);
        yield return FadeText(0f, 1f, fadeDuration);
        yield return new WaitForSeconds(displayDuration);
        yield return FadeText(1f, 0f, fadeDuration);
        equipText.gameObject.SetActive(false);
    }

    // code for fading in/out text popup messages
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

    // supposed to help with stopping text from fading when we want the message to persist
    // unfortunately, kinda buggy
    private void StopFadeCoroutine()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }

    public bool IsRotatingObject()
    {
        return isRotating;
    }
}
