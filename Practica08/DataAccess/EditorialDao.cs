using System;
using System.Data;
using Practica08.Models;
using Primosoft.DbUtils;

namespace Practica08.DataAccess
{

    public class EditorialDao : DaoBase
    {

        public EditorialDao(IDataBase db) : base(db) { }

        public Editorial GetById(int id) 
        {
            DB.SetCommand("dbo.GetEditorialById");
            DB.AddParameter("@id", id);
            var r = DB.ExecuteReader();
            var i = r.Read() ? FromDr(r) : null;
            r.Close();
            return i;
        }

        public Editorial[] GetAll()
        {
            DB.SetCommand("dbo.GetEditoriales");
            return ToArray(DB.ExecuteReader());
        }

        public int Insert(Editoria i)
        {
            DB.SetCommand("dbo.InsertEditorial");
            DB.AddParameter("@nombre", i.Nombre);
            DB.AddParameter("@pais", i.Pais);
            DB.AddParameter("@activo", i.Activo);
            i.Id = (int)DB.ExecuteScalar();
            return i.Id;
        }

        public void Update(Editorial i)
        {
            DB.SetCommand("UpdateEditorial");
            DB.AddParameter("@id", i.Id);
            DB.AddParameter("@nombre", i.Nombre);
            DB.AddParameter("@pais", i.Pais);
            DB.AddParameter("@activo", i.Activo);
            DB.ExecuteNonQuery();
        }

        private Editorial FromDr(IDataReader r)
        {
            var i = new Editorial() {
                Id = (int)r["Id"],  // <- Cast!
                Nombre = (string)r["Nombre"],
                Pais = (string)r["Pais"],
                Activo = (bool)r["Activo"]
            }
            return i;
        }

        private Editorial[] ToArray(IDataReader r)
        {
            var lst = new List<Editorial>();
            while (r.Read()) lst.Add(FromDr(r));
            r.Close();  // <- IMPORTANT!
            return lst.ToArray();
        }

    }

}