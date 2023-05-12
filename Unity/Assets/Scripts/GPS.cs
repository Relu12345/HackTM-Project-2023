using System.Collections;
using System.Net;

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using Unity.VisualScripting;
using Gtec.Chain.Common.Nodes.FilterNodes;
using Newtonsoft.Json.Linq;

public class GPS : MonoBehaviour
{
    public static string city;
    public static string country;
    public Text gpsOut;
    public bool isUpdating;
    private void Update()
    {
        if (!isUpdating)
        {
            Debug.Log("in void update();");
            StartCoroutine(GetLocation());
            isUpdating = !isUpdating;
        }
    }
    IEnumerator GetLocation()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield return new WaitForSeconds(10);

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait == 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            gpsOut.text = "Timed out";
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            gpsOut.text = "Unable to determine device location";
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude);
            WebClient webClient = new WebClient();
            var link = "https://geocode.maps.co/reverse?lat=" + Input.location.lastData.latitude + "&lon=" + Input.location.lastData.longitude;
            var jsonData = new WebClient().DownloadString(link);
            var jsonD = JObject.Parse(jsonData);
            Debug.Log(jsonData.ToString());

            string json = JsonConvert.SerializeObject(jsonData);
            Debug.Log(json + "\ntest serialized json\n");
            var address = (JObject)jsonD["address"];
            var oras = (string)address["city"];
            var tara = (string)address["country"];

            city = oras;
            

            Debug.Log(city);
            Debug.Log(country);

            gpsOut.text = city + "\n" + country;    
        }
        

        // Stop service if there is no need to query location updates continuously
        isUpdating = !isUpdating;
        Input.location.Stop();
    }
}