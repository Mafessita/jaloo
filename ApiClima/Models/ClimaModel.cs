using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClima.Models
{
    public class ClimaModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Esto permite que SQLite autoincremente el Id
        public string name { get; set; }
        public double temp { get; set; }
        public string condition { get; set; }
        // Agrega más propiedades según sea necesario
    }


}
