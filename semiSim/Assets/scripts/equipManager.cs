using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class equipManager : MonoBehaviour
{
    public GameObject hairNet;
    public GameObject faceMask;
    public GameObject cleanHood;
    public GameObject goggles;
    public GameObject bunnySuit;
    public GameObject boots;
    public GameObject gloves;

    public GameObject trigger;
    public TextMeshProUGUI equipText; // Single TMP UI element for all messages
    public float fadeDuration = 0.5f;
    public float displayDuration = 3.0f;

    private Coroutine equipMessageCoroutine;
    private int currentMessageIndex = 0;

    private string[] equipMessages = new string[]
    {
        "Find and put on\nthe face mask",
        "Now for the clean hood",
        "Goggles are next",
        "Now for the hazmat suit",
        "Don't forget your boots",
        "Lastly, put on some gloves",
        "Alright. Now head to the door\nto enter the Yellow Room!"
    };

    private void Start()
    {
        equipText.gameObject.SetActive(false);
        Color textColor = equipText.color;
        textColor.a = 0f;
        equipText.color = textColor;

        hairNet.tag = "equip";
        faceMask.tag = "Untagged";
        cleanHood.tag = "Untagged";
        goggles.tag = "Untagged";
        bunnySuit.tag = "Untagged";
        boots.tag = "Untagged";
        gloves.tag = "Untagged";

        trigger.SetActive(false);
    }

    public void textDis()
    {
        StartCoroutine(DisplayTextMessage("hello this is my message"));
    }

    public void HandleEquip(GameObject equippedObject)
    {

        if (!equippedObject.CompareTag("equip")) return;

        switch (equippedObject)
        {
            case var obj when obj == hairNet:
                EquipItem(hairNet, faceMask);
                // DisplayTextMessage("hairnet equipped!");
                break;
            case var obj when obj == faceMask:
                EquipItem(faceMask, cleanHood);
                // DisplayTextMessage("facemask equipped!");
                break;
            case var obj when obj == cleanHood:
                EquipItem(cleanHood, goggles);
                // DisplayTextMessage("cleanhood equipped!");
                break;
            case var obj when obj == goggles:
                EquipItem(goggles, bunnySuit);
                // DisplayTextMessage("goggles equipped!");
                break;
            case var obj when obj == bunnySuit:
                EquipItem(bunnySuit, boots);
                // DisplayTextMessage("bunny suit equipped!");
                break;
            case var obj when obj == boots:
                EquipItem(boots, gloves);
                // DisplayTextMessage("boots equipped!");
                break;
            case var obj when obj == gloves:
                trigger.SetActive(true);
                EquipItem(gloves, null);
                // DisplayTextMessage("gloves equipped!");
                break;
            default:
                Debug.Log("Invalid equipment sequence.");
                break;
        }

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

    private void EquipItem(GameObject current, GameObject next)
    {
        current.SetActive(false);

        if (next != null) next.tag = "equip";

        //if (currentMessageIndex < equipMessages.Length)
        //{
        //     textDis();
        //     StartCoroutine(DisplayTextMessage(equipMessages[currentMessageIndex]));
        //     currentMessageIndex++;
        //}
    }

    private IEnumerator DisplayTextMessage(string message)
    {
        equipText.text = message;
        equipText.gameObject.SetActive(true);

        yield return FadeText(0f, 1f, fadeDuration); // Fade in
        yield return new WaitForSeconds(displayDuration);
        yield return FadeText(1f, 0f, fadeDuration); // Fade out

        equipText.gameObject.SetActive(false);
    }

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
}
