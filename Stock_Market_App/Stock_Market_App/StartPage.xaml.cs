using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using StockData;

namespace Stock_Market_App
{

    public partial class StartPage : ContentPage
    {
        public string API_Key = "7MW8GOBODP71QU9L";
        public List<TimeSeriesDaily> days;

        public StartPage()
        {
            InitializeComponent();
        }

        async void GetStockButton_Clicked(object sender, EventArgs e)
        {
            //Getting company's acronym entered by the user in order to be added to the api endpoint
            string userInput = "";
            userInput = stockEntry.Text;

            //Adding the user input to the base api endpoint and converting into an "Uri" variable.
            string stockApiEndpoint = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY" + "&symbol=" + userInput + "&apikey=" + API_Key;
            Uri stockApiUri = new Uri(stockApiEndpoint);

            HttpClient client = new HttpClient();
            TimeSeriesDaily stockData = new TimeSeriesDaily();

            HttpResponseMessage response = await client.GetAsync(stockApiUri);

            string jsonContent = await response.Content.ReadAsStringAsync();

            stockData = JsonConvert.DeserializeObject<TimeSeriesDaily>(jsonContent);

            days.Add(stockData);

            historyListView.ItemsSource = days;

            PopulatePage();
        }

        public void PopulatePage()
        {
            //================================START OF GETTING ABSOLUTE MAX AND MINS ================================
            double min = 999999.99;
            double max = 000000.00;
            double foundMin = 0;
            double foundMax = 0;
           
            
            for(int i=0; i<days.Count; i++)
            {

                Console.WriteLine(days[i].The3Low);

                if(double.Parse(days[i].The3Low) < min)
                {
                    foundMin = double.Parse(days[i].The3Low);
                }

                if(double.Parse(days[i].The2High) > max)
                {
                    foundMax = double.Parse(days[i].The2High);
                }
            }

            lowestLabel.Text = foundMin.ToString();
            highestLabel.Text = foundMax.ToString();
        }

        //================================ END OF GETTING ABSOLUTE MAX AND MINS ================================



    }
}
