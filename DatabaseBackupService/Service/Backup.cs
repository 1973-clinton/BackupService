using System;
using System.Data.SqlClient;


namespace DatabaseBackupService.Service
{
    public static class Backup 
    {
        
        public static void RunQuery()
        {
            try
            {
                string connectionString = @"Server=DESKTOP-VOTUKPG\SQLEXPRESS;Database=ACH; User Id=jeadmin;Password=jeadmin;Trusted_Connection=True;MultipleActiveResultSets=true";
                string queryString = $"DECLARE @name NVARCHAR(256)" +
                                     $"\nDECLARE @path NVARCHAR(512)" +
                                     $"\nDECLARE @fileName NVARCHAR(512)" +
                                     $"\nDECLARE @fileDate NVARCHAR(40)" +
                                     $"\n SET @path = 'D:\\Services\\Backups\\' " +
                                     $"\nSELECT @fileDate = CONVERT(NVARCHAR(20),GETDATE(),112) " +
                                     $"\nDECLARE db_cursor CURSOR READ_ONLY FOR  " +
                                     $"\nSELECT name" +
                                     $"\nFROM master.sys.databases" +
                                     $"\nWHERE name NOT IN ('master','model','msdb','tempdb')" +
                                     $"\nAND state = 0" +
                                     $"\nAND is_in_standby = 0 -- database is not read only for log shipping" +
                                     $"\nOPEN db_cursor " +
                                     $"\nFETCH NEXT FROM db_cursor INTO @name " +
                                     $"\nWHILE @@FETCH_STATUS = 0 " +
                                     $"\nBEGIN" +
                                     $"\nSET @fileName = @path + @name + '_' + @fileDate + '.bak' " +
                                     $"\nBACKUP DATABASE @name TO DISK = @fileName " +
                                     $"\nFETCH NEXT FROM db_cursor INTO @name " +
                                     $"\nEND" +
                                     $"\nCLOSE db_cursor" +
                                     $"\nDEALLOCATE db_cursor ";
                
                using SqlConnection connection = new SqlConnection(
                    connectionString);
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
           
        }
    }
}
