using System;
using System.Data;
using Practica08.Models;
using Primosoft.DbUtils;

namespace Practica08.DataAccess
{

    public class CategoriaDao : DaoBase
    {

        public CategoriaDao(IDataBase db) : base(db) { }

        public Categoria GetById(int id) 
        {
            DB.SetCommand("dbo.GetCategoriaById");
            DB.AddParameter("@id", id);
            var r = DB.ExecuteReader();
            var i = r.Read() ? FromDr(r) : null;
            r.Close();
            return i;
        }

        public Categoria[] GetAll()
        {
            DB.SetCommand("dbo.GetCategorias");
            return ToArray(DB.ExecuteReader());
        }

        public int Insert(Categoria i)
        {
            DB.SetCommand("dbo.InsertCategoria");
            DB.AddParameter("@nombre", i.Nombre);
            DB.AddParameter("@activo", i.Activo);
            i.Id = (int)DB.ExecuteScalar();
            return i.Id;
        }

        public void Update(Categoria i)
        {
            DB.SetCommand("dbo.UpdateCategoria");
            DB.AddParameter("@id", i.Id);
            DB.AddParameter("@nombre", i.Nombre);
            DB.AddParameter("@activo", i.Activo);
            DB.ExecuteNonQuery();
        }

        private Categoria FromDr(IDataReader r)
        {
            var i = new Categoria() {
                Id = (int)r["Id"],  // <- Cast!
                Nombre = (string)r["Nombre"],
                Activo = (bool)r["Activo"]
            }
            return i;
        }

        private Categoria[] ToArray(IDataReader r)
        {
            var lst = new List<Categoria>();
            while (r.Read()) lst.Add(FromDr(r));
            r.Close();  // <- IMPORTANT!
            return lst.ToArray();
        }

    }

}