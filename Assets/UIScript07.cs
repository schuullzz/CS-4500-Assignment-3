using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIScript07 : MonoBehaviour
{
    public GameObject[] questionGroupArray;
    public QAClass07[] qaArray;
    public GameObject AnswerPanel;

    // Start is called before the first frame update
    void Start()
    {
        qaArray = new QAClass07[questionGroupArray.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitAnswer()
    {
        for(int i = 0; i < qaArray.Length; i++)
        {
            qaArray[i] = ReadQuestionAndAnswer(questionGroupArray[i]);
        }

        DisplayResult();
    }

    QAClass07 ReadQuestionAndAnswer(GameObject questionGroup)
    {
        QAClass07 result = new QAClass07();

        GameObject q = questionGroup.transform.Find("Question").gameObject;
        GameObject a = questionGroup.transform.Find("Answer").gameObject;

        result.Question = q.GetComponent<Text>().text;

        if (a.GetComponent<ToggleGroup>() != null)
        {
            for (int i = 0; i < a.transform.childCount; i++)
            {
                if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    result.Answer = a.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
                    break;
                }
            }
        }

        else if (a.GetComponent<InputField>() != null)
        {
            result.Answer = a.transform.Find("Text").GetComponent<Text>().text;
        }

        else if (a.GetComponent<ToggleGroup>() == null && a.GetComponent<InputField>() == null)
        {
            string s = "";
            int counter = 0;

            for (int i = 0; i < a.transform.childCount; i++)
            {
                if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    if (counter != 0)
                    {
                        s = s + ", ";
                    }

                    s = s + a.transform.GetChild(i).Find("Label").GetComponent<Text>().text;

                    counter++;
                }

                if (i == a.transform.childCount - 1)
                {
                    s = s + ".";
                }
            }

            result.Answer = s;
        }

        return result;
    }

    void DisplayResult()
    {
        AnswerPanel.SetActive(true);

        string s = "";

        for (int i = 0; i < qaArray.Length; i++)
        {
            s = s + qaArray[i].Question + "\n";
            s = s + qaArray[i].Answer + "\n\n";
        }

        AnswerPanel.transform.Find("Answer").GetComponent<Text>().text = s;
    }
}

[System.Serializable]
public class QAClass07
{
    public string Question = "";
    public string Answer = "";

}