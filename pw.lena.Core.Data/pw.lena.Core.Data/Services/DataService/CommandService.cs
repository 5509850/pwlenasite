using pw.lena.Core.Data.Services.DataService.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Interfaces;
using pw.lena.Core.Data.Services.WebServices;
using pw.lena.CrossCuttingConcerns.Helpers;

namespace pw.lena.Core.Data.Services.DataService
{
    public class CommandService : ICommandService
    {
        private IConfiguration configuration;
        private ILocalizeService localizservice;
        private string CRC = "ASDASDYHRdasf";
        
        public CommandService(         
          IConfiguration configuration,          
          ILocalizeService localizservice)
        {            
            this.configuration = configuration;            
            this.localizservice = localizservice;
        }

        public async Task<IEnumerable<Command>> GetCommandRest(DeviceModel device)
        {
            string result = String.Empty;            
            List<Command> listCommand = null;
            try
            {
                RestService restService = new RestService(configuration.RestServerUrl, "GetCommand");
                restService.Timeout = configuration.ServerTimeOut;
                listCommand = await restService.Get<Command>
                    (new CodeRequest
                    {
                        AndroidIDmacHash = device.AndroidIDmacHash,
                        CRC = this.CRC,
                        TypeDeviceID = device.TypeDeviceID
                    });               
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions[0].Data.Count > 0)
                {
                    result = e.InnerExceptions[0].Data["message"].ToString();
                }
                else
                {
                    result = "undefinedException";
                }
            }
            catch (Exception e)
            {
                result = String.Format("Error = " + e.Message);
            }
            
            return await TaskHelper.Complete(listCommand);
        }
    }
}
