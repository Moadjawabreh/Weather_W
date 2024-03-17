/*using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Weather
{
	public partial class MainWindow : Window
	{
		private const string apiKey = "";

		public MainWindow()
		{
			InitializeComponent();
		}

		private async void GetWeather_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				double longitude = Convert.ToDouble(longitudeTextBox.Text);
				double latitude = Convert.ToDouble(latitudeTextBox.Text);

				string weatherInfo = await GetWeatherData(longitude, latitude);
				weatherInfoTextBlock.Text = weatherInfo;
			}
			catch (Exception ex)
			{
				weatherInfoTextBlock.Text = $"Error: {ex.Message}";
			}
		}

		private async Task<string> GetWeatherData(double longitude, double latitude)
		{
			string weatherInfo = "";

			for (int height = 0; height <= 2000; height += 200)
			{
				using (var client = new HttpClient())
				{
					var request = new HttpRequestMessage
					{
						Method = HttpMethod.Get,
						RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric&height={height}")
					};

					HttpResponseMessage response = await client.SendAsync(request);


					Dictionary<string, string> countryNames = new Dictionary<string, string>()
{
	{ "AF", "Afghanistan" },
	{ "AX", "Åland Islands" },
	{ "AL", "Albania" },
	{ "DZ", "Algeria" },
	{ "AS", "American Samoa" },
	{ "AD", "Andorra" },
	{ "AO", "Angola" },
	{ "AI", "Anguilla" },
	{ "AQ", "Antarctica" },
	{ "AG", "Antigua and Barbuda" },
	{ "AR", "Argentina" },
	{ "AM", "Armenia" },
	{ "AW", "Aruba" },
	{ "AU", "Australia" },
	{ "AT", "Austria" },
	{ "AZ", "Azerbaijan" },
	{ "BS", "Bahamas" },
	{ "BH", "Bahrain" },
	{ "BD", "Bangladesh" },
	{ "BB", "Barbados" },
	{ "BY", "Belarus" },
	{ "BE", "Belgium" },
	{ "BZ", "Belize" },
	{ "BJ", "Benin" },
	{ "BM", "Bermuda" },
	{ "BT", "Bhutan" },
	{ "BO", "Bolivia (Plurinational State of)" },
	{ "BQ", "Bonaire, Sint Eustatius and Saba" },
	{ "BA", "Bosnia and Herzegovina" },
	{ "BW", "Botswana" },
	{ "BV", "Bouvet Island" },
	{ "BR", "Brazil" },
	{ "IO", "British Indian Ocean Territory" },
	{ "BN", "Brunei Darussalam" },
	{ "BG", "Bulgaria" },
	{ "BF", "Burkina Faso" },
	{ "BI", "Burundi" },
	{ "CV", "Cabo Verde" },
	{ "KH", "Cambodia" },
	{ "CM", "Cameroon" },
	{ "CA", "Canada" },
	{ "KY", "Cayman Islands" },
	{ "CF", "Central African Republic" },
	{ "TD", "Chad" },
	{ "CL", "Chile" },
	{ "CN", "China" },
	{ "CX", "Christmas Island" },
	{ "CC", "Cocos (Keeling) Islands" },
	{ "CO", "Colombia" },
	{ "KM", "Comoros" },
	{ "CG", "Congo" },
	{ "CD", "Congo (Democratic Republic of the)" },
	{ "CK", "Cook Islands" },
	{ "CR", "Costa Rica" },
	{ "HR", "Croatia" },
	{ "CU", "Cuba" },
	{ "CW", "Curaçao" },
	{ "CY", "Cyprus" },
	{ "CZ", "Czech Republic" },
	{ "DK", "Denmark" },
	{ "DJ", "Djibouti" },
	{ "DM", "Dominica" },
	{ "DO", "Dominican Republic" },
	{ "EC", "Ecuador" },
	{ "EG", "Egypt" },
	{ "SV", "El Salvador" },
	{ "GQ", "Equatorial Guinea" },
	{ "ER", "Eritrea" },
	{ "EE", "Estonia" },
	{ "ET", "Ethiopia" },
	{ "FK", "Falkland Islands (Malvinas)" },
	{ "FO", "Faroe Islands" },
	{ "FJ", "Fiji" },
	{ "FI", "Finland" },
	{ "FR", "France" },
	{ "GF", "French Guiana" },
	{ "PF", "French Polynesia" },
	{ "TF", "French Southern Territories" },
	{ "GA", "Gabon" },
	{ "GM", "Gambia" },
	{ "GE", "Georgia" },
	{ "DE", "Germany" },
	{ "GH", "Ghana" },
	{ "GI", "Gibraltar" },
	{ "GR", "Greece" },
	{ "GL", "Greenland" },
	{ "GD", "Grenada" },
	{ "GP", "Guadeloupe" },
	{ "GU", "Guam" },
	{ "GT", "Guatemala" },
	{ "GG", "Guernsey" },
	{ "GN", "Guinea" },
	{ "GW", "Guinea-Bissau" },
	{ "GY", "Guyana" },
	{ "HT", "Haiti" },
	{ "HM", "Heard Island and McDonald Islands" },
	{ "VA", "Holy See" },
	{ "HN", "Honduras" },
	{ "HK", "Hong Kong" },
	{ "HU", "Hungary" },
	{ "IS", "Iceland" },
	{ "IN", "India" },
	{ "ID", "Indonesia" },
	{ "IR", "Iran (Islamic Republic of)" },
	{ "IQ", "Iraq" },
	{ "IE", "Ireland" },
	{ "IM", "Isle of Man" },
	{ "IL", "Israel" },
	{ "IT", "Italy" },
	{ "JM", "Jamaica" },
	{ "JP", "Japan" },
	{ "JE", "Jersey" },
	{ "JO", "Jordan" },
	{ "KZ", "Kazakhstan" },
	{ "KE", "Kenya" },
	{ "KI", "Kiribati" },
	{ "KP", "Korea (Democratic People's Republic of)" },
	{ "KR", "Korea (Republic of)" },
	{ "KW", "Kuwait" },
	{ "KG", "Kyrgyzstan" },
	{ "LA", "Lao People's Democratic Republic" },
	{ "LV", "Latvia" },
	{ "LB", "Lebanon" },
	{ "LS", "Lesotho" },
	{ "LR", "Liberia" },
	{ "LY", "Libya" },
	{ "LI", "Liechtenstein" },
	{ "LT", "Lithuania" },
	{ "LU", "Luxembourg" },
	{ "MO", "Macao" },
	{ "MK", "Macedonia (the former Yugoslav Republic of)" },
	{ "MG", "Madagascar" },
	{ "MW", "Malawi" },
	{ "MY", "Malaysia" },
	{ "MV", "Maldives" },
	{ "ML", "Mali" },
	{ "MT", "Malta" },
	{ "MH", "Marshall Islands" },
	{ "MQ", "Martinique" },
	{ "MR", "Mauritania" },
	{ "MU", "Mauritius" },
	{ "YT", "Mayotte" },
	{ "MX", "Mexico" },
	{ "FM", "Micronesia (Federated States of)" },
	{ "MD", "Moldova (Republic of)" },
	{ "MC", "Monaco" },
	{ "MN", "Mongolia" },
	{ "ME", "Montenegro" },
	{ "MS", "Montserrat" },
	{ "MA", "Morocco" },
	{ "MZ", "Mozambique" },
	{ "MM", "Myanmar" },
	{ "NA", "Namibia" },
	{ "NR", "Nauru" },
	{ "NP", "Nepal" },
	{ "NL", "Netherlands" },
	{ "NC", "New Caledonia" },
	{ "NZ", "New Zealand" },
	{ "NI", "Nicaragua" },
	{ "NE", "Niger" },
	{ "NG", "Nigeria" },
	{ "NU", "Niue" },
	{ "NF", "Norfolk Island" },
	{ "MP", "Northern Mariana Islands" },
	{ "NO", "Norway" },
	{ "OM", "Oman" },
	{ "PK", "Pakistan" },
	{ "PW", "Palau" },
	{ "PS", "Palestine, State of" },
	{ "PA", "Panama" },
	{ "PG", "Papua New Guinea" },
	{ "PY", "Paraguay" },
	{ "PE", "Peru" },
	{ "PH", "Philippines" },
	{ "PN", "Pitcairn" },
	{ "PL", "Poland" },
	{ "PT", "Portugal" },
	{ "PR", "Puerto Rico" },
	{ "QA", "Qatar" },
	{ "RE", "Réunion" },
	{ "RO", "Romania" },
	{ "RU", "Russian Federation" },
	{ "RW", "Rwanda" },
	{ "BL", "Saint Barthélemy" },
	{ "SH", "Saint Helena, Ascension and Tristan da Cunha" },
	{ "KN", "Saint Kitts and Nevis" },
	{ "LC", "Saint Lucia" },
	{ "MF", "Saint Martin (French part)" },
	{ "PM", "Saint Pierre and Miquelon" },
	{ "VC", "Saint Vincent and the Grenadines" },
	{ "WS", "Samoa" },
	{ "SM", "San Marino" },
	{ "ST", "Sao Tome and Principe" },
	{ "SA", "Saudi Arabia" },
	{ "SN", "Senegal" },
	{ "RS", "Serbia" },
	{ "SC", "Seychelles" },
	{ "SL", "Sierra Leone" },
	{ "SG", "Singapore" },
	{ "SX", "Sint Maarten (Dutch part)" },
	{ "SK", "Slovakia" },
	{ "SI", "Slovenia" },
	{ "SB", "Solomon Islands" },
	{ "SO", "Somalia" },
	{ "ZA", "South Africa" },
	{ "GS", "South Georgia and the South Sandwich Islands" },
	{ "SS", "South Sudan" },
	{ "ES", "Spain" },
	{ "LK", "Sri Lanka" },
	{ "SD", "Sudan" },
	{ "SR", "Suriname" },
	{ "SJ", "Svalbard and Jan Mayen" },
	{ "SZ", "Swaziland" },
	{ "SE", "Sweden" },
	{ "CH", "Switzerland" },
	{ "SY", "Syrian Arab Republic" },
	{ "TW", "Taiwan (Province of China)" },
	{ "TJ", "Tajikistan" },
	{ "TZ", "Tanzania, United Republic of" },
	{ "TH", "Thailand" },
	{ "TL", "Timor-Leste" },
	{ "TG", "Togo" },
	{ "TK", "Tokelau" },
	{ "TO", "Tonga" },
	{ "TT", "Trinidad and Tobago" },
	{ "TN", "Tunisia" },
	{ "TR", "Turkey" },
	{ "TM", "Turkmenistan" },
	{ "TC", "Turks and Caicos Islands" },
	{ "TV", "Tuvalu" },
	{ "UG", "Uganda" },
	{ "UA", "Ukraine" },
	{ "AE", "United Arab Emirates" },
	{ "GB", "United Kingdom of Great Britain and Northern Ireland" },
	{ "US", "United States of America" },
	{ "UM", "United States Minor Outlying Islands" },
	{ "UY", "Uruguay" },
	{ "UZ", "Uzbekistan" },
	{ "VU", "Vanuatu" },
	{ "VE", "Venezuela (Bolivarian Republic of)" },
	{ "VN", "Viet Nam" },
	{ "VG", "Virgin Islands (British)" },
	{ "VI", "Virgin Islands (U.S.)" },
	{ "WF", "Wallis and Futuna" },
	{ "EH", "Western Sahara" },
	{ "YE", "Yemen" },
	{ "ZM", "Zambia" },
	{ "ZW", "Zimbabwe" }
};



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
						string countryName = countryNames.ContainsKey(countryNameCode) ? countryNames[countryNameCode] : "Unknown";
						weatherInfo += $"Height = {height}\n";
                        weatherInfo += $"Country: {countryName}\n";
						weatherInfo += $"Time: {countryTime.ToString("yyyy-MM-dd HH:mm:ss")}\n";
						weatherInfo += $"Temperature: {temperature}°C\n";
						weatherInfo += $"Wind Speed: {windSpeed} m/s\n";
						weatherInfo += $"Wind Direction: {windDirection}\n";
						weatherInfo += $"Weather Description: {weatherDescription}\n\n";
					}

					else
					{
						return $"Failed to retrieve weather data. Status code: {response.StatusCode}";
					}
				}
			}

			return weatherInfo;
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
}*/

using System;
using System.Collections.Generic;
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
        }

        private async void GetWeather_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double longitude = Convert.ToDouble(longitudeTextBox.Text);
                double latitude = Convert.ToDouble(latitudeTextBox.Text);

                (string locationName, List<WeatherData> weatherDataList) = await GetWeatherData(longitude, latitude);

                weatherInfoTextBlock.Text = $"Location: {locationName}";

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
}

