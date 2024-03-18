
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Weather
{
    public partial class MainWindow : Window
    {
        private const string apiKey = "c37f32def15e692dfa2ac57acdfd154f";

        public MainWindow()
        {
            InitializeComponent();
			DataContext = new WeatherInfoViewModel();
		}

		private async void GetWeather_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				double longitude = Convert.ToDouble(longitudeTextBox.Text);
				double latitude = Convert.ToDouble(latitudeTextBox.Text);

				(string locationName, List<WeatherData> weatherDataList) = await GetWeatherData(longitude, latitude);

				weatherInfoTextBlock.Text = $"Location: {locationName}";

				// Retrieve weather data for the current conditions including height parameter
				WeatherInfoViewModel weatherInfo = await GetWeatherDataForTheCurrent(longitude, latitude, 500); // 500 meters height

				// Bind the weather info to the UI
				DataContext = weatherInfo;

				// Clear the existing items in the data grid
				DataTable.Items.Clear();

				// Add the new weather data to the data grid
				foreach (var weatherData in weatherDataList)
				{
					DataTable.Items.Add(weatherData);
				}
			}
			catch (Exception ex)
			{
				weatherInfoTextBlock.Text = $"Error: {ex.Message}";
			}
		}
		private async Task<WeatherInfoViewModel> GetWeatherDataForTheCurrent(double longitude, double latitude, int height)
		{
			WeatherInfoViewModel weatherInfoViewModel = null;

			using (var client = new HttpClient())
			{
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric&height={height}")
				};

				HttpResponseMessage response = await client.SendAsync(request);

				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

					double temperature = data.main.temp;
					double windSpeed = data.wind.speed;
					double windDirectionDegrees = (double)data.wind.deg; // Ensure wind direction is parsed as double
					string weatherDescription = data.weather[0].description;
					string windDirection = ConvertDegreesToCardinal(windDirectionDegrees);

					// Fetch time of the country
					DateTime countryTime = DateTimeOffset.FromUnixTimeSeconds((long)data.dt).DateTime;
					string countryNameCode = data.sys.country;

					// Look up the full country name based on the country code

					// Populate the WeatherInfoViewModel object
					weatherInfoViewModel = new WeatherInfoViewModel
					{
						Country = countryNameCode,
						Time = countryTime.AddHours(3).ToString("yyyy-MM-dd HH:mm:ss"), // Add 3 hours to the country time
						Temperature = $"{temperature}°C",
						WindSpeed = $"{windSpeed} m/s",
						WindDirection = windDirection,
						WeatherDescription = weatherDescription
					};
				}
				else
				{
					// Handle failed requests
					return null;
				}
			}

			return weatherInfoViewModel;
		}
		private async Task<(string locationName, List<WeatherData> weatherDataList)> GetWeatherData(double longitude, double latitude)
        {
            string locationName = "";

            List<WeatherData> weatherDataList = new List<WeatherData>();

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric")
                };

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                    locationName = data.city.name;

                    foreach (var forecast in data.list)
                    {
                        DateTime forecastTime = DateTimeOffset.FromUnixTimeSeconds((long)forecast.dt).DateTime;
                        double temperature = forecast.main.temp;
                        double windSpeed = forecast.wind.speed;
                        double windDirectionDegrees = (double)forecast.wind.deg;
                        string weatherDescription = forecast.weather[0].description;
                        string windDirection = ConvertDegreesToCardinal(windDirectionDegrees);

                        weatherDataList.Add(new WeatherData
                        {
                            Longitude = longitude,
                            Latitude = latitude,
                            Date = forecastTime,
                            Temperature = temperature,
                            WindSpeed = windSpeed,
                            WindDirection = windDirection,
                            WeatherDescription = weatherDescription
                        });
                    }
                }
                else
                {
                    throw new Exception($"Failed to retrieve weather data. Status code: {response.StatusCode}");
                }
            }

            return (locationName, weatherDataList);
        }


        private string ConvertDegreesToCardinal(double degrees)
        {
            string[] cardinalDirections = {
                "North", "North-Northeast", "Northeast", "East-Northeast",
                "East", "East-Southeast", "Southeast", "South-Southeast",
                "South", "South-Southwest", "Southwest", "West-Southwest",
                "West", "West-Northwest", "Northwest", "North-Northwest"
            };

            int index = (int)Math.Round((degrees % 360) / 22.5);
            return cardinalDirections[index % 16];
        }
    }

    public class WeatherData
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; }
        public string WeatherDescription { get; set; }
    }
	public class WeatherInfoViewModel : INotifyPropertyChanged
	{

		private string _country;
		private string _time;
		public string _temperature;
		public string _windSpeed;
		public string _windDirection;
		public string _weatherDescription;

		public string Country
		{
			get { return _country; }
			set
			{
				if (_country != value)
				{
					_country = value;
					OnPropertyChanged(nameof(Country));
				}
			}
		}

		public string Time
		{
			get { return _time; }
			set
			{
				if (_time != value)
				{
					_time = value;
					OnPropertyChanged(nameof(Time));
				}
			}
		}

		public string Temperature
		{
			get { return _temperature; }
			set
			{
				if (_temperature != value)
				{
					_temperature = value;
					OnPropertyChanged(nameof(Temperature));
				}
			}
		}

		public string WindSpeed
		{
			get { return _windSpeed; }
			set
			{
				if (_windSpeed != value)
				{
					_windSpeed = value;
					OnPropertyChanged(nameof(WindSpeed));
				}
			}

		}

		public string WindDirection
		{
			get { return _windDirection; }
			set
			{
				if (_windDirection != value)
				{
					_windDirection = value;
					OnPropertyChanged(nameof(WindDirection));
				}
			}

		}

		public string WeatherDescription
		{
			get { return _weatherDescription; }
			set
			{
				if (_weatherDescription != value)
				{
					_weatherDescription = value;
					OnPropertyChanged(nameof(WeatherDescription));
				}
			}

		}

		// Repeat this pattern for other properties

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}

