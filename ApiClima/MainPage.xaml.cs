using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ApiClima.Models;
using static ApiClima.Models.WeatherData;
using ApiClima.Data;
using System.Security.Cryptography;

namespace ApiClima
{
    public partial class MainPage : ContentPage
    {
        private string selectedCity;

        private const string ApiKey = "e0bea8c5b38e480f93f150836241504"; // Tu clave API

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            string cityName = cityEntry.Text;

            if (string.IsNullOrWhiteSpace(cityName))
            {
                await DisplayAlert("Error", "Por favor ingrese el nombre de la ciudad.", "OK");
                return;
            }

            await LoadWeatherDataAsync(cityName);
        }

        private async Task LoadWeatherDataAsync(string cityName)
        {
            string apiUrl = $"https://api.weatherapi.com/v1/current.json?key={ApiKey}&q={cityName}&aqi=no";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(apiUrl);
                    var weatherData = JsonConvert.DeserializeObject<WeatherApiResponse>(response);

                    cityLabel.Text = $"Ciudad: {weatherData.location.name}"; // Ciudad
                    temperatureLabel.Text = $"{weatherData.current.temp_c} °C"; // Temperatura
                    selectedCity = cityName;
                    weatherLabel.Text = weatherData.current.condition.text; // Descripción
                    localtimeLabel.Text = $"Hora: {weatherData.location.localtime}";

                    string iconCode = weatherData.current.condition.icon; // Icono del clima
                    iconImage.Source = new UriImageSource
                    {
                        Uri = new Uri($"https:{iconCode}"),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(1)
                    };
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la información de la ciudad: {cityName}.", "OK");
                Console.WriteLine(ex.Message);
            }
        }

        private async void Insertar(object sender, EventArgs e)
        {
            // Verifica si hay datos del clima cargados
            if (string.IsNullOrEmpty(selectedCity))
            {
                await DisplayAlert("Error", "Primero obtén el clima de una ciudad antes de guardarlo.", "OK");
                return;
            }

            // Crea un nuevo ClimaModel y llena sus propiedades
            ClimaModel climaData = LlenarClima(); // LlenarClima ahora llenará con datos obtenidos de la API

            // Guarda el clima en la base de datos
            int result = await App.PersonaDataBase.GuardarClimaAsync(climaData);

            if (result > 0)
            {
                await DisplayAlert("Guardado", "El clima de la ciudad ha sido guardado en la base de datos.", "OK");
            }
        }


        private ClimaModel LlenarClima()
        {
            ClimaModel clima = new ClimaModel();

            // Asignar el nombre de la ciudad
            clima.name = selectedCity;

            // Asignar la temperatura (ejemplo, asegúrate de que esta propiedad exista en tu modelo)
            if (double.TryParse(temperatureLabel.Text.Replace(" °C", ""), out double temp))
            {
                clima.temp = temp; // Asegúrate de que tienes una propiedad para la temperatura
            }

            // Asignar la descripción del clima (ejemplo)
            clima.condition = weatherLabel.Text; // Asegúrate de que existe una propiedad para la condición

            // Agrega aquí más campos según tu modelo ClimaModel

            return clima;
        }

        private async void OnViewClimasButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaClimas()); // Navegar a la página de lista de climas
        }

    }

}
