using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Salon
{
  public class StylistTest : IDisposable
  {
    public StylistTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Stylist.DeleteAll();
    }

    [Fact]
    public void T1_DBEmptyAtFirst()
    {
      int result = Stylist.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void T2_Equal_ReturnsTrueIfStylistIsSame()
    {
      Stylist firstStylist = new Stylist("Clementine", "Clips", "L.4 Specialist");
      Stylist secondStylist = new Stylist("Clementine", "Clips", "L.4 Specialist");

      Assert.Equal(firstStylist, secondStylist);
    }

    [Fact]
    public void T3_Save_SavesToDB()
    {
      Stylist testStylist = new Stylist("Clementine", "Clips", "L.4 Specialist");
      testStylist.Save();

      List<Stylist> result = Stylist.GetAll();
      List<Stylist> testList = new List<Stylist>{testStylist};

      Assert.Equal(testList, result);
    }
  }
}
