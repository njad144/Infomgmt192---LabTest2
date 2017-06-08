using UnityEngine;
using Pathfinding.Serialization.JsonFx; //make sure you include this using

public class Sketch : MonoBehaviour
{
	public GameObject myPrefab;
	//njad144 URL points to specified table
	string _WebsiteURL = "http://infomgmt192.azurewebsites.net/tables/" + "RevenueTest2" + "?zumo-api-version=2.0.0";

	void Start()
	{
		//Reguest.GET can be called passing in your ODATA url as a string in the form:
		//http://{Your Site Name}.azurewebsites.net/tables/{Your Table Name}?zumo-api-version=2.0.0
		//The response produce is a JSON string
		//        string Dataseat = "treesurveyv3";
		string jsonResponse = Request.GET(_WebsiteURL);

		//Just in case something went wrong with the request we check the reponse and exit if there is no response.
		if (string.IsNullOrEmpty(jsonResponse))
		{
			return;
		}

		//njad144 Deserialize into array of revenues
		RevenueTest2[] revenues = JsonReader.Deserialize<RevenueTest2[]>(jsonResponse);

		int totalCubes = Mathf.Min(10, revenues.Length);//revenues.Length;
		int totalDistance = 5;
		int i = 0;

		//njad144 Looping through all of the revenue objects to render new cubes.
		foreach (RevenueTest2 revenue in revenues)
		{
			float perc = i / (float)totalCubes;
			float x = perc * totalDistance; //njad144 Scales between totalDistances set
			float y = 5.0f;//Random.Range(3.0f, 5.0f);//5.0f;//revenue.Y;// 5.0f;
			float z = 0.0f;

			//Limit rendering to 10 cubes (totalCubes set to 10 if revenues.Length is greater than it)
			if (i < totalCubes)
			{
				GameObject newCube = (GameObject)Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
				newCube.GetComponent<myCubeScript>().setSize((1.0f - perc) * 2);
				newCube.GetComponent<myCubeScript>().ratateSpeed = perc;
				newCube.GetComponentInChildren<TextMesh>().text = revenue.City;

				//njad144 switch case which will set color of cube to green if urban category, blue if mix category, and defaults to white incase.
				switch (revenue.Category)
				{
					case "Urban":
						newCube.GetComponent<Renderer>().material.color = Color.green;
						break;
					case "Mix":
						newCube.GetComponent<Renderer>().material.color = Color.blue;
						break;
					default:
						newCube.GetComponent<Renderer>().material.color = Color.white;
						break;
				}
			}


			i++;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
