using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/*

{
  "question": "text",
  "answers": [
    "1": "rasp1", 
    "2": "rasp2", 
    "3": "rasp3", 
    "4": "rasp4"],
  "correct": 1/2/3/4
}

example:

{
"question": "In what year did Timisoara become the first European city with electric street lighting?",
"answers": {
"1": "1868",
"2": "1899",
"3": "1884",
"4": "1905"
},
"correct": 3
}

*/




public class Parser : MonoBehaviour
{
    public static Question parseQuestion(string aiResponse)
    {
        var jsonD = JObject.Parse(aiResponse);

        var question = (string)jsonD["question"];
        var answers = (JObject)jsonD["answers"];

        var answer1 = (string)answers["1"];
        var answer2 = (string)answers["2"];
        var answer3 = (string)answers["3"];
        var answer4 = (string)answers["4"];

        var correct = (int)jsonD["correct"];

        string[] answersList = new string[4]{
            answer1, answer2, answer3, answer4
        };

        return new Question(question, answersList, correct);
    }

    public class Question{
        public string question{get; }
        public string[] answers{get; }
        public int correctAnswer{get;}

        public Question(string _q, string[] _a, int _ca){
            question = _q;
            answers = _a;
            correctAnswer = _ca;
        }
    }
}
