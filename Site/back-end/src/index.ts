import express, { Express, Request, Response } from 'express';
import dotenv from 'dotenv';
import { openai } from './openai';
import { prompt } from './prompt';
import cors from "cors";
import {chatResponse} from './getResponse'
dotenv.config();

const app: Express = express();
const port = process.env.PORT;

const corsOption = {
  origin: 'http://localhost:3001',
  optionsSuccessStatus: 200
}

let buf = "";

app.use(cors(corsOption));

app.get('/', async(req: Request, res: Response) => {



  // const response = await openai.createCompletion({
  //   model: "text-davinci-003",
  //   prompt: prompt("Timisoara", "Romania"),
  //   // temperature: 0,
  //   // max_tokens: 7,
  //   max_tokens: 300,
  //   // n: 10  
  // })

  // res.json({code: 200, data: JSON.stringify(response.data.choices[0].text)});
  res.json({code: 200, data: buf});
});

app.get('/buffer/:oras/:tara', async(req: Request, res: Response) => {

  const response = await chatResponse(req.params.oras, req.params.tara);
  if(typeof response == 'string') buf = response;
  else buf = 'Error while fetching response from ChatGPT'
  res.send({code: 200})
})


app.get('/test', async(req: Request, res: Response) => {
  buf = "Asta este un text modificat de buffer";
  res.send({code: 200})
})
app.listen(port, () => {
  console.log(`⚡️[server]: Server is running at http://localhost:${port}`);
});