using ApiClima.Data;
using ApiClima.Models;

namespace ApiClima;

public partial class ListaClimas : ContentPage
{
    private readonly PersonaDataBase _database;

    public ListaClimas()
    {
        InitializeComponent();

        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WeatherData.db3");
        _database = new PersonaDataBase(dbPath);

        LoadClimas();
    }

    private async void LoadClimas()
    {
        // Carga todos los climas guardados
        var climas = await _database.GetClimasAsync(); // Asegúrate de tener un método para obtener todos los climas

        // Establece la fuente de datos de la ListView
        climaListView.ItemsSource = climas;
    }

    private async void OnClimaSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Obtén el clima seleccionado
        if (e.SelectedItem is ClimaModel clima)
        {
            // Aquí puedes manejar lo que sucede cuando se selecciona un clima
            await DisplayAlert("Clima Seleccionado", $"{clima.name}: {clima.temp} °C", "OK");

            // Deselecciona el elemento
            ((ListView)sender).SelectedItem = null;
        }
    }

}