using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTESystem : MonoBehaviour
{


    public Text text;
    public Image image;
    public bool success;
    public bool isDone;

    public void onQTE()
    {
        isDone = false;
        
        success = false;
        image.fillAmount = 1;
        StartCoroutine(startQTE());
        
    }

    public bool CheckIfDone()
    {
        return isDone;
    }

    IEnumerator startQTE()
    {
        KeyCode key;
        int randomInput = Random.Range(1, 5);

        Debug.Log(randomInput);
        if (randomInput == 1)
        {
            text.text = "W";
            key = KeyCode.W;

        }
        else if (randomInput == 2)
        {
            text.text = "A";
            key = KeyCode.A;
        }
        else if (randomInput == 3)
        {
            text.text = "S";
            key = KeyCode.S;
        }
        else if (randomInput == 4)
        {
            text.text = "D";
            key = KeyCode.D;
        }
        else
        {
            key = KeyCode.None;
        }

        float time = 0;


        while (time < 3)
        {
            if (Input.GetKeyDown(key))
            {
                success = true;
                image.fillAmount = 1;
                image.color = Color.cyan;
                break;
            }
            time = time + Time.deltaTime;
            image.fillAmount = 1 - (time / 3);

            yield return null;
        }
        yield return new WaitForSeconds(1);
        isDone = true;

    }

    
}
