"use client";

import Image from "next/image";
import logo from "../images/logo.svg";
import { useEffect, useState } from "react";

export default function Home() {
  const [facts, setFacts] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const res = (await fetch("http://localhost:3000/")) as any;

      const jsonData = await res.json();
      setFacts(jsonData.data.split("\n").filter((arr: any) => arr.length > 5));
      console.log(facts);
    };

    fetchData();
  }, []);

  return (
    <div className="md:container md:mx-auto">
      <div className="flex justify-center items-center text-2xl mt-12 mr-6">
        <div className="">
          <Image
            className="w-20 mx-2"
            src={logo}
            width={500}
            height={500}
            alt="Logo question mark"
          />
        </div>
        <div className=" mx-2">Did you know...?</div>
      </div>
      <div className="px-10 mt-5">
        {facts.map((fact) => (
          <div className="m-2" key={fact[0]}>
            {fact}
          </div>
        ))}
      </div>
      <div className="w-full flex flex-row-reverse">
        <button className="bg-sky-900 rounded-lg text-white py-3 px-4 mb-10 rounded m-5">
          Back to trivia
        </button>
      </div>
    </div>
  );
}
