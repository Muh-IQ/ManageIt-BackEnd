using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_DTO;
using Interface_Tier.DTOs.Task_DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Utiltiy;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Tier
{
    public class TaskRepository : ITaskRepository
    {
        public async Task<bool> AddTask(AddTaskDTO dTO)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", dTO.Name);
                        cmd.Parameters.AddWithValue("@Description", dTO.Description);
                        cmd.Parameters.AddWithValue("@CreationDate", dTO.CreationDate);
                        cmd.Parameters.AddWithValue("@CreatedBy", dTO.CreatedBy);
                        cmd.Parameters.AddWithValue("@TaskListID", dTO.TaskListID);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isSuccess = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteTask(int TaskID)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DeleteTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaskID", TaskID);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        isSuccess = rowsAffected > 0;
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

            return isSuccess;
        }

        public async Task<Permission?> GetPermissionOfTask(int TaskID, int UserID)
        {
            Permission? permission = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetPermissionOfTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", TaskID);
                        cmd.Parameters.AddWithValue("@UserID", UserID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int permValue))
                        {
                            permission = (Permission)permValue;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return permission;
        }

        public async Task<IEnumerable<TaskStatusesDTO>> GetTaskStatuses()
        {
            var statuses = new List<TaskStatusesDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetTaskStatuses", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                statuses.Add(new TaskStatusesDTO
                                {
                                    ID = reader.GetInt32("ID"),
                                    Name = reader.GetString("Name")
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

            return statuses;
        }

        public async Task<bool> ResetStartDateTask(int TaskID)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ResetStartDateTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaskID", TaskID);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isSuccess = rowsAffected > 0;
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

            return isSuccess;
        }
        public async Task<bool> ResetDeliveryDateTask(int TaskID)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ResetDeliveryDateTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaskID", TaskID);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isSuccess = rowsAffected > 0;
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

            return isSuccess;
        }
        public async Task<bool> UpdateTask(UpdateTaskDTO dTO)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", dTO.TaskID);
                        cmd.Parameters.AddWithValue("@Name", dTO.Name);
                        cmd.Parameters.AddWithValue("@Description", dTO.Description);
                        cmd.Parameters.AddWithValue("@TaskStatusID", dTO.TaskStatusID);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isSuccess = rowsAffected > 0;
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

            return isSuccess;
        }
        public async Task<GetTaskDTO> GetTask(int UserID, int TaskID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", TaskID);
                        cmd.Parameters.AddWithValue("@UserID", UserID);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                return new GetTaskDTO
                                {
                                    TasksName = reader.GetString(reader.GetOrdinal("TasksName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate"))
                                                ? (DateTime?)null
                                                : reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                    DeliveryDate = reader.IsDBNull(reader.GetOrdinal("ActualDeliveryDate"))
                                                   ? (DateTime?)null
                                                   : reader.GetDateTime(reader.GetOrdinal("ActualDeliveryDate")),
                                    TaskStatusID = reader.GetInt32(reader.GetOrdinal("TaskStatusID")),
                                    StatusName = reader.GetString(reader.GetOrdinal("StatusName")),
                                    permission = reader.IsDBNull(reader.GetOrdinal("Permission"))
                                                   ? null
                                                   : (Permission)reader.GetByte("Permission"),
                                };
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

            return null;
        }
        public async Task<bool> SetDeliveryDateTask(int TaskID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_SetDeliveryDateTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", TaskID);
                        cmd.Parameters.AddWithValue("@DeliveryDate", DateTime.Now);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
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

            return false;
        }

        public async Task<bool> SetStartDateTask(int TaskID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_SetStartDateTask", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", TaskID);
                        cmd.Parameters.AddWithValue("@StartDate", DateTime.Now);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
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

            return false;
        }
    }
}
