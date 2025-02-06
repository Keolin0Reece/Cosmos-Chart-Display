using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace CosmosChartDisplay
{
    public partial class Form1 : Form
    {
        private HttpClient _httpClient;
        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<int> array1 = new List<int>();
            array1.Add(1);
            array1.Add(2);
            array1.Add(3);
            array1.Add(4);

            chart1.Series.Clear();
            var series = new Series("Data")
            {
                ChartType = SeriesChartType.Line
            };
            
            int i = 0;
            foreach (var item in array1)
            {
                i++;
                series.Points.AddXY(i, item);
            }

            chart1.Series.Add(series);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string baseUrl = "URL";
            string bearerToken = "bearerToken";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            string deviceId = textBox1.Text;
            string StartDate = startDate.Value.ToString("yyyy-MM-ddTHH:mm:ss");
            string EndDate = endDate.Value.ToString("yyyy-MM-ddTHH:mm:ss");
            string dataKey = textBox2.Text;
            
            string apiUrl = $"{baseUrl}?deviceId={deviceId}&startDate={StartDate}&endDate={EndDate}&Data={dataKey}";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string response = await _httpClient.GetStringAsync(apiUrl);
            stopwatch.Stop();

            label1.Text = stopwatch.Elapsed.TotalMilliseconds.ToString() + " ms for query to be done";
            var jsonData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(response);

            chart1.Series.Clear();
            var series = new Series("Data")
            {
                ChartType = SeriesChartType.Line
            };

            // Calculate the difference between the first two timestamps
            long tsDifference = 0;
            if (jsonData.Count >= 2) // Ensure there are at least two records
            {
                var firstItem = jsonData[10];
                var secondItem = jsonData[11];

                if (firstItem.TryGetValue("ts", out var firstTsElement) &&
                    secondItem.TryGetValue("ts", out var secondTsElement) &&
                    firstTsElement is JsonElement firstTsJson &&
                    secondTsElement is JsonElement secondTsJson &&
                    firstTsJson.TryGetInt64(out long firstTs) &&
                    secondTsJson.TryGetInt64(out long secondTs))
                {
                    // Calculate the timestamp difference
                    tsDifference = secondTs - firstTs;
                }
            }

            if (tsDifference >= 60) { 
                tsDifference = tsDifference / 60;
                label2.Text = tsDifference.ToString();
                label2.Text = label2.Text + " minute transmission intervals";
            }
            else
            {
                label2.Text = tsDifference.ToString();
                label2.Text = label2.Text + " second transmission intervals";
            }

            int itemCount = 0;

            foreach (var item in jsonData)
            {
                // Check if the JSON object contains "ts" and "pSC" fields
                if (item.TryGetValue("ts", out var tsElement) &&
                    item.TryGetValue(dataKey, out var dataElement) &&
                    tsElement is JsonElement tsJson &&
                    dataElement is JsonElement dataJson)
                {
                    // Extract values and add to the chart
                    if (tsJson.TryGetInt64(out long ts) &&
                        dataJson.TryGetInt32(out int data))
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
    }

    public class ApiResponse
    {
        public Dictionary<string, object> Data { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
