using LiveCharts;
using LiveCharts.Wpf;
using MeteoLibrary;
using MeteoLibrary.Models;
using MeteoLibrary.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeteoLtWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string LastCode { get; set; }
        public string LastPlace { get; set; }

        private async void Onload(object sender, RoutedEventArgs e)
        {
            var allPlaces = await PlaceProcessor.LoadPlace();

            ListBoxPlaces.ItemsSource = allPlaces;
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxPlaceFilter.Items.Clear();

            ListBoxPlaceFilter.Visibility = Visibility.Visible;

            LabelChoosePlace.Visibility = Visibility.Visible;

            string toSearch = TextBoxSearch.Text;

            if (TextBoxSearch.Text == "")
            {
                ListBoxPlaceFilter.Items.Clear();
            }
            else
            {
                foreach (string place in ListBoxPlaces.Items)
                {
                    if (place.Contains(toSearch))
                    {
                        ListBoxPlaceFilter.Items.Add(place);
                    }
                }
            }
        }

        private async void ListBoxPlaceFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var code = "";

            var testFilter = ListBoxPlaceFilter.SelectedItem;

            if (testFilter != null)
            {
                LastPlace = ListBoxPlaceFilter.SelectedItem.ToString();

                code = await PlaceProcessor.GetPlaceCode(ListBoxPlaceFilter.SelectedItem.ToString());

                LastCode = code;
            }
            else
            {
                code = LastCode;
            }

            var placeForecast = await ForecastProcessor.LoadForecast(code);
            var firstTemp = Convert.ToInt32(placeForecast.First().AirTemperature);
            var firstCond = placeForecast.First().ConditionCode;
            var firstHum = placeForecast.First().RelativeHumidity;

            TextBoxPlace.Text = LastPlace.ToString();
            TextBoxTempNow.Text = $"{firstTemp.ToString()}°";
            TextBoxCondNow.Text = firstCond;
            TextBoxHumNow.Text = $"Humidity {firstHum.ToString()}%";

            DrawTempChart(placeForecast);
            DrawHumiChart(placeForecast);
        }

        private void DrawTempChart(IList<ForecastTimestampModel> forecasts)
        {
            ColumnSeries columnSeries = new ColumnSeries()
            {
                DataLabels = true,
                Values = new ChartValues<int>(),
                LabelPoint = point => point.Y.ToString()
            };

            Axis axis = new Axis()
            {
                Separator = new LiveCharts.Wpf.Separator()
                {
                    Step = 4,
                    IsEnabled = false
                }
            };

            axis.Labels = new List<string>();

            var recordsCount = 0;

            TempChart.Series.Clear();
            TempChart.AxisX.Clear();
            TempChart.AxisY.Clear();

            foreach (var x in forecasts)
            {
                columnSeries.Values.Add(Convert.ToInt32(x.AirTemperature));
                var ForecastTimeLocal = DateTime.Parse(x.ForecastTimeUtc).ToLocalTime();
                axis.Labels.Add(ForecastTimeLocal.ToString());

                recordsCount += 1;
                if (recordsCount == 48)
                {
                    break;
                }
            }
            TempChart.Visibility = Visibility.Visible;
            TempChart.Series.Add(columnSeries);
            TempChart.AxisX.Add(axis);
            TempChart.AxisY.Add(new Axis
            {
                LabelFormatter = value => value.ToString() + "°",
                Separator = new LiveCharts.Wpf.Separator()
            });
        }

        private void DrawHumiChart(IList<ForecastTimestampModel> forecasts)
        {
            ColumnSeries columnSeries = new ColumnSeries()
            {
                DataLabels = true,
                Values = new ChartValues<int>(),
                LabelPoint = point => point.Y.ToString()
            };

            Axis axis = new Axis()
            {
                Separator = new LiveCharts.Wpf.Separator()
                {
                    Step = 4,
                    IsEnabled = false
                }
            };

            axis.Labels = new List<string>();

            var recordsCount = 0;

            HumiChart.Series.Clear();
            HumiChart.AxisX.Clear();
            HumiChart.AxisY.Clear();

            foreach (var x in forecasts)
            {
                columnSeries.Values.Add(Convert.ToInt32(x.RelativeHumidity));
                var ForecastTimeLocal = DateTime.Parse(x.ForecastTimeUtc).ToLocalTime();
                axis.Labels.Add(ForecastTimeLocal.ToString());

                recordsCount += 1;
                if (recordsCount == 48)
                {
                    break;
                }
            }
            HumiChart.Visibility = Visibility.Visible;
            HumiChart.Series.Add(columnSeries);
            HumiChart.AxisX.Add(axis);
            HumiChart.AxisY.Add(new Axis
            {
                LabelFormatter = value => value.ToString() + "%",
                Separator = new LiveCharts.Wpf.Separator()
            });
        }
    }
}
