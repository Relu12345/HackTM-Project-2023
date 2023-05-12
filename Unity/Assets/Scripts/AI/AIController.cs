using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

public class AIController : MonoBehaviour
{

    private OpenAIAPI api;

    private Conversation chat;

    // ToDo remove this from here
    private readonly string api_key = "valid_key"; 
    
    private readonly string instructions = "You are a teacher who prepare a geograpy test for your students. If the user tells you the name of a city, and a country you will provide him a question about that city.";

    private readonly List<AIExamples> examples = new List<AIExamples>(){
        new AIExamples("Timisoara, Romania", "What is the population of Timisoara?"),
        new AIExamples("Oras, Romania", "Intrebare"),
        new AIExamples("Third, Example", "Intrebare #3")
    };

    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI(api_key);

        initConversation();

        // SendMessage("Arad, Romania");
    }

    private void initConversation()
    {
        chat = api.Chat.CreateConversation();

        /// give instruction
        chat.AppendSystemMessage(instructions);

        // give a few examples
        foreach (AIExamples example in examples)
        {
            chat.AppendUserInput(example.input);
            chat.AppendExampleChatbotOutput(example.output);
        }
    }

    public async void SendMessage(string input)
    {
        Debug.Log("Send: " + input);

        chat.AppendUserInput(input);
        // and get the response
        string response = await chat.GetResponseFromChatbot();
        Debug.Log(response);
    }

    private async void GetResponse()
    {

    }

    private class AIExamples{
        public string input{get;}
        public string output{get;}
        public AIExamples(string _input, string _output){
            input = _input;
            output = _output;
        }
    }
}
