﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carespot.DAL.Interfaces;
using Carespot.Models;

namespace Carespot.DAL.Context
{
    public class VrijwilligerSQLContext : IVrijwilligerContext
    {
        private readonly SqlConnection _con = new SqlConnection("Data Source=WIN-SRV-WEB.fhict.local;Initial Catalog=Carespot;User ID=carespot;Password=Test1234;Encrypt=False;TrustServerCertificate=True");

        public List<Vrijwilliger> RetrieveAll()
        {
                  
                _con.Open();
                var cmdString = "SELECT * FROM Gebruiker g WHERE gebruikerType = 'Vrijwilliger'";
                var command = new SqlCommand(cmdString, _con);
                var reader = command.ExecuteReader();
                var returnList = new List<Vrijwilliger>();
            while (reader.Read())
            {

                Vrijwilliger g = new Vrijwilliger(reader.GetString(1), reader.GetString(2), reader.GetString(9));
                g.Id = reader.GetInt32(0);
                g.Naam = reader.GetString(1);
                g.Wachtwoord = reader.GetString(2);
                g.Geslacht = (Gebruiker.GebruikerGeslacht)Enum.Parse(typeof(Gebruiker.GebruikerGeslacht), reader.GetString(3));
                g.Straat = reader.GetString(4);
                g.Huisnummer = reader.GetString(5);
                g.Postcode = reader.GetString(6);
                g.Plaats = reader.GetString(7);
                g.Land = reader.GetString(8);
                g.Email = reader.GetString(9);
                g.Telefoonnummer = reader.GetString(10);
                g.Type = (Gebruiker.GebruikerType)Enum.Parse(typeof(Gebruiker.GebruikerType), reader.GetString(11));
               // g.Foto = reader.GetString(12);
                returnList.Add(g);

            }
            _con.Close();          
            return returnList;
        }

        public void CreateVrijwilliger(Vrijwilliger v)
        {
            object newID;
         
                string query = "INSERT INTO Gebruiker (naam, wachtwoord, geslacht, straat, huisnummer, postcode, plaats, land, email, telefoonnummer, gebruikerType, foto) VALUES(@naam,@wachtwoord,@geslacht,@straat,@huisnummer,@postcode,@plaats,@land,@email,@telefoonnummer,@gebruikerType,NULL);SELECT CAST(scope_identity() AS int)";
                SqlCommand cmd = new SqlCommand(query, _con);
                
                    _con.Open();
                    cmd.Parameters.AddWithValue("@naam", v.Naam);
                    cmd.Parameters.AddWithValue("@wachtwoord", v.Wachtwoord);
                    cmd.Parameters.AddWithValue("@geslacht", v.Geslacht);
                    cmd.Parameters.AddWithValue("@straat", v.Straat);
                    cmd.Parameters.AddWithValue("@huisnummer", v.Huisnummer);
                    cmd.Parameters.AddWithValue("@postcode", v.Postcode);
                    cmd.Parameters.AddWithValue("@plaats", v.Plaats);
                    cmd.Parameters.AddWithValue("@land", v.Land);
                    cmd.Parameters.AddWithValue("@email", v.Email);
                    cmd.Parameters.AddWithValue("@telefoonnummer", v.Telefoonnummer);
                    cmd.Parameters.AddWithValue("@gebruikerType", v.Type);


                    newID = (int)cmd.ExecuteScalar();

                    if (newID != null && DBNull.Value != newID)
                    {
                        string query1 = "INSERT INTO Vrijwilliger VALUES (@newID)";
                        SqlCommand command1 = new SqlCommand(query1, _con);
                        command1.Parameters.AddWithValue("@newID", newID);               
                        command1.ExecuteScalar();
                    }

                    _con.Close();
          

            /*
            int newID = 0;
            try
            {
              string query = "INSERT INTO Gebruiker(naam, wachtwoord, geslacht, straat, huisnummer, postcode, plaats, land, email, telefoonnummer, gebruikerType, foto)" +
                "VALUES(" + v.Naam + "," + v.Wachtwoord + "," + v.Geslacht + "," + v.Straat + "," + v.Huisnummer + "," +
                v.Postcode + "," + v.Plaats + "," + v.Land + "," + v.Email + "," + v.Telefoonnummer + "," + v.Type + "," +
                v.Foto + ")SELECT CAST(scope_identity() AS int);";

                _con.Open();
                using (SqlCommand cmd = new SqlCommand(query, _con))
                {
                            
                    newID = (int)cmd.ExecuteScalar();               
                    if (newID != null)
                    {
                        string query1 = "INSERT INTO Vrijwilliger (gebruikerId)VALUES(" + newID + ");";
                        SqlCommand command1 = new SqlCommand(query1, _con);
                        command1.ExecuteScalar();
                    }

                   
                }
                _con.Close();
            }
            catch (Exception ex)
            {
                //
            }         
            */
        }

        public Vrijwilliger RetrieveVrijwilliger(int id)
        {
            _con.Open();
            var cmdString = "SELECT * FROM Gebruiker g WHERE gebruikerType = 'Vrijwilliger' and id=" + id;
            var command = new SqlCommand(cmdString, _con);
            var reader = command.ExecuteReader();

            Vrijwilliger g = null;

            while (reader.Read())
            {

                g = new Vrijwilliger(reader.GetString(1), reader.GetString(2), reader.GetString(9));
                g.Id = reader.GetInt32(0);
                g.Naam = reader.GetString(1);
                g.Wachtwoord = reader.GetString(2);
                g.Geslacht = (Gebruiker.GebruikerGeslacht)Enum.Parse(typeof(Gebruiker.GebruikerGeslacht), reader.GetString(3));
                g.Straat = reader.GetString(4);
                g.Huisnummer = reader.GetString(5);
                g.Postcode = reader.GetString(6);
                g.Plaats = reader.GetString(7);
                g.Land = reader.GetString(8);
                g.Email = reader.GetString(9);
                g.Telefoonnummer = reader.GetString(10);
                g.Type = (Gebruiker.GebruikerType)Enum.Parse(typeof(Gebruiker.GebruikerType), reader.GetString(11));
                // g.Foto = reader.GetString(12);
             
            }
            _con.Close();
            return g;
        }

        public void UpdateVrijwilliger(Vrijwilliger v)
        {
            throw new NotImplementedException();
        }

        public void DeleteVrijwilliger(int id)
        {
            _con.Open();
            var cmdString = "DELETE FROM Gebruiker WHERE id =" + id;
            var command = new SqlCommand(cmdString, _con);
            command.ExecuteNonQuery();
            command.CommandText = "DELETE FROM Vrijwilliger WHERE gebruikerId =" + id;
            command.ExecuteNonQuery();
            _con.Close();
        }
    }
}
