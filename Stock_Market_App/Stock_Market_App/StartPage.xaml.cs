using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

using StockDataNamespace;

namespace Stock_Market_App
{

    public partial class StartPage : ContentPage
    {
        public string API_Key = "7MW8GOBODP71QU9L";  //personal API key
        public List<TimeSeriesDaily> stockList; //list containing all the objects parsed from the Json string
        double min = 999.00;    //highest price holder. Initizlized so it can also be used to compare. It's reset to this value so it cna be reused after the page has been populated.
        double max = 000.00;    //Lowest price holder. Initizlized so it can also be used to compare. It's reset to this value so it cna be reused after the page has been populated.

        public StartPage()
        {
            InitializeComponent();
        }

        async void GetStockButton_Clicked(object sender, EventArgs e)
        {

            //Getting company's acronym entered by the user in order to be added to the api endpoint
            string userInput;
            userInput = stockEntry.Text;

            //Adding the user input to the base api endpoint and converting into an "Uri" variable.
            string stockApiEndpoint = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY" + "&symbol=" + userInput + "&apikey=" + API_Key;
            Uri stockApiUri = new Uri(stockApiEndpoint);

            //creating a HTTP client variable
            HttpClient client = new HttpClient();

            //Response holder
            HttpResponseMessage response = await client.GetAsync(stockApiUri);

            //response stream into a string (we wait for the whole reponse to be received)
            string jsonContent = await response.Content.ReadAsStringAsync();

            //Parse into an object accroding to our data model (StockData)
            var stockData = StockData.FromJson(jsonContent);

            //Convert the dictionary into a list
            stockList = stockData.TimeSeriesDaily.Values.ToList();

            //Setting the list as the source of the ListView
            historyListView.ItemsSource = stockList;

            //populating the rest of the fields in the page
            populatePage();

        }

        void GetHighAndLow()
        {
            //================================START OF GETTING ABSOLUTE MAX AND MINS ===============================

            //loop through each element in the list and compare it's lowest and highest prices. 
            for (int i = 0; i < stockList.Count; i++)
            {
                //If highest so far => max = this
                if (double.Parse(stockList[i].The2High) >= max)
                {
                    max = double.Parse(stockList[i].The2High);
                }

                //If lowest so far => min = this
                if (double.Parse(stockList[i].The3Low) <= min)
                {
                    min = double.Parse(stockList[i].The3Low);
                }
            }

            //set labels to found high and low prices.
            lowestLabel.Text = min.ToString();
            highestLabel.Text = max.ToString();

            //reset values for next use.
            min = 999.00;
            max = 000.00;

            //================================END OF GETTING ABSOLUTE MAX AND MINS ===============================
        }

        void populatePage()
        {
            //apiCall(); //need to fix the asynchronous methods. Async / Await...
            GetHighAndLow();
        }
    }
}





