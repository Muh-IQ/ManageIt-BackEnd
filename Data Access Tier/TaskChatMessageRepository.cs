using Interface_Tier.DTOs.Task_Chat_Message_DTOs;
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
    public class TaskChatMessageRepository : ITaskChatMessageRepository
    {
        public async Task<bool> AddMessage(int userId, int taskId, string message)
        {
            bool isInserted = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddTaskChatMessage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaskID", taskId);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@Message", message);

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

        public async Task<int> GetCountTaskChatMessagesPaged(int taskId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetCountTaskChatMessages", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", taskId);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int count))
                        {
                            return count;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return 0; 
        }


        public async Task<IEnumerable<TaskChatMessagePageDTOs>> GetTaskChatMessagesPaged(int taskId, int pageNumber, int pageSize)
        {
            List<TaskChatMessagePageDTOs> messages = new List<TaskChatMessagePageDTOs>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetTaskChatMessagesPaged", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", taskId);
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                messages.Add(new TaskChatMessagePageDTOs
                                {
                                    TaskChatMessageID = reader.GetInt32(reader.GetOrdinal("TaskChatMessageID")),
                                    Message = reader.GetString(reader.GetOrdinal("Message")),
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
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return messages;
        }

    }
}
