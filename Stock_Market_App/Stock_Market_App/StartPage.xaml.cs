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
        public string API_Key = "7MW8GOBODP71QU9L";
        public List<StockData> days;
        public List<TimeSeriesDaily> stockList;
        double min = 999.00;
        double max = 000.00;

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

            //Getting other data (30 days High and Low)
            PopulatePage();

        }


        public void PopulatePage()
        {
            //================================START OF GETTING ABSOLUTE MAX AND MINS ===============================

            /*
            foreach (var stock in stockList)
            {
                Console.WriteLine(stock.The3Low); //debugging
                Console.WriteLine(double.Parse(stock.The3Low)); //debugging
                if (double.Parse(stock.The3Low) <= min)
                {
                    foundMin = double.Parse(stock.The3Low);
                }

                Console.WriteLine(stock.The2High); //debugging
                Console.WriteLine(double.Parse(stock.The2High)); //debugging
                if (double.Parse(stock.The2High) >= max)
                {
                    foundMax = double.Parse(stock.The2High);
                }
            }

            */

            for(int i = 0; i<stockList.Count; i++)
            {
                if(double.Parse(stockList[i].The2High) >= max)
                {
                    max = double.Parse(stockList[i].The2High);
                }

                if (double.Parse(stockList[i].The3Low) <= min)
                {
                    min = double.Parse(stockList[i].The3Low);
                }
            }


            lowestLabel.Text = min.ToString();
            highestLabel.Text = max.ToString();
            min = 999.00;
            max = 000.00;
        }
    }
}
               

             
    

