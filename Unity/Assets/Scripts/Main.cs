using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{

    [SerializeField]
    private AIController aIController;

    [SerializeField]
    private GPS gps;

    [SerializeField]
    private UILogic uILogic;

    private Parser.Question currentQuestion;
    public GameObject BCI, BCImgr, loadingScreen, simpleEventSystem;
    private int currQuest = 0;
    private int correct = 0;

    private string[] questions;
    private GPS.Location currentLocation;
    private bool questionsInitialized = false;

    public static bool SimpleMode = false;

    // Start is called before the first frame update
    void Start()
    {
        aIController = GetComponent<AIController>();
        gps = GetComponent<GPS>();
        uILogic = GetComponent<UILogic>();

        gps.Init();
        aIController.Init();
        uILogic.Init(StartButton, StartBCIButton, NextQuestion);
    }

    void StartBCIButton()
    {
#if UNITY_EDITOR
        currentLocation = new GPS.Location("Timisoara", "Romania");
#else
        currentLocation = gps.GetLocation();
#endif
        

        if(currentLocation.success){
            LoadMessages(false);
            // SendGetRequest(currentLocation);
            SimpleMode = false;
            uILogic.ChangeScreen(uILogic.calibrare);
            simpleEventSystem.SetActive(false);
            BCI.SetActive(true);
            BCImgr.SetActive(true);
        }
        else{
            uILogic.ChangeScreen(uILogic.errorPage);
        }
    }

    void StartButton()
    {
#if UNITY_EDITOR
        currentLocation = new GPS.Location("Timisoara", "Romania");
#else
        currentLocation = gps.GetLocation();
#endif

        if(currentLocation.success){
            loadingScreen.SetActive(true);
            simpleEventSystem.SetActive(true);
            LoadMessages(true);
            SimpleMode = true;
            // SendGetRequest(currentLocation);
        }
        else{
            // uILogic.ChangeScreen(uILogic.errorPage);
        }
    }

    private async void LoadMessages(bool startQuiz)
    {
        questions = new string[5];
        for(int i=0; i<2;i++){
            questions[i] = await aIController.GetResponse(currentLocation.getFormated()); 
        }
        questionsInitialized = true;

        if(startQuiz){
            loadingScreen.SetActive(false);
            StartQuiz();
        }

        await Task.Delay(61000);
        for(int i=2; i<5;i++){
            questions[i] = await aIController.GetResponse(currentLocation.getFormated()); 
        }
    }

    public async void StartQuiz()
    {
        int attempts = 100;
        while(!questionsInitialized && attempts-- > 0){
            await Task.Delay(100);
        }

        if(!questionsInitialized){
            uILogic.ChangeScreen(uILogic.errorPage);
        }

        currQuest = 1;
        BCI.SetActive(false);
        Debug.Log("Parse");
        currentQuestion = Parser.parseQuestion(questions[currQuest-1]);
        uILogic.DisplayQuestion(currentQuestion, buttonPressed);
        uILogic.UpdateDots(currQuest);
        // Debug.Log(r);
       
    }

    async void NextQuestion()
    {
        if(currQuest < 5)
        {
            currQuest++;
            currentQuestion = Parser.parseQuestion(questions[currQuest-1]);
            uILogic.DisplayQuestion(currentQuestion, buttonPressed);
            uILogic.UpdateDots(currQuest);
            BCImgr.SetActive(true);
        }
        else
        {
            uILogic.ChangeScreen(uILogic.endPage);
            uILogic.stopGame(correct);

        }
        
    }

    public void buttonPressed(uint userAnswer)
    {
        if (!uILogic.canSelect)
        {
            return;
        }
        uILogic.canSelect = false;
        if(userAnswer == currentQuestion.correctAnswer){
            uILogic.ChangeScreen(uILogic.corect);
            uILogic.corect.Q<Label>("Value").text = Convert.ToChar(currentQuestion.correctAnswer -1 + 97) + ") " + currentQuestion.answers[currentQuestion.correctAnswer - 1];
            correct++;
            Debug.Log(correct);
        }
        else{
            uILogic.ChangeScreen(uILogic.gresit);
            uILogic.gresit.Q<Label>("Value").text = "Correct: " + Convert.ToChar(currentQuestion.correctAnswer - 1 + 97) + ") " + currentQuestion.answers[currentQuestion.correctAnswer - 1];
            uILogic.gresit.Q<Label>("WValue").text = "Yours: " + Convert.ToChar(userAnswer- 1 + 97) + ") " + currentQuestion.answers[userAnswer - 1];
        }
        BCImgr.SetActive(false);
    }

    private void SendGetRequest(GPS.Location location)
    {
        var request = UnityWebRequest.Get(String.Format("http://92.87.91.85:3000/buffer/{0}/{1}", location.city, location.country));
        request.SendWebRequest();
    }

}
