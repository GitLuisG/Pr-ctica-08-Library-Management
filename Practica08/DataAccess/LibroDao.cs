using System;
using System.Data;
using Practica08.Models;
using Primosoft.DbUtils;

namespace Practica08.DataAccess
{

    public class LibroDao : DaoBase
    {

        public LibroDao(IDataBase db) : base(db) { }

        public Libro GetById(int id) 
        {
            DB.SetCommand("dbo.GetLibroById");
            DB.AddParameter("@id", id);
            var r = DB.ExecuteReader();
            var i = r.Read() ? FromDr(r) : null;
            r.Close();
            if (i != null)
            {
                i.Autores = new AutorLibroDao(DB).GetByLibro(i.Id);
            }
            return i;
        }

        public Libro[] GetByEditorial(int editorialId)
        {
            DB.SetCommand("dbo.GetLibrosByEditorial");
            DB.AddParameter("@editorialId", editorialId);
            var items = ToArray(DB.ExecuteReader());
            var autorLibroDao = new AutorLibroDao(DB);
            foreach (var i in items)
            {
                i.Autores = autorLibroDao.GetByLibro(i.Id);
            }
            return items;
        }

        public int Insert(Libro i)
        {
            DB.SetCommand("dbo.InsertLibro");
            AddInsertParams(i);
            i.Id = (int)DB.ExecuteScalar();
            return i.Id;
        }

        public void Update(Libro i)
        {
            DB.SetCommand("dbo.UpdateLibro");
            DB.AddParameter("@id", i.Id);
            AddInsertParams(i);
            DB.ExecuteNonQuery();
        }

        private void AddInsertParams(Libro i)
        {
            DB.AddParameter("@titulo", i.Titulo);
            DB.AddParameter("@isbn", i.Isbn);
            DB.AddParameter("@edicion", i.Edicion);
            DB.AddParameter("@editoriaId", i.EditorialId);
            DB.AddParameter("@fechaAdquirido", i.FechaAdquirido);
            DB.AddParameter("@activo", i.Activo);
        }

        private Libro FromDr(IDataReader r)
        {
            var i = new Libro() {
                Id = (int)r["Id"],  // <- Cast!
                Titulo = (string)r["Titulo"],
                Isbn = r["Isbn"] as string,
                Edicion = r["Edicion"] as string,
                EditoriaId = (int)r["EditorialId"],
                Editoria = new Editorial() {
                    Id = (int)r["EditoriaId"],
                    Nombre = (string)r["EditorialNombre"],
                    Pais = (string)r["EditorialPais"],
                    Activo = (bool)r["EditorialActivo"]
                },
                FechaAdquirido = (DateTime)r["FechaAdquirido"],
                Activo = (bool)r["Activo"]
            }
            return i;
        }

        private Libro[] ToArray(IDataReader r)
        {
            var lst = new List<Libro>();
            while (r.Read()) lst.Add(FromDr(r));
            r.Close();  // <- IMPORTANT!
            return lst.ToArray();
        }

    }

}