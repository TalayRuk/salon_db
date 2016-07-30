using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Salon
{
  public class ClientTest : IDisposable
  {
    public ClientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Client.DeleteAll();
    }

    [Fact]
    public void T1_DBEmptyAtFirst()
    {
      int result = Client.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void T2_Equal_ReturnsTrueIfClientIsSame()
    {
      Client firstClient = new Client("Shaggy", "Dew", 1);
      Client secondClient = new Client("Shaggy", "Dew", 1);

      Assert.Equal(firstClient, secondClient);
    }

    [Fact]
    public void T3_Save_SavesToDB()
    {
      Client testClient = new Client("Shaggy", "Dew", 1);
      testClient.Save();

      List<Client> result = Client.GetAll();
      List<Client> testList = new List<Client>{testClient};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void T4_1_Save_AssignsIdToClient()
    {
      Client testClient = new Client("Shaggy", "Dew", 1);
      testClient.Save();

      Client savedClient = Client.GetAll()[0];
      int result = savedClient.GetId();
      int testId = testClient.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void T5_Find_FindsClientInDB()
    {
      Client testClient = new Client("Shaggy", "Dew", 1);
      testClient.Save();

      Client foundClient = Client.Find(testClient.GetId());

      Assert.Equal(testClient, foundClient);
    }

    [Fact]
    public void T6_Update_UpdatesClientInDB()
    {
      Client testClient = new Client("Shaggy", "Dew", 1);
      testClient.Save();

      string newFirst = "Lange";
      string newLast = "Ponyta";
      int newStylistId = 2;

      testClient.Update(newFirst, newLast, newStylistId);

      string result1 = testClient.GetFirstName();
      string result2 = testClient.GetLastName();
      int result3 = testClient.GetStylistId();

      Assert.Equal(newFirst, result1);
      Assert.Equal(newLast, result2);
      Assert.Equal(newStylistId, result3);
    }

    [Fact]
    public void T7_Delete_DeleteClientFromDB()
    {
      Client testClient1 = new Client("Shaggy", "Dew", 1);
      testClient1.Save();
      Client testClient2 = new Client("Lange", "Ponyta", 2);
      testClient2.Save();

      testClient1.Delete();

      List<Client> result = Client.GetAll();
      List<Client> testClients = new List<Client>{testClient2};

      Assert.Equal(testClients, result);
    }
  }
}
