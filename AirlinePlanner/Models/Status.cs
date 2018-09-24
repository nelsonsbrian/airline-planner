
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;
using System;

namespace AirlinePlanner.Models
{
  public enum EnumStatus { "On time", "Delayed", "Cancelled", "Complete" };
  public class Status
  {
    public int Id { get; set; }
    public EnumStatus Name { get; set; }

    public Status(string name, int id=0)
    {
      Id = id;
      Name = (EnumStatus)Enum.Parse(typeof(EnumStatus), name);
    }

    public override bool Equals(System.Object otherStatus)
    {
      if (!(otherStatus is Status))
      {
        return false;
      }
      else
      {
        Status newStatus = (Status) otherStatus;
        bool nameEquality = (this.Name == newStatus.Name);
        return (nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.Name.GetHashCode();
    }

    public static List<Status> GetAll()
    {
      List<Status> allStatus = List<Status> {};
      MySqlConnection conn = DB.Connection();
      conn.Open()
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM status ORDER BY name ASC;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        Status foundStatus = new Status(name, id);
        allStatus.Add(foundStatus);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStatus;
    }

    public static Status Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM status WHERE id=@id;";
      cmd.Parameters.AddWithValue("@id", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      Status foundStatus = new Status("name", id);
      while (rdr.Read())
      {
        foundStatus.Name = rdr.GetString(1);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundStatus;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO status (name) VALUES (@name);";
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

      cmd.CommandText = @"DELETE FROM status WHERE id=@id;";
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

      cmd.CommandText = @"UPDATE status SET name=@name WHERE id=@id;";
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
