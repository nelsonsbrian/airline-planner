
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;
using System;

namespace AirlinePlanner.Models
{
  public class Flight
  {
    public int Id { get; set; }
    public int Status_Id { get; set; }
    public DateTime Dept_Time { get; set; }
    public int Dept_City_Id { get; set; }
    public DateTime Arr_Time { get; set; }
    public int Arr_City_Id { get; set; }

    public Flight(string name, int status_id, DateTime dept_time, int dept_city_id, DateTime arr_time, int arr_city_id, int id=0)
    {
      Id = id;
      Status_Id = status_id;
      Dept_Time = dept_time;
      Dept_City_Id = dept_city_id;
      Arr_Time = arr_time;
      Arr_City_Id = arr_city_id;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool Status_IdEquality = (this.Status_Id == newFlight.Status_Id);
        bool Dept_TimeEquality = (this.Dept_Time == newFlight.Dept_Time);
        bool Dept_City_IdEquality = (this.Dept_City_Id == newFlight.Dept_City_Id);
        bool Arr_TimeEquality = (this.Arr_Time == newFlight.Arr_Time);
        bool Arr_City_IdEquality = (this.Arr_City_Id == newFlight.Arr_City_Id);

        return (Status_IdEquality && Dept_TimeEquality && Dept_City_IdEquality && Arr_TimeEquality && Arr_City_IdEquality);
      }
    }

    public override int GetHashCode()
    {
      string concat = Id.ToString() + Status_Id.ToString() + Dept_Time.ToString() + Dept_City_Id.ToString() + Arr_Time.ToString() + Arr_City_Id.ToString();
      return this.concat.GetHashCode();
    }

    //id status_id status_name dept_time dept_city_id dept_city_name arr_time arr_city_id arr_city_name

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = List<Flight> {};
      MySqlConnection conn = DB.Connection();
      conn.Open()
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights GROUP BY dept_city_id ORDER BY dept_time ASC;";
      // cmd.CommandText = @"SELECT flights.id, flights.status_id, status.name AS status_name, flights.dept_time, flights.dept_city_id, dept_cities.name AS dept_city_name, flights.arr_time, flights.arr_city_id, arr_cities.name AS arr_city_name FROM flights JOIN cities AS dept_cities ON flights.dept_city_id = dept_cities.id JOIN cities AS arr_cities ON flights.arr_city_id = arr_cities.id JOIN status ON flights.status_id = status.id;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string status = rdr.GetString(9);

        Flight foundFlight = new Flight(name, id);
        allFlights.Add(foundFlight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }

    public static Flight Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights WHERE id=@id;";
      cmd.Parameters.AddWithValue("@id", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      Flight foundFlight = new Flight("name", id);
      while (rdr.Read())
      {
        foundFlight.Name = rdr.GetString(1);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundFlight;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (name) VALUES (@name);";
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

      cmd.CommandText = @"DELETE FROM flights WHERE id=@id;";
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

      cmd.CommandText = @"UPDATE flights SET name=@name WHERE id=@id;";
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
