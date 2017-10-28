using pw.lena.Core.Data.Models;
using pw.lena.Core.Data.Models.SQLite;
using pw.lena.CrossCuttingConcerns.Helpers;
using pw.lena.CrossCuttingConcerns.Interfaces;
using System;

namespace pw.lena.Core.Data.Converters
{
    public class ModelConverter
    {
        private ILocalizeService _localizer;

        public ModelConverter(ILocalizeService localizer)
        {
            _localizer = localizer;
        }

        public MasterSQL ConvertToMasterSQL(Master master)
        {
            return new MasterSQL()
            {
                MasterId = master.MasterId,
                ChatId = master.ChatId,
                codeB = master.codeB,
                Name = master.Name,
                TypeDeviceID = master.TypeDeviceID,
                TypeName = master.TypeName,
                IsActive = master.IsActive,
                Date = ConverterHelper.ConvertDateTimeToMillisec(DateTime.Now).ToString()
            };
        }

        public Master ConvertToMaster(MasterSQL master)
        {
            return new Master()
            {
                MasterId = master.MasterId,
                ChatId = master.ChatId,
                codeB = master.codeB,
                Name = master.Name,
                TypeDeviceID = master.TypeDeviceID,
                TypeName = master.TypeName,
                IsActive = master.IsActive
            };
        }

        public PowerPCSQL ConvertToPowerPCSQL(PowerPC powerpc)
        {
            return new PowerPCSQL()
            {              
                IsSynchronized = powerpc.IsSynchronized,
                dateTimeOffPC = ConverterHelper.ConvertDateTimeToMillisec(powerpc.dateTimeOffPC).ToString(),
                dateTimeOnPC = ConverterHelper.ConvertDateTimeToMillisec(powerpc.dateTimeOnPC).ToString(),
                GUID = powerpc.GUID,
                IsActive = powerpc.IsActive,
                Date = ConverterHelper.ConvertDateWithoutTimeToMillisec(DateTime.Now)
            };
        }

        public PowerPC ConvertToPowerPC(PowerPCSQL powerpc)
        {
            return new PowerPC()
            {
                dateTimeOffPC = ConverterHelper.ConvertMillisecToDateTime(powerpc.dateTimeOffPC),
                dateTimeOnPC = ConverterHelper.ConvertMillisecToDateTime(powerpc.dateTimeOnPC),
                GUID = powerpc.GUID,
                IsActive = powerpc.IsActive,
                IsSynchronized = powerpc.IsSynchronized
            };
        }

        public ScreenShotSQL ConvertToScreenShotSQL(ScreenShot screenshot)
        {
            return new ScreenShotSQL()
            {
                dateCreate = ConverterHelper.ConvertDateTimeToMillisec(DateTime.Now).ToString(),
                Date = ConverterHelper.ConvertDateWithoutTimeToMillisec(DateTime.Now),
                GUID = screenshot.GUID,
                QueueCommandID = screenshot.QueueCommandID,
                ImageScreen = screenshot.ImageScreen, //(screenshot.ImageScreen == null) ? null : JsonConvertorExtention.FromJsonString<byte[]>(screenshot.ImageScreen.ToJsonString()),
            IsActive = true,
                IsSynchronized = screenshot.IsSynchronized                
            };
        }

        public ScreenShot ConvertToScreenShot(ScreenShotSQL screenshot)
        {
            return new ScreenShot()
            {                
                dateCreate = ConverterHelper.ConvertMillisecToDateTime(screenshot.dateCreate),
                GUID = screenshot.GUID,
                IsActive = screenshot.IsActive,
                IsSynchronized = screenshot.IsSynchronized,
                QueueCommandID = screenshot.QueueCommandID,
                ImageScreen = screenshot.ImageScreen
            };
        }
    }
}
