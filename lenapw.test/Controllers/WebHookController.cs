using lenapw.test.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;


using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot.Types;
using File = System.IO.File;
using Telegram.Bot.Types.Enums;

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

        public ICollection<Client> Get(int id)
        {
            //http://localhost:18442/api/webhook/5
            //http://localhost:18442/api/webhook/get/5

           
            {
                return new Collection<Client>
                       {
                            {
                                new Client { Id = id, Title = "wrong" }
                            }
                       };
            }   
        }

        public async Task<IHttpActionResult> Post(Update update)
        {
            var message = update.Message;          

            if (message.Type == MessageType.TextMessage)
            {
                // Echo each Message
               Message x = await Bot.Api.SendTextMessage(message.Chat.Id, message.Text);
            }
            else if (message.Type == MessageType.PhotoMessage)
            {
                // Download Photo
                var file = await Bot.Api.GetFile(message.Photo.LastOrDefault()?.FileId);

                var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                using (var saveImageStream = File.Open(filename, FileMode.Create))
                {
                    await file.FileStream.CopyToAsync(saveImageStream);
                }

                Message x = await Bot.Api.SendTextMessage(message.Chat.Id, "Thx for the Pics");
            }

            return Ok();
        }
    }
}
