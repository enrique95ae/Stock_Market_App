﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using StockDataNamespace;
using DayModel;
using Entry = Microcharts.Entry;
using globals;
using SkiaSharp;
using Microcharts;

namespace Stock_Market_App
{
    public partial class ChartsPage : ContentPage
    {

        List<Entry> entries = new List<Entry> { };
        Entry entry;
        float temp;



        public void fillChartList()
        {
            if(GlobalVariables.retreived == true)
            {
                for (int i = 0; i < GlobalVariables.stockList.Count; i++)
                {
                    if(i%2 == 0) //getting rid of some values to display in the chart because it was too packed.
                    {
                        temp = float.Parse(GlobalVariables.stockList[i].The2High);


                        entry = new Entry(temp)
                        {
                            Color = SKColor.Parse("#FF1943"),
                            Label = GlobalVariables.datesList[i],
                            ValueLabel = GlobalVariables.datesList[i]
                        };

                        entries.Add(entry);
                        entries.Reverse();
                    }
                }
            }
        }

        public ChartsPage()
        {
            InitializeComponent();
            //fillChartList();
            //ChartView.Chart = new LineChart { Entries = entries };

        }

        async void GenerateChartButton_Clicked(object sender, EventArgs e)
        {
            entries.Clear();
            ChartView.Chart = new LineChart { Entries = entries };

            fillChartList();
            ChartView.Chart = new LineChart { Entries = entries };
            GlobalVariables.retreived = false;
        }
    }
}
