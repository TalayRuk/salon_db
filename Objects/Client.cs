using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Salon
{
  public class Client
  {
    private int _id;
    private string _first_name;
    private string _last_name;

    public Client(string FirstName, string LastName, int Id = 0)
    {
      _id = Id;
      _first_name = FirstName;
      _last_name = LastName;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetFirstName()
    {
      return _first_name;
    }
    public void SetFirstName(string newFirst)
    {
      _first_name = newFirst;
    }

    public string GetLastName()
    {
      return _last_name;
    }
    public void SetLastName(string newLast)
    {
      _last_name = newLast;
    }

    public string GetFullName()
    {
      string name = _first_name + " " + _last_name;
      return name;
    }

    public static List<Client> GetAll()
    {
      List<Client> allClients = new List<Client>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM clients;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int clientId = rdr.GetInt32(0);
        string clientFirstName = rdr.GetString(1);
        string clientLastName = rdr.GetString(2);

        Client newClient = new Client(clientFirstName, clientLastName, clientId);
        allClients.Add(newClient);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allClients;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM clients;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
