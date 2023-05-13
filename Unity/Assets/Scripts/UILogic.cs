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
    public VisualElement endPage;

    private VisualElement currentScreen;

    [SerializeField]
    public Sprite[] confetiAnim;
    private int currentConfetiIndex = 0;

    public void Init(SimpleCallBack startCallBack, SimpleCallBack nextCallBack)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startPage = root.Q("StartPage");
        intrebare = root.Q("IntrebareWindow");
        corect = root.Q("CorectWindow");
        gresit = root.Q("GresitWindow");
        endPage = root.Q("FinishPage");

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

        if(currentScreen == endPage){
            StartCoroutine(AnimateConfeti());
            StartCoroutine(MoveConfeti());
        }
    }

    private IEnumerator AnimateConfeti()
    {
        VisualElement confeti1 = endPage.Q("confeti1");
        VisualElement confeti2 = endPage.Q("confeti2");

        foreach(var sprite in confetiAnim){
            confeti1.style.backgroundImage = new StyleBackground(sprite);
            confeti2.style.backgroundImage = new StyleBackground(sprite);

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator MoveConfeti()
    {
        VisualElement confeti1 = endPage.Q("confeti1");
        VisualElement confeti2 = endPage.Q("confeti2");

        float posC1B = 160f;

        float posC2B = 155f;

        while(posC1B >= 0f && posC2B >= 0f){
            confeti1.style.bottom = posC1B;
            posC1B -= 113*Time.deltaTime;

            confeti2.style.bottom = posC2B;
            posC2B -= 105*Time.deltaTime;
            yield return null;
        }
    }
}
