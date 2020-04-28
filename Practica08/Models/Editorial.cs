using System;
using System.Data;
using Primosoft.DbUtils;

namespace Practica08.Models
{

    public class Editorial
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public bool Activo { get; set; }        
    }

}
