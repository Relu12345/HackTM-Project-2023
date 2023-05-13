using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Main : MonoBehaviour
{

    [SerializeField]
    private AIController aIController;

    [SerializeField]
    private GPS gps;

    [SerializeField]
    private UILogic uILogic;

    private Parser.Question currentQuestion;
    public GameObject BCI, BCImgr;
    private int currQuest = 0;
    private int correct = 0;

    private GPS.Location currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        //aIController = GetComponent<AIController>();
        gps = GetComponent<GPS>();
        uILogic = GetComponent<UILogic>();

        gps.Init();
        //aIController.Init();
        uILogic.Init(StartButton, NextQuestion);
    }

    void StartButton()
    {
        // currentLocation = gps.GetLocation();
        currentLocation = new GPS.Location("Budapesta", "Hungary");

        if(currentLocation.success){
            SendGetRequest(currentLocation);
            //ToDo
            // Load all the questions here asyncron
            uILogic.ChangeScreen(uILogic.calibrare);
            BCI.SetActive(true);
            BCImgr.SetActive(true);
        }
        else{
            //Display error screen
        }
    }

    public async void StartQuiz()
    {
        currQuest = 1;
        BCI.SetActive(false);
        currentQuestion = Parser.parseQuestion("{#question#:#Used in ancient times by the poet Tibullus, The Eternal City is a nickname given to what European capital?#, #answers#:{#1#:#Venice#,#2#:#Tivoli#,#3#:#Rome#,#4#:#Siena#},#correct#:3}");

        // var r = await aIController.GetResponse(currentLocation.getFormated());

        // currentQuestion = Parser.parseQuestion(r);
        uILogic.DisplayQuestion(currentQuestion, buttonPressed);
        // Debug.Log(r);
       
    }

    async void NextQuestion()
    {
        if(currQuest < 5)
        {
            //var r = await aIController.GetResponse("Baia Mare, Romania");

            //currentQuestion = Parser.parseQuestion(r);
            uILogic.DisplayQuestion(currentQuestion, buttonPressed);
            currQuest++;
        }
        else
        {
            uILogic.ChangeScreen(uILogic.endPage);
            uILogic.stopGame(correct);

        }
        
    }

    void buttonPressed(int userAnswer)
    {
        if(userAnswer == currentQuestion.correctAnswer){
            uILogic.ChangeScreen(uILogic.corect);
            correct++;
            Debug.Log(correct);
        }
        else{
            uILogic.ChangeScreen(uILogic.gresit);
        }
    }

    private void SendGetRequest(GPS.Location location)
    {
        var request = UnityWebRequest.Get(String.Format("http://92.87.91.85:3000/buffer/{0}/{1}", location.city, location.country));
        request.SendWebRequest();
    }

}
