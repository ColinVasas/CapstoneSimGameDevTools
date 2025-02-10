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

    private GameObject activeStream;
    private RecievingLiquidContainer targetContainer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

        if(tiltAngle > pourAngleThreshhold)
        {
            DetectContainer();
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
        if (Physics.Raycast(pourPoint.position, Vector3.down, out hit, 0.5f))
        {
            targetContainer = hit.collider.GetComponent<RecievingLiquidContainer>();
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

        Pour();
    }

    void Pour()
    {
        float pourAmount = pourRate * Time.deltaTime;
        if (containedLiquid.amount < pourAmount) pourAmount = containedLiquid.amount;

        Liquid pouringLiquid = new Liquid(containedLiquid.type, pourAmount);
        targetContainer.ReceiveLiquid(pouringLiquid);

        containedLiquid.amount -= pourAmount;
        if (containedLiquid.amount <= 0) StopPouring();
    }

    void StopPouring()
    {
        if (activeStream != null)
        {
            Destroy(activeStream);
        }
    }
}

