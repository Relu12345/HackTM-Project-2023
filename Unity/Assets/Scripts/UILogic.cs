using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UILogic : MonoBehaviour
{
    public delegate void ButtonPressedCallBack(int answer);
    public delegate void SimpleCallBack();

    public VisualElement startPage;
    public VisualElement intrebare;
    public VisualElement corect;
    public VisualElement gresit;
    public VisualElement calibrare;

    public List<GameObject> lgam = new List<GameObject>(4);
    public List<UnityEngine.UI.Button> lbut = new List<UnityEngine.UI.Button>(4);

    private VisualElement currentScreen;

    public void Init(SimpleCallBack startCallBack, SimpleCallBack nextCallBack)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startPage = root.Q("StartPage");
        intrebare = root.Q("IntrebareWindow");
        corect = root.Q("CorectWindow");
        gresit = root.Q("GresitWindow");
        calibrare = root.Q("CalibrareWindow");

        currentScreen = startPage;
        currentScreen.style.display = DisplayStyle.Flex;

        startPage.Q<UnityEngine.UIElements.Button>("StartButton").clicked += () =>
        {
            startCallBack.Invoke();
        };

        corect.Q<UnityEngine.UIElements.Button>("NextQuestion").clicked += () =>
        {
            nextCallBack.Invoke();
        };

        gresit.Q<UnityEngine.UIElements.Button>("NextQuestion").clicked += () =>
        {
            nextCallBack.Invoke();
        };
        
        Debug.Log("UI initialized");
    }

    public void DisplayQuestion(Parser.Question  question, ButtonPressedCallBack callBack)
    {
        ChangeScreen(intrebare);
        for(int i=0; i<4; i++){
            lbut[i].gameObject.SetActive(true);
            lgam[i].SetActive(true);
            int ans = i; // We need this because otherwise C# will take the "i" by reference and all the callbacks will be called with 5
            lbut[i].onClick.AddListener(delegate ()
            {
                callBack.Invoke(ans);
            });
            lbut[i].GetComponentInChildren<Text>().text = question.answers[i];
        }
        var textIntrebare = intrebare.Q<Label>("Label");
        textIntrebare.text = question.question;

    }

    public void ChangeScreen(VisualElement newScreen)
    {
        for(int i=0;i<4;i++)
        {
            lbut[i].gameObject.SetActive(false);
            lgam[i].SetActive(false);
        }
        currentScreen.style.display = DisplayStyle.None;
        newScreen.style.display = DisplayStyle.Flex;
        currentScreen = newScreen;
    }
}
