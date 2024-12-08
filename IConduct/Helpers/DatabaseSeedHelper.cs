using System.Data.SqlClient;

namespace IConduct.Helpers
{
    public static class DatabaseSeedHelper
    {
        public static async Task SeedAsync(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var createTableCommand = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employee' AND xtype='U')
            CREATE TABLE Employee (
                ID INT PRIMARY KEY,
                Name NVARCHAR(50) NOT NULL,
                ManagerID INT NULL,
                Enable BIT NOT NULL
            );";

            using var tableCommand = new SqlCommand(createTableCommand, connection);
            await tableCommand.ExecuteNonQueryAsync();

            var insertDataCommand = @"
            IF NOT EXISTS (SELECT 1 FROM Employee WHERE ID = 1)
            BEGIN
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (1, 'Andrey', NULL, 1);
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (2, 'Alexey', 1, 1);    
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (3, 'Roman', 2, 1);     
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (4, 'Olga', 2, 1);      
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (5, 'Dmitry', 3, 1);    
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (6, 'Irina', 3, 1);     
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (7, 'Vera', 4, 1);      
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (8, 'Pavel', 5, 1);     
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (9, 'Svetlana', 6, 1);  
            INSERT INTO Employee (ID, Name, ManagerID, Enable) VALUES (10, 'Victor', 7, 1);   
            END";

            using var dataCommand = new SqlCommand(insertDataCommand, connection);
            await dataCommand.ExecuteNonQueryAsync();
        }
    }
}