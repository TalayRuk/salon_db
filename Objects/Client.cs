using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Salon
{
  public class Client
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private int _stylistId;

    public Client(string FirstName, string LastName, int StylistId, int Id = 0)
    {
      _id = Id;
      _firstName = FirstName;
      _lastName = LastName;
      _stylistId = StylistId;
    }

    public override bool Equals(System.Object otherClient)
    {
      if (!(otherClient is Client))
      {
        return false;
      }
      else
      {
        Client newClient = (Client) otherClient;
        bool idEquality = this.GetId() == newClient.GetId();
        bool firstNameEquality = this.GetFirstName() == newClient.GetFirstName();
        bool lastNameEquality = this.GetLastName() == newClient.GetLastName();
        bool stylistIdEquality = this.GetStylistId() == newClient.GetStylistId();

        return (idEquality && firstNameEquality && lastNameEquality && stylistIdEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetFirstName()
    {
      return _firstName;
    }
    public void SetFirstName(string newFirst)
    {
      _firstName = newFirst;
    }

    public string GetLastName()
    {
      return _lastName;
    }
    public void SetLastName(string newLast)
    {
      _lastName = newLast;
    }

    public int GetStylistId()
    {
      return _stylistId;
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
        int clientStylistId = rdr.GetInt32(3);

        Client newClient = new Client(clientFirstName, clientLastName, clientStylistId, clientId);
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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO clients (first_name, last_name, stylist_id) OUTPUT INSERTED.id VALUES (@ClientFirst, @ClientLast, @ClientStylistId);", conn);

      SqlParameter firstNameParameter = new SqlParameter();
      firstNameParameter.ParameterName = "@ClientFirst";
      firstNameParameter.Value = this.GetFirstName();

      SqlParameter lastNameParameter = new SqlParameter();
      lastNameParameter.ParameterName = "@ClientLast";
      lastNameParameter.Value = this.GetLastName();

      SqlParameter clientStylistIdParameter = new SqlParameter();
      clientStylistIdParameter.ParameterName = "@ClientStylistId";
      clientStylistIdParameter.Value = this.GetStylistId();

      cmd.Parameters.Add(firstNameParameter);
      cmd.Parameters.Add(lastNameParameter);
      cmd.Parameters.Add(clientStylistIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Client Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM clients WHERE id = @ClientId;", conn);
      SqlParameter clientIdParameter = new SqlParameter();
      clientIdParameter.ParameterName = "@ClientId";
      clientIdParameter.Value = id.ToString();

      cmd.Parameters.Add(clientIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundClientId = 0;
      string foundClientFirstName = null;
      string foundClientLastName = null;
      int foundClientStylistId = 0;

      while (rdr.Read())
      {
        foundClientId = rdr.GetInt32(0);
        foundClientFirstName = rdr.GetString(1);
        foundClientLastName = rdr.GetString(2);
        foundClientStylistId = rdr.GetInt32(3);
      }
      Client foundClient = new Client(foundClientFirstName, foundClientLastName, foundClientStylistId, foundClientId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundClient;
    }
    public void Update(string newFirst, string newLast, int newStylistId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      //Discovered how to output all (*) will allow you to select which column by index in Reader below
      SqlCommand cmd = new SqlCommand("UPDATE clients SET first_name = @NewFirst, last_name = @NewLast, stylist_id = @NewStylistId OUTPUT INSERTED.* WHERE id = @ClientId;", conn);

      SqlParameter newFirstParameter = new SqlParameter();
      newFirstParameter.ParameterName = "@NewFirst";
      newFirstParameter.Value = newFirst;

      SqlParameter newLastParameter = new SqlParameter();
      newLastParameter.ParameterName = "@NewLast";
      newLastParameter.Value = newLast;

      SqlParameter newStylistIdParameter = new SqlParameter();
      newStylistIdParameter.ParameterName = "@NewStylistId";
      newStylistIdParameter.Value = newStylistId;

      SqlParameter clientIdParameter = new SqlParameter();
      clientIdParameter.ParameterName = "@ClientId";
      clientIdParameter.Value = this.GetId();

      cmd.Parameters.Add(newFirstParameter);
      cmd.Parameters.Add(newLastParameter);
      cmd.Parameters.Add(newStylistIdParameter);
      cmd.Parameters.Add(clientIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._firstName = rdr.GetString(1);
        this._lastName = rdr.GetString(2);
        this._stylistId = rdr.GetInt32(3);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM clients WHERE id = @ClientId;", conn);

      SqlParameter clientIdParameter = new SqlParameter();
      clientIdParameter.ParameterName = "@ClientId";
      clientIdParameter.Value = this.GetId();

      cmd.Parameters.Add(clientIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
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
