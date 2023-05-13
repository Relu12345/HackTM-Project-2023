using System.Collections;
using System.Net;
using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using Unity.VisualScripting;
using Gtec.Chain.Common.Nodes.FilterNodes;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public class GPS : MonoBehaviour
{
    private bool initialized = false;

    private readonly int ms_in_sec = 1000;
    private readonly int locationEnabledTimeout = 10000;
    private readonly int maxInitializationWait = 10;

    public async void Init()
    {
        initialized = false;

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser){
            await Task.Delay(locationEnabledTimeout);
        }

        Input.location.Start();

        // Wait until service initializes
        int i=0;
        while (Input.location.status == LocationServiceStatus.Initializing && i < maxInitializationWait)
        {
            await Task.Delay(ms_in_sec);
            i++;
        }

        // Service didn't initialize
        if (i >= maxInitializationWait)
        {
            Debug.Log("GPS initialization timed out");
            return;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            return;
        }

        initialized = true;
        Debug.Log("GPS initialized");
    }

    public Location GetLocation()
    {
        if(!initialized){
            return new Location();
        }

        if(Input.location.status == LocationServiceStatus.Failed){
            return new Location();
        }

        var link = "https://geocode.maps.co/reverse?lat=" + Input.location.lastData.latitude + "&lon=" + Input.location.lastData.longitude;
        var jsonData = new WebClient().DownloadString(link);
        var jsonD = JObject.Parse(jsonData);

        var address = (JObject)jsonD["address"];
        var oras = (string)address["city"];
        var tara = (string)address["country"];

        return new Location(oras, tara);
    }

    public class Location{
        public bool success{get;}
        public string city{get;}
        public string country{get;}

        public Location(){
            success = false;
        }

        public Location(string _city, string _country){
            city = _city;
            country = _country;
            success = true;
        }

        public string getFormated(){
            return String.Format("{0}, {1}", city, country);
        }
    }
}