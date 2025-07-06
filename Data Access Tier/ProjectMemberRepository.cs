using Interface_Tier.DTOs.Project_Member_DTOs;
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
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        public async Task<bool> AddProjectMember(AddProjectMemberRepoDTO memberDTO)
        {
            bool isAdded = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddProjectMember_WithChatIfAdminOrOwner", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProjectID", memberDTO.ProjectID);
                        cmd.Parameters.AddWithValue("@UserID", memberDTO.UserID);
                        cmd.Parameters.AddWithValue("@Permission", (byte)memberDTO.Permission);

                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        isAdded = rowsAffected > 0;
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

            return isAdded;
        }
        public async Task<int> GetCountProjectMembers(int projectID)
        {
            int count = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetCountProjectMembers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProjectID", projectID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {
                            count = Convert.ToInt32(result);
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

        public async Task<Permission?> GetPermissionForProjectMember(int userID, int projectID)
        {
            Permission? permission = null;
            
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetPermissionForProjectMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();
                        if (result != null && byte.TryParse(result.ToString(), out byte value))
                        {
                            permission = (Permission)value;
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

            return permission;
        }

        public async Task<Permission> GetPermissionForProjectMember(int ProjectMemberID)
        {
            Permission permission = Permission.Member;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetPermissionForProjectMemberByMemberID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProjectMemberID", ProjectMemberID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();
                        if (result != null && byte.TryParse(result.ToString(), out byte value))
                        {
                            permission = (Permission)value;
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

            return permission;
        }

        public async Task<IEnumerable<ProjectMemberPageDTO>> GetProjectMembersPaged(int projectID, int pageNumber, int pageSize)
        {
            List<ProjectMemberPageDTO> members = new List<ProjectMemberPageDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetProjectMembersPaged", conn))
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
                                members.Add(new ProjectMemberPageDTO
                                {
                                    ProjectMemberID = reader.GetInt32(reader.GetOrdinal("ProjectMemberID")),
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    Permission = (Permission)reader.GetByte(reader.GetOrdinal("Permission")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    ImagePath = reader.GetString(reader.GetOrdinal("ImagePath"))
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

            return members;
        }

        public async Task<bool> IsUserAlreadyInProject(int userID, int projectID)
        {
            bool exists = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_IsUserAlreadyInProject", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();
                        if (result != null && int.TryParse(result.ToString(), out int value))
                        {
                            exists = value == 1;
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

            return exists;
        }

        public async Task<bool> RemoveProjectMember(int projectMemberID)
        {
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_RemoveProjectMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProjectMemberID", projectMemberID);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        success = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return success;
        }

        public async Task<bool> SetProjectMemberPermission(int projectMemberID, Permission permission)
        {
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_SETProjectMemberPermission", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProjectMemberID", projectMemberID);
                        cmd.Parameters.AddWithValue("@Permission", (byte)permission);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        success = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return success;
        }

        public async Task<IEnumerable<ProjectMemberPageDTO>> GetProjectMembersOutsideTaskMember(int pageNumber, int pageSize, int projectID, int taskID)
        {
            var list = new List<ProjectMemberPageDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetProjectMembersOutsideTaskMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);
                        cmd.Parameters.AddWithValue("@TaskID", taskID);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var dto = new ProjectMemberPageDTO
                                {
                                    ProjectMemberID = reader.GetInt32(reader.GetOrdinal("ProjectMemberID")),
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    Permission = (Permission)reader.GetByte(reader.GetOrdinal("Permission")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath"))
                                };

                                list.Add(dto);
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

            return list;
        }

        public async Task<int> GetCountProjectMembersOutsideTaskMember(int ProjectID, int TaskID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetCountProjectMembersOutsideTaskMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                        cmd.Parameters.AddWithValue("@TaskID", TaskID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int totalCount))
                        {
                            return totalCount;
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

    }
}
