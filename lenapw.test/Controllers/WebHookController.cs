using lenapw.test.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;

namespace lenapw.test.Controllers
{
    public class WebHookController : ApiController
    {

        public ICollection<Client> Get()
        {
            return new Collection<Client>
                       {
                           new Client { Id = 1, Title = Environment.Version.Build.ToString()},
                           new Client { Id = 2, Title = Environment.Version.Major.ToString()},
                           new Client { Id = 3, Title = Environment.Version.MajorRevision.ToString()},
                           new Client { Id = 4, Title = Environment.Version.Minor.ToString()},
                           new Client { Id = 5, Title = Environment.Version.MinorRevision.ToString()},
                           new Client { Id = 6, Title = Environment.Version.Revision.ToString()},
                           new Client { Id = 7, Title = Environment.Version.ToString()}
                       };
        }
    }
}
