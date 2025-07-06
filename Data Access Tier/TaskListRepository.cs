using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_DTOs;
using Interface_Tier.DTOs.Task_List_DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Utiltiy;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Tier
{
    public class TaskListRepository : ITaskListRepository
    {
        public async Task<bool> AddTaskList(AddTaskListDTO dTO)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddTaskList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", dTO.Name);
                        cmd.Parameters.AddWithValue("@ProjectID", dTO.ProjectID);
                        cmd.Parameters.AddWithValue("@CreatedBy", dTO.CreatedBy);

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

        public async Task<bool> DeleteTasksList(int TaskListID)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DeleteTasksList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TasksListID", TaskListID);

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

        public async Task<Permission> GetPermissionOfTaskList(int UserID, int TaskListID)
        {
            Permission permission = Permission.Member; 

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetPermissionOfTaskList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        cmd.Parameters.AddWithValue("@TaskListID", TaskListID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int perm))
                        {
                            permission = (Permission)perm;
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

            return permission;
        }

        public async Task<IEnumerable<TaskListWithTasksDTO>> GetTaskListsWithTasks(int UserID, int ProjectID)
        {
            var result = new Dictionary<int, TaskListWithTasksDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetTasksListAndTask", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int taskListID = reader.GetInt32(reader.GetOrdinal("TaskListID"));

                            // TaskList
                            if (!result.ContainsKey(taskListID))
                            {
                                result[taskListID] = new TaskListWithTasksDTO
                                {
                                    TaskListID = taskListID,
                                    TaskListName = reader.GetString(reader.GetOrdinal("TaskListName"))
                                };
                            }

                            // Task 
                            if (!reader.IsDBNull(reader.GetOrdinal("TaskID")))
                            {
                                var task = new TaskDTO
                                {
                                    TaskID = reader.GetInt32(reader.GetOrdinal("TaskID")),
                                    MembershipStatus = reader.GetInt32(reader.GetOrdinal("MembershipStatus")),
                                    TaskName = reader.GetString(reader.GetOrdinal("TaskName"))
                                };

                                result[taskListID].Tasks.Add(task);
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
                Console.WriteLine($"Error: {ex.Message}");
            }

            return result.Values;
        }


        public async Task<bool> UpdateTaskList(UpdateTaskListDTO dTO)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateTaskList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", dTO.Name);
                        cmd.Parameters.AddWithValue("@ProjectID", dTO.ProjectID);
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
    }
}
