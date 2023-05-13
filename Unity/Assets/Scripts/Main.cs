using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField]
    private AIController aIController;

    [SerializeField]
    private GPS gps;

    [SerializeField]
    private UILogic uILogic;

    private Parser.Question currentQuestion;

    // Start is called before the first frame update
    void Start()
    {
        // aIController = GetComponent<AIController>();
        gps = GetComponent<GPS>();
        uILogic = GetComponent<UILogic>();

        gps.Init();
        // aIController.Init();
        uILogic.Init(StartButton, NextQuestion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartButton()
    {
        currentQuestion = Parser.parseQuestion("{\"question\": \"In what year did Timisoara become the first European city with electric street lighting?\",\"answers\": {\"1\": \"1868\",\"2\": \"1899\",\"3\": \"1884\",\"4\": \"1905\"},\"correct\": 3}");

        uILogic.DisplayQuestion(currentQuestion, buttonPressed);
    }

    void NextQuestion()
    {
        uILogic.DisplayQuestion(currentQuestion, buttonPressed);
    }

    void buttonPressed(int userAnswer)
    {
        if(userAnswer == currentQuestion.correctAnswer){
            uILogic.ChangeScreen(uILogic.corect);
        }
        else{
            uILogic.ChangeScreen(uILogic.gresit);
        }
    }

}
