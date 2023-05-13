using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Threading.Tasks;

public class AIController : MonoBehaviour
{

    private OpenAIAPI api;

    private Conversation chat;

    // ToDo remove this from here
    private readonly string api_key = "sarac ia-ti si tu cheie"; 
    
    private readonly string instructions = @"
    I need 1 trivia question about the city of X located in Y. I want it with 4 possible answers. Out of the 4 answares only 1 correct. The answers will have a maximum of 5 words. The question will also have a maximum of 30 words. The question and the answer can only be written in ENGLISH. This will be the format in which you will give the question with the answers:
'
{#question#: #Question PlaceHolder#,#answers#: {#1#:#Answer1#,#2#: #Answer2#,#3#: #Answer3#,#4#: #Answer4#},#correct#: 3}
'";

    private readonly List<AIExamples> examples = new List<AIExamples>(){
        new AIExamples("Roma, Italy", "{#question#:#Used in ancient times by the poet Tibullus, The Eternal City is a nickname given to what European capital?#, #answers#:{#1#:#Venice#,#2#:#Tivoli#,#3#:#Rome#,#4#:#Siena#},#correct#:3}"),
        new AIExamples("Barcelona, Spain", "{#question#:#Barcelona held the Summer Olympics in which year?#, #answers#:{#1#:#1950#,#2#:#1992#,#3#:#2015#,#4#:#2002#},#correct#:2}"),
        new AIExamples("Busan, South Korea", "{#question#: #Which is the second most populated city in South Korea?#,#answers#: {#1#:#Pyeongchang#,#2#: #Seoul#,#3#: #Daegu#,#4#: #Busan#},#correct#: 4}"),
        new AIExamples("Cairo, Egipt", "{#question#: #Resembling an inverted triangle or a flower with a stem on maps, the Nile River Delta is located just north of which African capital city?#,#answers#: {#1#:#Cairo#,#2#: #Algers#,#3#: #Tunis#,#4#: #Rabat#},#correct#: 1}"),
        new AIExamples("Roma, Italy", "{#question#: #In what European capital city would you find the landmark known as the Spanish Steps?#,#answers#: {#1#:#Istambul#,#2#: #Madrid#,#3#: #Milano#,#4#: #Rome#},#correct#: 4}"),
        new AIExamples("Seattle, United States", "{#question#: #I-90, the longest interstate highway in the United States, has its termini in Boston and what west coast city (which is NOT its state's capital)?#,#answers#: {#1#:#Ontario#,#2#: #Seattle#,#3#: #Corona#,#4#: #Santa Maria#},#correct#: 2}")
    };

    public void Init()
    {
        api = new OpenAIAPI(api_key);

        initConversation();
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

        Debug.Log("AI initialized");
    }

    public async Task<string> GetResponse(string input)
    {
        Debug.Log("Send: " + input);

        chat.AppendUserInput(input);
        // and get the response
        string response = await chat.GetResponseFromChatbot();
        Debug.Log(response);
        return response;
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
