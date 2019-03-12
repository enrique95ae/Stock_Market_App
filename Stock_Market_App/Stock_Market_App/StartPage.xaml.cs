using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

using StockDataNamespace;
using DayModel;
using globals;


namespace Stock_Market_App
{
    public partial class StartPage : ContentPage
    {
        public string API_Key = "7MW8GOBODP71QU9L";  //personal API key
        DayData aDay = new DayData();
        double min = 99999.00;    //highest price holder. Initizlized so it can also be used to compare. It's reset to this value so it cna be reused after the page has been populated.
        double max = 00000.00;    //Lowest price holder. Initizlized so it can also be used to compare. It's reset to this value so it cna be reused after the page has been populated.

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
            GlobalVariables.stockList = stockData.TimeSeriesDaily.Values.ToList();
            GlobalVariables.datesList = stockData.TimeSeriesDaily.Keys.ToList();

            List<DayData> DaysList = new List<DayData>();

            for(int i=0; i< GlobalVariables.stockList.Count; i++)
            {
                //Console.WriteLine("test1");
                //Console.WriteLine(datesList[i]);
                //Console.WriteLine(stockList[i].The2High);
                //Console.WriteLine(stockList[i].The3Low);

                aDay.date = GlobalVariables.datesList[i];
                aDay.high = double.Parse(GlobalVariables.stockList[i].The2High);
                aDay.low = double.Parse(GlobalVariables.stockList[i].The3Low);
                aDay.close = double.Parse(GlobalVariables.stockList[i].The4Close);

                //Console.WriteLine("test2");

               DaysList.Add(aDay);
                //DaysList

                //Console.WriteLine("test3");

                //Console.WriteLine("test4");
                //Console.WriteLine(DaysList[i].date);
                //Console.WriteLine(DaysList[i].high);
                //Console.WriteLine(DaysList[i].low);

                //Console.WriteLine("test5");

                aDay = new DayData();

                //Console.WriteLine("test6");
            }

            //Setting the list as the source of the ListView
            historyListView.ItemsSource = DaysList;

            //populating the rest of the fields in the page
            populatePage();
            GlobalVariables.retreived = true;
        }

        void GetHighAndLow()
        {
            //================================START OF GETTING ABSOLUTE MAX AND MINS ===============================

            //loop through each element in the list and compare it's lowest and highest prices. 
            for (int i = 0; i < GlobalVariables.stockList.Count; i++)
            {
                //Console.WriteLine(datesList[i]);

                //If highest so far => max = this
                if (double.Parse(GlobalVariables.stockList[i].The2High) >= max)
                {
                    max = double.Parse(GlobalVariables.stockList[i].The2High);
                }

                //If lowest so far => min = this
                if (double.Parse(GlobalVariables.stockList[i].The3Low) <= min)
                {
                    min = double.Parse(GlobalVariables.stockList[i].The3Low);
                }
            }

            //set labels to found high and low prices.
            lowestLabel.Text = min.ToString();
            highestLabel.Text = max.ToString();

            //reset values for next use.
            min = 99999.00;
            max = 00000.00;

            //================================END OF GETTING ABSOLUTE MAX AND MINS ===============================
        }

        void populatePage()
        {
            //apiCall(); //need to fix the asynchronous methods. Async / Await...
            GetHighAndLow();
        }

    }
}