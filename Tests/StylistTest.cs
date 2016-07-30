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

    [Fact]
    public void T4_1_Save_AssignsIdToStylist()
    {
      Stylist testStylist = new Stylist("Clementine", "Clips", "L.4 Specialist");
      testStylist.Save();

      Stylist savedStylist = Stylist.GetAll()[0];
      int result = savedStylist.GetId();
      int testId = testStylist.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    //Additional test for GetId as it was built and passed the test from the outset
    public void T4_2_Save_AssignsIdToStylist()
    {
      Stylist testStylist = new Stylist("Clementine", "Clips", "L.4 Specialist");
      testStylist.Save();

      Stylist savedStylist = Stylist.GetAll()[0];
      int result = savedStylist.GetId();

      Assert.Equal(true, (result > 0));
    }

    [Fact]
    public void T5_Find_FindsStylistInDB()
    {
      Stylist testStylist = new Stylist("Clementine", "Clips", "L.4 Specialist");
      testStylist.Save();

      Stylist foundStylist = Stylist.Find(testStylist.GetId());

      Assert.Equal(testStylist, foundStylist);
    }

    [Fact]
    public void T6_Update_UpdatesStylistInDB()
    {
      Stylist testStylist = new Stylist("Jake", "Shears", "L.5 Master");
      testStylist.Save();

      string newExpertise = "Lvl. 15 Grand Master";

      testStylist.Update(newExpertise);

      string resultExpertise = testStylist.GetExpertise();

      Assert.Equal(newExpertise, resultExpertise);
    }

    [Fact]
    public void T7_Delete_DeletesStylistFromDB()
    {
      //Always remember to save to DB (Save())
      Stylist testStylist1 = new Stylist("Clementine", "Clips", "L.4 Specialist");
      testStylist1.Save();
      Stylist testStylist2 = new Stylist("Jake", "Shears", "L.5 Master");
      testStylist2.Save();

      testStylist1.Delete();

      List<Stylist> result = Stylist.GetAll();
      List<Stylist> testStylists = new List<Stylist> {testStylist2};

      Assert.Equal(testStylists, result);
    }

    //TEST FOR GETTING ALL CLIENTS OF A STYLIST
  }
}
