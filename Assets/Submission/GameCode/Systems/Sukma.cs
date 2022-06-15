using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sukma : MonoBehaviour
{
    // 1. Generate random action button
    // 2. Sets timer
    // 3. Calculate how many is correct
    // 4. Get total demage = percentage correct * initiator.dmg

    // components
    [SerializeField]
    TMPro.TextMeshProUGUI[] objs;// btn template length
    [SerializeField]
    Image imgSlider;
    [SerializeField]
    GameObject endTxt;

    // vars
    [SerializeField]
    [Tooltip("This is the button info to press")]
    char[] txts;
    char[] inputNeed;
    GameObject[] wrongs;
    int i = 0;
    int correctVal;
    int counter;
    bool isWaitInput;


    static bool isRandomized;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        wrongs = new GameObject[objs.Length];
        inputNeed = new char[wrongs.Length];
        InitWrongs();
        isRandomized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRandomized) {
            RandomizeNow();
            correctVal = 0;
            isWaitInput = true;
            isRandomized = false;
            imgSlider.fillAmount = 1f;
        }

        WaitInput();
    }

    
    void InitWrongs() {
        i = 0;
        while (i < objs.Length) {
            wrongs[i] = objs[i].transform.parent.GetChild(1).gameObject;
            wrongs[i].SetActive(false);
            i++;
        }
    }

    public static void StartSukmaProject() {
        isRandomized = true;
    }

    char GetChar(int indexChar) {
        return txts[indexChar];
    }

    void RandomizeNow() {
        i = 0;
        while (i < objs.Length) {
            wrongs[i].SetActive(false);
            inputNeed[i] = GetChar(Random.Range(0, txts.Length));
            objs[i].SetText(inputNeed[i].ToString());
            i++;
        }
    }

    void WaitInput() {
        if (isWaitInput) {
            if (imgSlider.fillAmount > 0f)
            {
                imgSlider.fillAmount -= .5f * Time.deltaTime;
                // listens to input, validate if done
                InputReader();
            }
            else {
                // validate how many correct
                ValidateOnEnd();
                isWaitInput = false;
            }
        }
    }

    void ValidateOnEnd() {
        isWaitInput = false;
        GameSystem.sukmaCorrect = correctVal;
        endTxt.SetActive(true);
        if (correctVal > 4)
        {
            endTxt.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText("Perfect!");
        }
        else { 
            endTxt.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText("Good");
        }
        StartCoroutine(StartHiding(2f));
    }

    IEnumerator StartHiding(float timers) {
        Debug.Log("First time call "+Time.realtimeSinceStartup);
        yield return new WaitForSeconds(timers);
        Debug.Log("End time call " + Time.realtimeSinceStartup);
        gameObject.SetActive(false);
        endTxt.SetActive(false);
    }

    void InputReader() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (inputNeed[counter] == 'Z')
            {
                correctVal++;
            }
            else { 
                wrongs[counter].SetActive(true);
            }
            counter++;

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (inputNeed[counter] == 'X')
            {
                correctVal++;
            }
            else
            {
                wrongs[counter].SetActive(true);
            }
            counter++;

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (inputNeed[counter] == 'C')
            {
                correctVal++;
            }
            else
            {
                wrongs[counter].SetActive(true);
            }
            counter++;

        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            if (inputNeed[counter] == 'V')
            {
                correctVal++;
            }
            else
            {
                wrongs[counter].SetActive(true);
            }
            counter++;

        }
        if (counter > inputNeed.Length-1) {
            Debug.Log("input done");
            isWaitInput = false;
            ValidateOnEnd();
        }
    }
}
