using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UILogic : MonoBehaviour
{
    public delegate void ButtonPressedCallBack(int answer);
    public delegate void SimpleCallBack();

    public VisualElement startPage;
    public VisualElement intrebare;
    public VisualElement corect;
    public VisualElement gresit;

    private VisualElement currentScreen;

    public void Init(SimpleCallBack startCallBack, SimpleCallBack nextCallBack)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startPage = root.Q("StartPage");
        intrebare = root.Q("IntrebareWindow");
        corect = root.Q("CorectWindow");
        gresit = root.Q("GresitWindow");

        currentScreen = startPage;
        currentScreen.style.display = DisplayStyle.Flex;

        startPage.Q<Button>("StartButton").clicked += () =>
        {
            startCallBack.Invoke();
        };

        corect.Q<Button>("NextQuestion").clicked += () =>
        {
            nextCallBack.Invoke();
        };

        gresit.Q<Button>("NextQuestion").clicked += () =>
        {
            nextCallBack.Invoke();
        };
        
        Debug.Log("UI initialized");
    }

    public void DisplayQuestion(Parser.Question  question, ButtonPressedCallBack callBack)
    {
        ChangeScreen(intrebare);
        for(int i=1; i<=4; i++){
            var button = intrebare.Q<Button>("Button"+i.ToString());
            int ans = i; // We need this because otherwise C# will take the "i" by reference and all the callbacks will be called with 5
            button.clicked += () =>
            {
                callBack.Invoke(ans);
            };
            button.text = question.answers[i-1];
        }
        var textIntrebare = intrebare.Q<Label>("Label");
        textIntrebare.text = question.question;
    }

    public void ChangeScreen(VisualElement newScreen)
    {
        currentScreen.style.display = DisplayStyle.None;
        newScreen.style.display = DisplayStyle.Flex;
        currentScreen = newScreen;
    }
}
