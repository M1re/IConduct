using IConduct.Models;
using IConduct.Services.Abstractions;
using System.Data.SqlClient;

namespace IConduct.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _connectionString;

        public EmployeeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            Employee employee = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT Id, Name, Enable, ManagerID FROM Employee WHERE Id = @EmployeeId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            employee = new Employee
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Enable = reader.GetBoolean(2),
                                ManagerID = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                            };
                        }
                    }
                }

                if (employee != null)
                {
                    employee.Subordinates = await GetSubordinatesAsync(employee.Id);
                }
            }

            return employee;
        }

        public async Task<bool> EnableEmployeeAsync(int employeeId, bool enable)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Employee SET Enable = @Enable WHERE Id = @EmployeeId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Enable", enable);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        private async Task<List<Employee>> GetSubordinatesAsync(int managerId)
        {
            List<Employee> subordinates = new List<Employee>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT Id, Name, Enable, ManagerID FROM Employee WHERE ManagerID = @ManagerId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ManagerId", managerId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var subordinate = new Employee
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Enable = reader.GetBoolean(2),
                                ManagerID = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                                Subordinates = await GetSubordinatesAsync(reader.GetInt32(0)) // Recursively get subordinates using a new connection
                            };

                            subordinates.Add(subordinate);
                        }
                    }
                }
            }

            return subordinates;
        }
    }
}