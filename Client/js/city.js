;(function () 
{
	
	'use strict';

    // ============ Variables ============ //

    var citiesid=[];
    var container = document.querySelector('#container-cities');



    //=================================================//
    // ================== Functions ================== //
    //=================================================//


    const getCities = async () => {
        const response = await fetch("https://localhost:44302/api/cities");
        const data = response.json();
        return data;
    };

    const getCitiesByStreet = async (street) => {
        const response = await fetch(`https://localhost:44302/api/cities/street/${street}`);
        const data = response.json(); 
        return data;
    };

    
    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

 

    const CreateCityCard = async (options='' , source='') => {


        var container = document.getElementById("container-cities");
        if(source==''){
            var options = await getCities();
        }
      
        options.sort((a,b) => (a.hebName > b.hebName) ? 1 : ((b.hebName > a.hebName) ? -1 : 0))
        options.forEach(renderCitiesList);
        
    };


    function renderCitiesList(element, index, arr) {

        citiesid.push(element.id);

        // div container
        var div = document.createElement("div");
        div.setAttribute('id',`city-card-${element.id}`);
        div.setAttribute('class','city-card');

        container.appendChild(div);            

        // header of the card
        var h1 = document.createElement('h1');
        h1.setAttribute('id',`city-header-${element.id}`);
        h1.setAttribute('class','city-header');
        h1.innerText=element.hebName;
        document.getElementById(`city-card-${element.id}`).appendChild(h1);

        // details of the card
        var ul = document.createElement('ul');
        ul.setAttribute('id',`ul-city-card-${element.id}`);
        document.getElementById(`city-card-${element.id}`).appendChild(ul);
        
        // city id
        var li = document.createElement('li');
        li.setAttribute('class','cityid');
        ul.appendChild(li);
        li.innerHTML= "מזהה עיר:  " + element.id;  //  li.innerHTML +

        // name city heb
        var li = document.createElement('li');
        li.setAttribute('class','heb');
        ul.appendChild(li);
        li.innerHTML= "שם עיר:  "+element.hebName;  

        // name city english
        var li = document.createElement('li');
        li.setAttribute('class','eng');
        ul.appendChild(li);
        li.innerHTML= "שם עיר (לועזי):  " +element.engName;  

        // button details
        var button = document.createElement('button');
        button.setAttribute('id',`city-button-${element.id}`);
        button.setAttribute('class','button-city-link');
        button.innerText= "פרטים";
        document.getElementById(`city-card-${element.id}`).appendChild(button);

    }


   

    //===================================================  //
    // =============== Events Listeners    ============= //
    //=================================================//


    const input = document.querySelector('input');
    input.addEventListener('input', updateValue);
    
    async function updateValue(e)  {

 


        if(e.target.value==''){

            sleep(250).then(() => {
            var citieshidden = document.getElementsByClassName("hide"); 
            for (var i = 0 ; i < citieshidden.length; i++) {

                citieshidden[i].classList.toggle("hide", false);
            };
        });
  
        }

        else{
        
                var data =  await getCitiesByStreet(e.target.value);

                var difference = citiesid.filter(x => !data.includes(x));

                difference.forEach((city)=>{

                    var card = document.querySelector(`#city-card-${city}`);
                    card.classList.toggle("hide", true);

                });

                let intersection = citiesid.filter(x => data.includes(x));
                intersection.forEach((city)=>{

                    var card = document.querySelector(`#city-card-${city}`);
                    card.classList.toggle("hide", false);

        });

      }
                 


    };

    


      //================================================= //
     // ============== Invoke Functions    ============= //
    //================================================= //


    // Add event listener to add all buttons events 
        
    sleep(2500).then(() => {

    
        var cards = document.getElementsByClassName("button-city-link"); 
        

        for (var i = 0 ; i < cards.length; i++) {
            cards[i].addEventListener('click' , OnClickCardCity  ) ; 
        }
        
        function OnClickCardCity(e){
        

                var parentNode = e.target.parentNode;
                var ul_link = parentNode.childNodes[1]
                var id = ul_link.childNodes[0].innerHTML
                id = id.replace(/\D/g, '');
                let url = new URL(window.location);
                let params =  new URLSearchParams(url.search);

                params.set('id', id);

               window.location =  'details.html'+'?'+`${params.toString()}`;
                                    


        };
    
    });               


    // Add  Cards first time the site loading 

    CreateCityCard();


    


}());