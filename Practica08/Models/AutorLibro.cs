using System;
using System.Data;
using Primosoft.DbUtils;

namespace Practica08.Models
{

    public class AutorLibro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }
    }

}
