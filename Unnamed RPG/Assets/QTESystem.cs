using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTESystem : MonoBehaviour
{

    
    public Text text;

    public void onQTE()
    {
        StartCoroutine(StartTimer(3));
        StartCoroutine(startQTE());
    }

    IEnumerator startQTE()
    {
        int randomInput = Random.Range(1, 5);
        if (randomInput == 1)
        {
            text.text = "W";
        }
        else if (randomInput == 2)
        {
            text.text = "A";
        }
        else if (randomInput == 3)
        {
            text.text = "S";
        }
        else if (randomInput == 4)
        {
            text.text = "D";
        }

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }

    IEnumerator StartTimer(float duration)
    {
        float time = 0;

        while(time < duration)
        {
            time = time + Time.deltaTime;
            gameObject.GetComponent<Image>().fillAmount = 1 - (time / duration);
            yield return null;
        }
    }
}
