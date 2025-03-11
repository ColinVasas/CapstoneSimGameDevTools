using System;
using UnityEngine;

public class PourableLiquidContainer : MonoBehaviour
{
    
    [SerializeField] Transform pourPoint;

    [Header("Set these based on container")]
    [SerializeField] bool hasCap;

    [Header("Stream")]
    [SerializeField] GameObject liquidStream;
    public Liquid containedLiquid;
    [SerializeField] float pourAngleThreshhold = 80f;
    public float pourRate = 10f; // mL per second
    [SerializeField] float maximumPourDistance = 1f;

    private GameObject activeStream;
    private RecievingLiquidContainer targetContainer;

    private LiquidVisual liquidVisual; // Reference to LiquidVisual for color
    private Renderer streamRenderer;

    public Material PouringStreamMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        liquidVisual = GetComponent<LiquidVisual>();
        if (liquidStream != null)
        {
            streamRenderer = liquidStream.GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

        if(tiltAngle > pourAngleThreshhold)
        {
            StartPouring();
        }
        else
        {
            StopPouring();
        }
    }

    void DetectContainer()
    {
        RaycastHit hit;
        if (Physics.Raycast(pourPoint.position, Vector3.down, out hit, maximumPourDistance))
        {
            targetContainer = hit.collider.GetComponentInParent<RecievingLiquidContainer>();
            Debug.DrawLine(pourPoint.position, hit.point, Color.green, maximumPourDistance); // Visualize the ray
            if (targetContainer != null)
            {
                Debug.Log("Detected RecievingLiquidContainer: " + targetContainer.name);
            }
            else
            {
                Debug.Log("No RecievingLiquidContainer detected.");
            }
        }
        else
        {
            targetContainer = null;
        }
        
    }

    void StartPouring()
    {
        if (containedLiquid == null || containedLiquid.amount <= 0)
        {
            StopPouring();
            return;
        }

        if (activeStream == null)
        {
            activeStream = Instantiate(liquidStream, pourPoint.position, Quaternion.identity);
            activeStream.transform.SetParent(pourPoint);
        }
        SetStreamColor();

        Pour();
    }

    private void SetStreamColor()
    {
        if (streamRenderer != null && liquidVisual != null)
        {
            streamRenderer.material.color = liquidVisual.GetColor();
        }
        else if (liquidVisual == null)
        {
            streamRenderer.material = PouringStreamMaterial;
        }
    }

    void Pour()
    {
        Debug.Log("Pouring");
        float pourAmount = pourRate * Time.deltaTime;
        if (containedLiquid.amount < pourAmount) pourAmount = containedLiquid.amount;

        Liquid pouringLiquid = new Liquid(containedLiquid.type, pourAmount);

        DetectContainer();

        if (targetContainer != null) targetContainer.ReceiveLiquid(pouringLiquid);

        containedLiquid.amount -= pourAmount;
        if (containedLiquid.amount <= 0) StopPouring();


        UpdateStreamPositionAndScale();

    }

    void StopPouring()
    {
        if (activeStream != null)
        {
            Destroy(activeStream);
        }
    }
    void UpdateStreamPositionAndScale()
    {

        activeStream.transform.localRotation = Quaternion.Inverse(pourPoint.rotation);

        RaycastHit hit;
        // Cast a ray downward from the pour point to find the nearest collider below
        if (Physics.Raycast(pourPoint.position, Vector3.down, out hit))
        {
            // Calculate the distance to the hit point
            float distanceToCollider = hit.distance;

            // Set the scale of the stream based on the distance to the collider
            Vector3 newScale = activeStream.transform.localScale;
            newScale.y = distanceToCollider;  // Adjust Y scale to stretch the stream
            activeStream.transform.localScale = newScale;

            // Update the position so that the top of the stream stays at the pour point
            activeStream.transform.position = pourPoint.position - new Vector3(0, distanceToCollider / 2, 0);
        }
    }
}

