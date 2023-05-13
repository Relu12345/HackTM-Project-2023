import { openai } from "./openai";
import { prompt } from "./prompt";

export const chatResponse = async (oras: string, tara: string) => {
    const response = await openai.createCompletion({
    model: "text-davinci-003",
    prompt: prompt(oras, tara),
    // temperature: 0,
    // max_tokens: 7,
    max_tokens: 300,
    // n: 10  
  })
  console.log(response.data.choices[0].text);
  return response.data.choices[0].text;
  // return "\\n\\n1. Timisoara is the capital city of Timis County, Romania. :\\n\\n2. Called the City of Flowers, it is the third largest city in Romania. :\\n\\n3. It is known for its exciting nightlife, large pedestrian-friendly squares and impressive baroque architecture. :\\n\\n4. Three separate cultures inhabit the city; Hungarian, German and Romanian. :\\n\\n5. It was the first city in Europe to have installed electric street lighting in 1884. :\\n\\n6. The main source of income for the city is the automotive industry. :\\n\\n7. The city is home to the Timisoara Orthodox Cathedral which was built in 1751. :\\n\\n8. It has a humid continental climate, with cold winters and hot summers. :\\n\\n9. It is home to the country’s third largest airport, Traian Vuia International Airport. :\\n\\n10. The metropolitan area of Timisoara is home to a population of over 859 thousand people. :";
}