using System;
using System.Data;
using Practica08.Models;
using Primosoft.DbUtils;

namespace Practica08.DataAccess
{

    public class UsuarioDao : DaoBase
    {
        public UsuarioDao(IDataBase db) : base(db) { }

        public int Insert(Usuario i)
        {
            DB.SetCommand("dbo.InsertUsuario");
            DB.AddParameter("@username", i.Username);
            DB.AddParameter("@password", i.Password);
            DB.AddParameter("@nombre", i.Nombre);
            DB.AddParameter("@apellidos", i.Apellidos);
            DB.AddParameter("@email", i.Email);
            i.Id = (int)DB.ExecuteScalar();
            return i.Id;
        }

        public void Update(Usuario i)
        {
            DB.SetCommand("dbo.UpdateUsuario");
            DB.AddParameter("@id", i.Id);
            DB.AddParameter("@username", i.Username);
            DB.AddParameter("@password", i.Password);
            DB.AddParameter("@nombre", i.Nombre);
            DB.AddParameter("@apellidos", i.Apellidos);
            DB.AddParameter("@email", i.Email);
            DB.ExecuteNonQuery();
        }

    }

}