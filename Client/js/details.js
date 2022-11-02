;(function () 
{
	
	'use strict';



	  //==================================================//
     // ================== Functions =================== //
    //==================================================//

		const getCityDetails = async () => {

			let url = new URL(window.location);
			let params = new URLSearchParams(url.search);
			console.log(params);
			var cityid = params.get("id");
			const response = await fetch(`https://localhost:44302/api/cities/${cityid}`);
			const data = response.json();
			return data;
		};




		const displayOption = async () => {

				var options = await getCityDetails();


				var regionid = document.getElementById("regionid");
				regionid.innerHTML = "סמל נפה:  "+ options[0].regionId;

				var regionname = document.getElementById("regionname");
				regionname.innerHTML = "שם נפה: "+options[0].regionName;


				var subRegionId = document.getElementById("subRegionId");
				subRegionId.innerHTML = "סמל לשכה: "+ options[0].subRegionId;

				var subRegionName = document.getElementById("subRegionName");
				subRegionName.innerHTML = "שם לשכה: "+ options[0].subRegionName;


				var regionalCouncilId = document.getElementById("regionalCouncilId");
				regionalCouncilId.innerHTML = "סמל מועצה: "+ options[0].regionalCouncilId;



				var regionalCouncilIdName = document.getElementById("regionalCouncilIdName");
				regionalCouncilIdName.innerHTML = "שם מועצה: "+options[0].regionalCouncilIdName;


				var NumOfCities = document.getElementById("NumOfCities");
				NumOfCities.innerHTML = options[0].streetCount;


				var cityname = document.getElementById("cityname");
				cityname.innerHTML =  options[0].hebName;


		};



		displayOption();

                




}());




