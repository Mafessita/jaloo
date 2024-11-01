

using ApiClima.Data;
using ApiClima.Dependency;

namespace ApiClima
{
    public partial class App : Application
    {

        public static PersonaDataBase PersonaDataBase { get; private set; }

        public App()
        {
            InitializeComponent();

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WeatherData.db3");
            PersonaDataBase = new PersonaDataBase(dbPath); // Inicializar la base de datos

            MainPage = new NavigationPage(new MainPage());
        }


    }
}
