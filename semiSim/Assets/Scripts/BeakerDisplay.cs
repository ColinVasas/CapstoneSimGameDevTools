using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeakerDisplay : MonoBehaviour
{
    private RecievingLiquidContainer recievingLiquidContainer;
    [SerializeField] private TextMeshProUGUI[] liquidAmounts;
    [SerializeField] private TextMeshProUGUI[] detectedMixtures;
    [SerializeField] private TextMeshProUGUI[] statuses;

    [SerializeField] private Canvas canvas; // <- assign your Canvas here
    [SerializeField] private Canvas screenCanvas;
    [SerializeField] private RectTransform screenCanvasPanel;
    private Vector3 screenCanvasPanelScale;
    private bool isLerping = false;
    private float lerpT = 0f;




    [SerializeField] float transitionSpeed = 5f; // how fast it moves (higher = faster)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        recievingLiquidContainer = GetComponent<RecievingLiquidContainer>();
        screenCanvasPanelScale = screenCanvasPanel.localScale;
     
      
    }


    // Update is called once per frame
    void Update()
    {
        if (recievingLiquidContainer.GetMixture() != null)
        {
            foreach (TextMeshProUGUI text in detectedMixtures)
                text.text = "Solution:" + recievingLiquidContainer.GetMixture().solutionName.ToString();
        }
        else
        {
            foreach (TextMeshProUGUI text in detectedMixtures)
                text.text = "Solution: ?";
        }

        foreach (TextMeshProUGUI text in liquidAmounts)
            text.text = (int)recievingLiquidContainer.GetLiquidLevel() + "ml / " + (int)recievingLiquidContainer.GetMaxCapacity() + "ml";
        Lerpscale();

    }

    private void Lerpscale()
    {
        if (isLerping)
        {
            lerpT += transitionSpeed * Time.deltaTime;
            lerpT = Mathf.Clamp01(lerpT); // keep between 0 and 1
            screenCanvasPanel.localScale = Vector3.Lerp(Vector3.zero, screenCanvasPanelScale, lerpT);

            if (lerpT >= 1f)
            {
                isLerping = false; // stop lerping once done
            }
        }
    }

    public void SwitchCanvasToWorldSpace()
    {
        Debug.Log("Switching to world space");
        screenCanvas.enabled = false;

    }

    public void SwitchCanvasToScreenSpace()
    {
        isLerping = true;
        lerpT = 0f;

        Debug.Log("Switching to screen space");
        screenCanvas.enabled = true;
    }

}
