using System;
using System.Data;
using Practica08.Models;
using Primosoft.DbUtils;

namespace Practica08.DataAccess
{

    public class AutorLibroDao : DaoBase
    {

        public AutorLibroDao(IDataBase db) : base(db) { }

        public AutorLibro GetById(int id) 
        {
            DB.SetCommand("dbo.GetAutorLibroById");
            DB.AddParameter("@id", id);
            var r = DB.ExecuteReader();
            var i = r.Read() ? FromDr(r) : null;
            r.Close();
            return i;
        }

        public AutorLibro[] GetByLibro(int libroId)
        {
            DB.SetCommand("dbo.GetAutoresLibroByLibro");
            DB.AddParameter("@libroId", libroId);
            return ToArray(DB.ExecuteReader());
        }

        public int Insert(AutorLibro i)
        {
            DB.SetCommand("dbo.InsertAutorLibro");
            DB.AddParameter("@nombre", i.Nombre);
            DB.AddParameter("@apellidos", i.Apellidos);
            DB.AddParameter("@libroId", i.LibroId);
            i.Id = (int)DB.ExecuteScalar();
            return i.Id;
        }

        public void Update(AutorLibro i)
        {
            DB.SetCommand("dbo.UpdateAutorLibro");
            DB.AddParameter("@id", i.Id);
            DB.AddParameter("@nombre", i.Nombre);
            DB.AddParameter("@apellidos", i.Apellidos);
            DB.AddParameter("@libroId", i.LibroId);
            DB.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            DB.SetCommand("dbo.DeleteAutorLibro");
            DB.AddParameter("@id", id);
            DB.ExecuteNonQuery();
        }

        public void Delete(AutorLibro i)
        {
            Delete (i.Id);
        }

        private AutorLibro FromDr(IDataReader r)
        {
            var i = new AutorLibro() {
                Id = (int)r["Id"],  // <- Cast!
                Nombre = (string)r["Nombre"],
                Apellidos = r["Apellidos"] as string,  // <- Porque es nullable en DB
                LibroId = (int)r["LibroId"]
            }
            return i;
        }

        private AutorLibro[] ToArray(IDataReader r)
        {
            var lst = new List<AutorLibro>();
            while (r.Read()) lst.Add(FromDr(r));
            r.Close();  // <- IMPORTANT!
            return lst.ToArray();
        }

    }

}