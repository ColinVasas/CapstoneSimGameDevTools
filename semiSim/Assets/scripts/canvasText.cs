using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class canvasText : MonoBehaviour
{

     public GameObject[] textObjects;  
     public float displayTime = 2.0f;  
     public float delayBetweenTexts = .5f; 

     void Start()
     {

          foreach (GameObject textObject in textObjects)
          {
               textObject.SetActive(false);
          }

          StartCoroutine(StartUp(1));
          StartCoroutine(DisplayTextSequence());
     }

     IEnumerator StartUp(int i)
     {
          yield return new WaitForSeconds(i);
     }

     IEnumerator DisplayTextSequence()
     {
          foreach (GameObject textObject in textObjects)
          {
               textObject.SetActive(true); 
               yield return new WaitForSeconds(displayTime); 
               textObject.SetActive(false); 
               yield return new WaitForSeconds(delayBetweenTexts); 
          }
     }

}
