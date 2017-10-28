
INSERT INTO [dbo].[bCommands]
           ([commandID]
           ,[name])
     VALUES
           (1
           ,'Where R U')
		   
		   INSERT INTO [dbo].[bCommands]
           ([commandID]
           ,[name])
     VALUES
           (2
           ,'Rename Device')
		   INSERT INTO [dbo].[bCommands]
           ([commandID]
           ,[name])
     VALUES
           (3
           ,'Delete Device')
		   INSERT INTO [dbo].[bCommands]
           ([commandID]
           ,[name])
     VALUES
           (4
           ,'STATUS')
		   INSERT INTO [dbo].[bCommands]
           ([commandID]
           ,[name])
     VALUES
           (5
           ,'<< Back to Device List')
		   INSERT INTO [dbo].[bCommands]
           ([commandID]
           ,[name])
     VALUES
           (6
           ,'Time Start up')
		   INSERT INTO [dbo].[bCommands]
           ([commandID]
           ,[name])
     VALUES
           (7
           ,'Time Start up 3days')
INSERT INTO [dbo].[bCommands]
           ([commandID]
           ,[name])
     VALUES
           (8
           ,'SCREENSHOT')
		   
---------------------------------------------------
INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])
     VALUES	 
           (1
           ,1
           ,1
           ,1)

INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   	
     VALUES
           (1
           ,2
           ,2
           ,1)

INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])
     VALUES	 
           (1
           ,3
           ,4
           ,1)

INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])
     VALUES	 	
           (1
           ,4
           ,3
           ,1)

INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   
     VALUES
           (1
           ,5
           ,5
           ,1)

		   INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   
     VALUES
           (4
           ,6
           ,1
           ,1)

			INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   
     VALUES
           (4
           ,7
           ,2
           ,1)    

		   

		    INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   
     VALUES
           (4
           ,4
           ,3
           ,1)
		   
		   
		   INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   
     VALUES
           (4
           ,8
           ,4
           ,1)
		   
		   INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   
     VALUES
           (4
           ,3
           ,5
           ,1)
		   
		   
		   INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   
     VALUES
           (4
           ,2
           ,6
           ,1)
		   
		   		   INSERT INTO [dbo].[bDeviceMenu]
           ([TypeDeviceID]
           ,[commandID]
           ,[OrderPlace]
           ,[isActive])		   
     VALUES
           (4
           ,5
           ,7
           ,1)

--=================================================

-------------------------------------------------------
INSERT INTO [dbo].[bDeviceStatus]
           ([name])
     VALUES
           ('online')
INSERT INTO [dbo].[bDeviceStatus]
           ([name])
     VALUES
           ('offline')
INSERT INTO [dbo].[bDeviceStatus]
           ([name])
     VALUES
           ('removed')
--------------------------------------------

INSERT INTO [dbo].[bTypeDevice]
           ([Name])
     VALUES
           ('Android Slave')


INSERT INTO [dbo].[bTypeDevice]
           ([Name])
     VALUES
           ('TelegramBot Master')


INSERT       
INTO              bTypeDevice (Name)
VALUES        ('Android Master')

INSERT       
INTO              bTypeDevice (Name)
VALUES        ('WindowsPC Slave')
-----------------------------------