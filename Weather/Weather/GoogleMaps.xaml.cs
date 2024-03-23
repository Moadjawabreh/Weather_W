using GMap.NET.MapProviders;
using GMap.NET;
using System;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;

namespace Weather
{
	/// <summary>
	/// Interaction logic for GoogleMaps.xaml
	/// </summary>
	/// 

	public class LocationSelectedEventArgs : EventArgs
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public LocationSelectedEventArgs(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}
	}

	public partial class GoogleMaps : Window
	{
		private PointLatLng startDragPoint;
		private bool isDragging;
		private PointLatLng selectedPoint;
		public event EventHandler<LocationSelectedEventArgs> LocationSelected;


		private const double MovementThreshold = 5; // Adjust the movement threshold as needed
		private PointLatLng previousMousePosition;


		public GoogleMaps()
		{
			InitializeComponent();
			InitializeMap();
		}

		private void InitializeMap()
		{
			// Set up the map control
			gmapControl.MapProvider = GMapProviders.GoogleHybridMap; // Use Google Hybrid map provider
			GMaps.Instance.Mode = AccessMode.ServerOnly;
			gmapControl.Position = new PointLatLng(0, 0);
			gmapControl.Zoom = 2;

			// Enable map interaction
			gmapControl.CanDragMap = true;
			gmapControl.MouseWheelZoomEnabled = true;

			// Set minimum and maximum zoom levels
			gmapControl.MinZoom = 2;
			gmapControl.MaxZoom = 20;

			// Add event handlers for mouse events
			gmapControl.MouseDoubleClick += GmapControl_MouseDoubleClick;
			gmapControl.MouseDown += GmapControl_MouseDown;
			gmapControl.MouseMove += GmapControl_MouseMove;
			gmapControl.MouseUp += GmapControl_MouseUp;
		}

		//private void SearchCountry_Click(object sender, RoutedEventArgs e)
		//{
		//	string countryName = txtSearchCountry.Text.Trim();

		//	if (!string.IsNullOrEmpty(countryName))
		//	{
		//		var addresses = geocodingProvider.GetPoints(countryName, out int total);

		//		if (addresses != null && addresses.Count > 0)
		//		{
		//			var firstAddress = addresses[0];
		//			gmapControl.Position = new PointLatLng(firstAddress.Latitude, firstAddress.Longitude);
		//			gmapControl.Zoom = 6; // Adjust zoom level as needed
		//		}
		//		else
		//		{
		//			MessageBox.Show("Country not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		//		}
		//	}
		//	else
		//	{
		//		MessageBox.Show("Please enter a country name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		//	}
		//}

		private async void GmapControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (isDragging)
			{
				PointLatLng currentMousePosition = gmapControl.FromLocalToLatLng((int)e.GetPosition(gmapControl).X, (int)e.GetPosition(gmapControl).Y);
				double distance = GMap.NET.MapProviders.GMapProviders.EmptyProvider.Projection.GetDistance(previousMousePosition, currentMousePosition);

				if (distance > MovementThreshold)
				{
					double latChange = startDragPoint.Lat - currentMousePosition.Lat;
					double lngChange = startDragPoint.Lng - currentMousePosition.Lng;
					gmapControl.Position = new PointLatLng(gmapControl.Position.Lat + latChange, gmapControl.Position.Lng + lngChange);
					startDragPoint = currentMousePosition;

					await Task.Delay(10); // Delay to reduce the frequency of updates and improve performance
				}
			}
		}


		private void GmapControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
			{
				startDragPoint = gmapControl.FromLocalToLatLng((int)e.GetPosition(gmapControl).X, (int)e.GetPosition(gmapControl).Y);
				previousMousePosition = startDragPoint;
				isDragging = true;
			}
		}

		private void GmapControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (e.LeftButton == System.Windows.Input.MouseButtonState.Released)
			{
				isDragging = false;
			}
		}

		private void GmapControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			// Get the location where the user double-clicked on the map
			selectedPoint = gmapControl.FromLocalToLatLng((int)e.GetPosition(gmapControl).X, (int)e.GetPosition(gmapControl).Y);

			// Now you can access the Latitude and Longitude properties of selectedPoint
			double latitude = selectedPoint.Lat;
			double longitude = selectedPoint.Lng;

			// You can then save these values to variables or do whatever you need with them
			LocationSelected?.Invoke(this, new LocationSelectedEventArgs(latitude, longitude));

			Close();
		}
		private async void SearchCity_ClickAsync(object sender, RoutedEventArgs e)
		{
			string APIKey = "fc3fbb1cb53a44ca90d8cde02aaabda2";
			string cityName = txtSearchCity.Text.Trim();
			using (var httpClient = new HttpClient())
			{
				string requestUri = $"https://api.opencagedata.com/geocode/v1/json?q={cityName}&key={APIKey}";

				HttpResponseMessage response = await httpClient.GetAsync(requestUri);
				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<OpenCageResponse>(responseBody);

					if (result != null && result.Results.Count > 0)
					{
						var firstResult = result.Results[0];
						double latitude = firstResult.Geometry.Latitude;
						double longitude = firstResult.Geometry.Longitude;
						gmapControl.Position = new PointLatLng(latitude, longitude);
						gmapControl.Zoom = 10; // Adjust zoom level as needed
					}
					else
					{
						MessageBox.Show("City not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		public class OpenCageResponse
		{
			[JsonProperty("results")]
			public List<Result> Results { get; set; }
		}
		public class Result
		{
			[JsonProperty("geometry")]
			public Geometry Geometry { get; set; }
		}

		public class Geometry
		{
			[JsonProperty("lat")]
			public double Latitude { get; set; }

			[JsonProperty("lng")]
			public double Longitude { get; set; }
		}
	}
}