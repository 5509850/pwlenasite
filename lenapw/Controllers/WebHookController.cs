using lenapw.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;

namespace lenapw.Controllers
{
    public class WebHookController : ApiController
    {

        public ICollection<Client> Get()
        {
            return new Collection<Client>
                       {
                           new Client { Id = 1, Title = "ClientRequest 1"},
                           new Client { Id = 2, Title = "ClientRequest 2"},
                           new Client { Id = 3, Title = "ClientRequest 3"}
                       };
        }

    }
}
