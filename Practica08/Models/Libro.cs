using System;
using System.Data;
using Primosoft.DbUtils;

namespace Practica08.Models
{

    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Isbn { get; set; }
        public string Edicion { get; set; }
        public int EditorialId { get; set; }
        public Editorial Editorial { get; set; }
        public DateTime FechaAdquirido { get; set; }
        public Categoria[] Categorias { get; set; }
        public AutorLibro[] Autores { get; set; }
        public bool Activo { get; set; }
    }

}
