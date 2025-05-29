using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class equipManager : MonoBehaviour
{
    // these GameObjects are the equippable objects
    public GameObject hairNet;
    public GameObject faceMask;
    public GameObject cleanHood;
    public GameObject goggles;
    public GameObject bunnySuit;
    public GameObject boots;
    public GameObject gloves;

    public GameObject trigger;

    // some variables for text display
    public TextMeshProUGUI equipText; // Single TMP UI element for all messages
    public float fadeDuration = 0.5f;
    public float displayDuration = 3.0f;
    private Coroutine equipMessageCoroutine;
    private int currentMessageIndex = 0;

    // messages that tell the user what order to equip objects
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

    public TaskListUI taskListUI; //Reference TaskListUI script through UI manager

    private void Start()
    {
        equipText.gameObject.SetActive(false);
        Color textColor = equipText.color;
        textColor.a = 0f;
        equipText.color = textColor;

        // set up the equip tag(s)
        hairNet.tag = "equip";
        faceMask.tag = "Untagged";
        cleanHood.tag = "Untagged";
        goggles.tag = "Untagged";
        bunnySuit.tag = "Untagged";
        boots.tag = "Untagged";
        gloves.tag = "Untagged";

        trigger.SetActive(false);

        // Add initial tasks to the task list
        taskListUI.AddTask("Wear HairNet");
        taskListUI.AddTask("Equip Face Mask");
        taskListUI.AddTask("Put on Clean Hood");
        taskListUI.AddTask("Wear Goggles");
        taskListUI.AddTask("Put on Bunny Suit");
        taskListUI.AddTask("Wear Boots");
        taskListUI.AddTask("Put on Gloves");
    }

    // this function was for testing purposes only
    public void textDis()
    {
        StartCoroutine(DisplayTextMessage("hello this is my message"));
    }

    public void HandleEquip(GameObject equippedObject)
    {
        // only equip the item if it's in the correct order
        if (!equippedObject.CompareTag("equip")) return;

        switch (equippedObject)
        {
            case var obj when obj == hairNet:
                EquipItem(hairNet, faceMask, "Wear HairNet");
                break;
            case var obj when obj == faceMask:
                EquipItem(faceMask, cleanHood, "Equip Face Mask");
                break;
            case var obj when obj == cleanHood:
                EquipItem(cleanHood, goggles, "Put on Clean Hood");
                break;
            case var obj when obj == goggles:
                EquipItem(goggles, bunnySuit, "Wear Goggles");
                break;
            case var obj when obj == bunnySuit:
                EquipItem(bunnySuit, boots, "Put on Bunny Suit");
                break;
            case var obj when obj == boots:
                EquipItem(boots, gloves, "Wear Boots");
                break;
            case var obj when obj == gloves:
                trigger.SetActive(true);
                EquipItem(gloves, null, "Put on Gloves");
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

    private void EquipItem(GameObject current, GameObject next, string taskName)
    {
        current.SetActive(false);

        // manage equip tag
        if (next != null) next.tag = "equip";

        //if (currentMessageIndex < equipMessages.Length)
        //{
        //     textDis();
        //     StartCoroutine(DisplayTextMessage(equipMessages[currentMessageIndex]));
        //     currentMessageIndex++;
        //}

        // Mark the task as completed in the task list
        taskListUI.CompleteTask(taskName);
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