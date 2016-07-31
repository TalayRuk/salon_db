using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Salon
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Stylist> allStylists = Stylist.GetAll();
        return View["index.cshtml", allStylists];
      };

      Post["/stylist/new"] = _ => {
        Stylist newStylist = new Stylist( Request.Form["new-stylist-first"],
                                          Request.Form["new-stylist-last"],
                                          Request.Form["new-stylist-expertise"]);
        newStylist.Save();
        List<Stylist> allStylists = Stylist.GetAll();
        return View["index.cshtml", allStylists];
      };

      Post["/clients/{id}"] = parameters => {
        Client newClient = new Client(    Request.Form["new-client-first"],
                                          Request.Form["new-client-last"],
                                          Request.Form["new-client-stylist-id"]);
        newClient.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        Stylist selectedStylist = Stylist.Find(parameters.id);
        List<Client> stylistClients = selectedStylist.GetClients();
        model.Add("stylist", selectedStylist);
        model.Add("clients", stylistClients);
        return View["clients.cshtml", model];
      };

      Get["/clients/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Stylist selectedStylist = Stylist.Find(parameters.id);
        List<Client> stylistClients = selectedStylist.GetClients();
        model.Add("stylist", selectedStylist);
        model.Add("clients", stylistClients);
        return View["clients.cshtml", model];
      };

      Get["/stylist/edit/{id}"] = parameters => {
        Stylist selectedStylist = Stylist.Find(parameters.id);
        return View["stylist_edit.cshtml", selectedStylist];
      };

      Patch["/{id}"] = parameters => {
        Stylist selectedStylist = Stylist.Find(parameters.id);
        selectedStylist.Update(Request.Form["update-stylist-expertise"]);
        List<Stylist> allStylists = Stylist.GetAll();
        return View["index.cshtml", allStylists];
      };

      Delete["/{id}"] = parameters => {
        Stylist selectedStylist = Stylist.Find(parameters.id);
        selectedStylist.DeleteStylistClients();
        selectedStylist.Delete();
        List<Stylist> allStylists = Stylist.GetAll();
        return View["index.cshtml", allStylists];
      };

      Get["/client/edit/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Stylist> allStylists = Stylist.GetAll();
        Client currentClient = Client.Find(parameters.id);
        Stylist currentStylist = Stylist.Find(currentClient.GetStylistId());
        model.Add("stylists", allStylists);
        model.Add("client", currentClient);
        model.Add("stylist", currentStylist);
        return View["client_edit.cshtml", model];
      };

      Patch["/clients/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Client selectedClient = Client.Find(parameters.id);
        selectedClient.Update(  Request.Form["update-client-first"],
                                Request.Form["update-client-last"],
                                Request.Form["update-client-stylist-id"]);
        Stylist selectedStylist = Stylist.Find(selectedClient.GetStylistId());
        List<Client> stylistClients = selectedStylist.GetClients();
        model.Add("stylist", selectedStylist);
        model.Add("clients", stylistClients);
        return View["clients.cshtml", model];
      };

      Delete["/clients/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Client selectedClient = Client.Find(parameters.id);
        Stylist selectedStylist = Stylist.Find(selectedClient.GetStylistId());
        selectedClient.Delete();
        List<Client> stylistClients = selectedStylist.GetClients();
        model.Add("stylist", selectedStylist);
        model.Add("clients", stylistClients);
        return View["clients.cshtml", model];
      };
    }
  }
}
