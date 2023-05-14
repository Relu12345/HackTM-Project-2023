using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UILogic : MonoBehaviour
{
    public delegate void ButtonPressedCallBack(uint answer);
    public delegate void SimpleCallBack();

    public VisualElement startPage;
    public VisualElement intrebare;
    public VisualElement corect;
    public VisualElement gresit;
    public VisualElement endPage;
    public VisualElement calibrare;
    public bool canSelect = false;

    public List<GameObject> lgam = new List<GameObject>(4);
    public List<UnityEngine.UI.Button> lbut = new List<UnityEngine.UI.Button>(4);

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
            uint ans = (uint)i+1; // We need this because otherwise C# will take the "i" by reference and all the callbacks will be called with 5
            lbut[i].onClick.RemoveAllListeners();
            lbut[i].onClick.AddListener(delegate ()
            {
                callBack.Invoke(ans);
            });
            lbut[i].GetComponentInChildren<Text>().text = Convert.ToChar(i + 97) + ") " + question.answers[i];
        }
        var textIntrebare = intrebare.Q<Label>("Label");
        textIntrebare.text = question.question;
        enableSelection();
    }

    private async void enableSelection()
    {
        await Task.Delay(1000);
        canSelect = true;
    }

    public void stopGame(int correct)
    {
        StartCoroutine(AnimateConfeti());
        StartCoroutine(MoveConfeti());
        endPage.Q<Label>("Answers").text = correct + "/5";
        endPage.Q<Label>("Points").text = (correct * 10).ToString();
        endPage.Q<UnityEngine.UIElements.Button>("RestartButton").clicked += () =>
        {
            SceneManager.LoadScene(0);
        };
        endPage.Q<UnityEngine.UIElements.Button>("FactsButton").clicked += () =>
        {
            Application.OpenURL("http://92.87.91.85:3001/");
        };
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
