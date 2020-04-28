using System;
using System.Data;
using Primosoft.DbUtils;

namespace Practica08.Models
{

    public class Municipio
    {

        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Sigla { get; set; }

        public int Clave { get; set; }
        
        public int EstadoId { get; set; }

        public Estado Estado { get; set; }

    }

}
