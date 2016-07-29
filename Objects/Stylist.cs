using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Salon
{
  public class Stylist
  {
    private int _id;
    private string _first_name;
    private string _last_name;
    private string _expertise;

    public Stylist(string FirstName, string LastName, string Expertise, int Id = 0)
    {
      _id = Id;
      _first_name = FirstName;
      _last_name = LastName;
      _expertise = Expertise;
    }

    public override bool Equals(System.Object otherStylist)
    {
      if(!(otherStylist is Stylist))
      {
        return false;
      }
      else
      {
        Stylist newStylist = (Stylist) otherStylist;
        bool idEquality = this.GetId() == newStylist.GetId();
        bool firstNameEquality = this.GetFirstName() == newStylist.GetFirstName();
        bool lastNameEquality = this.GetLastName() == newStylist.GetLastName();
        bool expertiseEquality = this.GetExpertise() == newStylist.GetExpertise();

        return (idEquality && firstNameEquality && lastNameEquality && expertiseEquality);
      }
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

    public string GetExpertise()
    {
      return _expertise;
    }
    public void SetExpertise(string newExpertise)
    {
      _expertise = newExpertise;
    }

    public static List<Stylist> GetAll()
    {
      List<Stylist> allStylists = new List<Stylist>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stylists;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int stylistId = rdr.GetInt32(0);
        string stylistFirstName = rdr.GetString(1);
        string stylistLastName = rdr.GetString(2);
        string stylistExpertise = rdr.GetString(3);

        Stylist newStylist = new Stylist(stylistFirstName, stylistLastName, stylistExpertise, stylistId);
        allStylists.Add(newStylist);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allStylists;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO stylists (first_name, last_name, expertise) OUTPUT INSERTED.id VALUES (@StylistFirstName, @StylistLastName, @StylistExpertise);", conn);

      SqlParameter firstNameParameter = new SqlParameter();
      firstNameParameter.ParameterName = "@StylistFirstName";
      firstNameParameter.Value = this.GetFirstName();

      SqlParameter lastNameParameter = new SqlParameter();
      lastNameParameter.ParameterName = "@StylistLastName";
      lastNameParameter.Value = this.GetLastName();

      SqlParameter expertiseParameter = new SqlParameter();
      expertiseParameter.ParameterName = "@StylistExpertise";
      expertiseParameter.Value = this.GetExpertise();

      cmd.Parameters.Add(firstNameParameter);
      cmd.Parameters.Add(lastNameParameter);
      cmd.Parameters.Add(expertiseParameter);

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

    public static Stylist Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stylists WHERE id = @StylistId;", conn);
      SqlParameter stylistIdParameter = new SqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = id.ToString();

      cmd.Parameters.Add(stylistIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundStylistId = 0;
      string foundStylistFirstName = null;
      string foundStylistLastName = null;
      string foundStylistExpertise = null;

      while (rdr.Read())
      {
        foundStylistId = rdr.GetInt32(0);
        foundStylistFirstName = rdr.GetString(1);
        foundStylistLastName = rdr.GetString(2);
        foundStylistExpertise = rdr.GetString(3);
      }
      Stylist foundStylist = new Stylist(foundStylistFirstName, foundStylistLastName, foundStylistExpertise, foundStylistId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStylist;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM stylists;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
