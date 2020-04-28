using System;
using System.Data;
using Primosoft.DbUtils;

namespace Practica08.Models
{

    public class EstadoDao : DaoBase
    {

        public EstadoDao(IDataBase db) : base(db) { }

        public Estado GetById(int id)
        {
            DB.SetCommand("dbo.GetEstadoById");
            DB.AddParameter("@id", id);
            var r = DB.ExecuteReader();
            var i = r.Read() ? FromDr(r) : null;
            r.Close();
            return i;
        }

        

    }

}
