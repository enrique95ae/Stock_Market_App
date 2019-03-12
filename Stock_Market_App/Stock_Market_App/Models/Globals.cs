namespace globals
{
    using StockDataNamespace;
    using System.Collections.Generic;

    public static class GlobalVariables
    {
        public static List<TimeSeriesDaily> stockList;
        public static List<string> datesList;
        public static bool retreived = false;
    }
}