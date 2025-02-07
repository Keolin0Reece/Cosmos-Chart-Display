using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CosmosChartDisplay
{
    public partial class Form1 : Form
    {
        private HttpClient _httpClient;

        /// <summary>
        /// Initializes the form and sets up the HTTP client.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Handles the form load event. Initializes and populates a sample chart.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Sample data for chart display
            List<int> array1 = new List<int> { 1, 2, 3, 4 };
            chart1.Series.Clear();

            var series = new Series("Data")
            {
                ChartType = SeriesChartType.Line
            };
            
            // Populate chart with sample data
            for (int i = 0; i < array1.Count; i++)
            {
                series.Points.AddXY(i + 1, array1[i]);
            }

            chart1.Series.Add(series);
        }

        /// <summary>
        /// Handles the click event for retrieving data from an API and updating the chart.
        /// </summary>
        private async void button1_Click(object sender, EventArgs e)
        {
            // API configuration
            string baseUrl = "URL";
            string bearerToken = "bearerToken";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            
            // Construct API request parameters
            string deviceId = textBox1.Text;
            string startDate = startDate.Value.ToString("yyyy-MM-ddTHH:mm:ss");
            string endDate = endDate.Value.ToString("yyyy-MM-ddTHH:mm:ss");
            string dataKey = textBox2.Text;
            string apiUrl = $"{baseUrl}?deviceId={deviceId}&startDate={startDate}&endDate={endDate}&Data={dataKey}";

            // Measure API response time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string response = await _httpClient.GetStringAsync(apiUrl);
            stopwatch.Stop();
            label1.Text = $"{stopwatch.Elapsed.TotalMilliseconds} ms for query to be done";

            // Deserialize JSON response
            var jsonData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(response);
            chart1.Series.Clear();
            var series = new Series("Data") { ChartType = SeriesChartType.Line };

            // Calculate timestamp difference for transmission intervals
            long tsDifference = CalculateTimestampDifference(jsonData);
            label2.Text = tsDifference >= 60 ? $"{tsDifference / 60} minute transmission intervals" : $"{tsDifference} second transmission intervals";

            // Populate chart with retrieved data
            int itemCount = 0;
            foreach (var item in jsonData)
            {
                if (item.TryGetValue("ts", out var tsElement) && item.TryGetValue(dataKey, out var dataElement) &&
                    tsElement is JsonElement tsJson && dataElement is JsonElement dataJson)
                {
                    if (tsJson.TryGetInt64(out long ts) && dataJson.TryGetInt32(out int data))
                    {
                        itemCount++;
                        var dateTime = DateTimeOffset.FromUnixTimeSeconds(ts).UtcDateTime.AddHours(2);
                        series.Points.AddXY(dateTime, data);
                    }
                }
            }

            chart1.Series.Add(series);
            label3.Text = $"{itemCount} number of records for device";
        }

        /// <summary>
        /// Calculates the time difference between the first two timestamps in the dataset.
        /// </summary>
        private long CalculateTimestampDifference(List<Dictionary<string, object>> jsonData)
        {
            if (jsonData.Count >= 2 && jsonData[0].TryGetValue("ts", out var firstTsElement) &&
                jsonData[1].TryGetValue("ts", out var secondTsElement) &&
                firstTsElement is JsonElement firstTsJson &&
                secondTsElement is JsonElement secondTsJson &&
                firstTsJson.TryGetInt64(out long firstTs) &&
                secondTsJson.TryGetInt64(out long secondTs))
            {
                return secondTs - firstTs;
            }
            return 0;
        }
    }

    /// <summary>
    /// Represents an API response containing data and its timestamp.
    /// </summary>
    public class ApiResponse
    {
        public Dictionary<string, object> Data { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
