using Interface_Tier.DTOs.Project_Chat_Message_DTOs;
using Interface_Tier.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Tier
{
    public class ProjectChatMessageRepository : IProjectChatMessageRepository
    {
        public async Task<bool> AddMessage(int userId, int projectId, string message)
        {
            bool isInserted = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddProjectChatMessage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@Message", message ?? (object)DBNull.Value);

                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isInserted = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return isInserted;
        }
        public async Task<int> GetCountProjectChatMessage(int projectID)
        {
            int count = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetCountProjectChatMessages", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();
                        if (result != null && int.TryParse(result.ToString(), out int parsedCount))
                        {
                            count = parsedCount;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return count;
        }
        public async Task<IEnumerable<ProjectChatMessagePageDTOs>> GetProjectChatMessagesPaged(int projectID, int pageNumber, int pageSize)
        {
            List<ProjectChatMessagePageDTOs> messages = new List<ProjectChatMessagePageDTOs>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetProjectChatMessagesPaged", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                messages.Add(new ProjectChatMessagePageDTOs
                                {
                                    ProjectChatMessageID = reader.GetInt32(reader.GetOrdinal("ProjectChatMessageID")),
                                    Message = reader["Message"]?.ToString(),
                                    SentAt = reader.GetDateTime(reader.GetOrdinal("SentAt")),
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID"))
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return messages;
        }
    }
}
