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

      Console.WriteLine(result[0].GetFirstName());
      Console.WriteLine(result[0].GetLastName());
      Console.WriteLine(result[0].GetStylistId());

      Console.WriteLine(testList[0].GetFirstName());
      Console.WriteLine(testList[0].GetLastName());
      Console.WriteLine(testList[0].GetStylistId());

      Assert.Equal(testList, result);
    }
  }
}
