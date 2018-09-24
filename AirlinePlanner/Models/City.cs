
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;
using System;

namespace AirlinePlanner.Models
{
  public class City
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public City(string name, int id=0)
    {
      Id = id;
      Name = name;
    }

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool nameEquality = (this.Name == newCity.Name);
        return (nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.Name.GetHashCode();
    }

    public static List<City> GetAll()
    {
      List<City> allCities = List<City> {};
      MySqlConnection conn = DB.Connection();
      conn.Open()
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities ORDER BY name ASC;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        City foundCity = new City(name, id);
        allCities.Add(foundCity);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCities;
    }

    public static City Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities WHERE id=@id;";
      cmd.Parameters.AddWithValue("@id", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      City foundCity = new City("name", id);
      while (rdr.Read())
      {
        foundCity.Name = rdr.GetString(1);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundCity;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";
      cmd.Parameters.AddWithValue("@name", this.Name);

      cmd.ExecuteNonQuery();
      this.Id = (int)cmd.LastInsertedId;
      conn.Close()
      if (conn != null)
      {
        conn.Dispose()
      }
    }

    public static void Delete(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"DELETE FROM cities WHERE id=@id;";
      cmd.Parameters.AddWithValue("@id", id);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Edit(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"UPDATE cities SET name=@name WHERE id=@id;";
      cmd.Parameters.AddWithValue("@name", newName);
      cmd.Parameters.AddWithValue("@id", this.Id);

      cmd.ExecuteNonQuery();
      this.Name = newName;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
