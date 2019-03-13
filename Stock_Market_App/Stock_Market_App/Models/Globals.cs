namespace globals
{
    using StockDataNamespace;
    using System.Collections.Generic;

    public static class GlobalVariables
    {
        public static List<TimeSeriesDaily> stockList;
        public static List<string> datesList;
        public static bool retreived = false;
        public static string errorString = "{\n    \"Error Message\": \"Invalid API call. Please retry or visit the documentation (https://www.alphavantage.co/documentation/) for TIME_SERIES_DAILY.\"\n}";
    }
}