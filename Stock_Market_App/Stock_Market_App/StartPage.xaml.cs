﻿using System;
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
        public string API_Key = "7MW8GOBODP71QU9L";
        public List<StockData> days;
        public List<TimeSeriesDaily> stockList;
        double min = 999999.99;
        double max = 000000.00;
        double foundMin = 0;
        double foundMax = 0;

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
            //TimeSeriesDaily stockData = new TimeSeriesDaily();

            //Response holder
            HttpResponseMessage response = await client.GetAsync(stockApiUri);

            //response stream into a string (we wait for the whole reponse to be received)
            string jsonContent = await response.Content.ReadAsStringAsync();

            //Parse into an object accroding to our data model (TimeSeriesDaily)
            var stockData = StockData.FromJson(jsonContent);

            stockList = stockData.TimeSeriesDaily.Values.ToList();


            historyListView.ItemsSource = stockList;

            PopulatePage();

        }


        public void PopulatePage()
        {
            //================================START OF GETTING ABSOLUTE MAX AND MINS ===============================

            foreach (var stock in stockList)
            {
                // Console.WriteLine(days[i].TimeSeriesDaily.The2high);
                if(double.Parse(stock.The3Low) < min)
                {
                    foundMin = double.Parse(stock.The3Low);
                }

                if(double.Parse(stock.The2High) > max)
                {
                    foundMax = double.Parse(stock.The2High);
                }
            }


            lowestLabel.Text = foundMin.ToString();
            highestLabel.Text = foundMax.ToString();
        }
    }
}
               

             
    

