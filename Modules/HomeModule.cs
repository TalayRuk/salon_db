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

      Get["/clients/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Stylist selectedStylist = Stylist.Find(parameters.id);
        List<Client> stylistClients = selectedStylist.GetClients();
        model.Add("stylist", selectedStylist);
        model.Add("clients", stylistClients);
        return View["clients.cshtml", model];
      };
    }
  }
}
