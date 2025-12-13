using Microsoft.Data.SqlClient;
using System.Data;

namespace myapi
{
    public class GlobalsDBUtils
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GlobalsDBUtils> _logger;

        public GlobalsDBUtils(IConfiguration configuration, ILogger<GlobalsDBUtils> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<SqlDataReader> ExecuteStoredProcedureAsync(string commandText, SqlParameter[] parameters = null)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString"));
                await connection.OpenAsync();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = commandText;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing Async Reader for: {commandText}");
                throw;
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = commandType;
                        command.CommandText = commandText;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        return await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error executing Async NonQuery for: {commandText}");
                    return -1;
                }
            }
        }
    }
}