using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.DTOs.Task_Member_DTOs;
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
    public class TaskMemberRepository : ITaskMemberRepository
    {
        public async Task<bool> AddTaskMember(AddTaskMemberDTO dTO)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddTaskMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", dTO.TaskID);
                        cmd.Parameters.AddWithValue("@ProjectMemberID", dTO.ProjectMemberID);
                        cmd.Parameters.AddWithValue("@Permission", (byte)dTO.permission);

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

        public async Task<bool> ChangeTaskMemberPermission(ChangeTaskMemberPermissionDTO dTO)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ChangeTaskMemberPermission", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskMemberID", dTO.TaskMemberID);
                        cmd.Parameters.AddWithValue("@Permission", (byte)dTO.permission);

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

        public async Task<bool> DeleteTaskMember(int TaskMemberID)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DeleteTaskMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaskMemberID", TaskMemberID);

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

        public async Task<int> GetCountTaskMembers(int TaskID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetCountTaskMembers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", TaskID);

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


        public async Task<Permission> GetPermissionOfTaskMember(int UserID, int TaskMemberID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetPermissionOfTaskMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        cmd.Parameters.AddWithValue("@TaskMemberID", TaskMemberID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && byte.TryParse(result.ToString(), out byte permissionValue))
                        {
                            return (Permission)permissionValue;
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

            return Permission.Member;
        }

        public async Task<Permission?> GetPermissionOfTaskMemberByTaskID(int UserID, int TaskID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetPermissionOfTaskMemberByTaskID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        cmd.Parameters.AddWithValue("@TaskID", TaskID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && byte.TryParse(result.ToString(), out byte permissionValue))
                        {
                            return (Permission)permissionValue;
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


        public async Task<IEnumerable<TaskMemberPageDTO>> GetTaskMembersPaged(int PageNumber, int PageSize, int TaskID)
        {
            List<TaskMemberPageDTO> taskMembers = new List<TaskMemberPageDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetTaskMembersPaged", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", PageSize);
                        cmd.Parameters.AddWithValue("@TaskID", TaskID);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                TaskMemberPageDTO member = new TaskMemberPageDTO
                                {
                                    TaskMemberID = reader.GetInt32(reader.GetOrdinal("TaskMemberID")),
                                    permission = (Permission)reader.GetByte(reader.GetOrdinal("Permission")),
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    ImagePath = reader.GetString(reader.GetOrdinal("ImagePath"))
                                };

                                taskMembers.Add(member);
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

            return taskMembers;
        }


        public async Task<bool> IsUserAlreadyInTaskMember(int TaskID, int ProjectMemberID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_IsUserAlreadyInTaskMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskID", TaskID);
                        cmd.Parameters.AddWithValue("@ProjectMemberID", ProjectMemberID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int exists))
                        {
                            return exists == 1;
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

            return false;
        }
    }
}
