using Interface_Tier.DTOs.Project_DTOs;
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
    public class ProjectRepository : IProjectRepository
    {
        public async Task<bool> AddProject(AddProjectDTO projectDTO)
        {
            bool isAdded = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddProject", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", projectDTO.Name);
                        cmd.Parameters.AddWithValue("@Description", projectDTO.Description);
                        cmd.Parameters.AddWithValue("@Requirements", projectDTO.Requirements);
                        cmd.Parameters.AddWithValue("@UserID", projectDTO.UserID);

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

        public async Task<int> GetCountUserProjects(int userID)
        {
            int count = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetCountUserProjects", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", userID);

                        await conn.OpenAsync();
                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int projectCount))
                        {
                            count = projectCount;
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

        public async Task<ProjectDetailsDTO> GetProjectDetails(int projectID)
        {
            ProjectDetailsDTO projectDetails = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetProjectDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                projectDetails = new ProjectDetailsDTO
                                {
                                    ProjectID = reader.GetInt32(reader.GetOrdinal("ProjectID")),
                                    ProjectName = reader.GetString(reader.GetOrdinal("ProjectName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Requirements = reader.GetString(reader.GetOrdinal("Requirements")),
                                    ProjectStatusID = reader.GetInt32(reader.GetOrdinal("ProjectStatusID")),
                                    ProjectStatuseName = reader.GetString(reader.GetOrdinal("ProjectStatuseName"))
                                };
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

            return projectDetails;
        }

        public async Task<IEnumerable<ProjectStatuseDTO>> GetProjectStatuses()
        {
            List<ProjectStatuseDTO> statuses = new List<ProjectStatuseDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetProjectStatuses", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var status = new ProjectStatuseDTO
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                };

                                statuses.Add(status);
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

            return statuses;
        }


        public async Task<IEnumerable<UserProjectsPagedDTO>> GetUserProjectsPaged(int userID, int pageNumber, int pageSize )
        {
            List<UserProjectsPagedDTO> projects = new List<UserProjectsPagedDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetUserProjectsPaged", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                projects.Add(new UserProjectsPagedDTO
                                {
                                    ProjectsID = reader.GetInt32(reader.GetOrdinal("ProjectsID")),
                                    ProjectsName = reader.GetString(reader.GetOrdinal("ProjectsName")),
                                    ProjectStatuseName = reader.GetString(reader.GetOrdinal("ProjectStatuseName")),
                                    ProjectStatuseID = reader.GetInt32(reader.GetOrdinal("ProjectStatuseID")),
                                    Permission = reader.GetByte(reader.GetOrdinal("Permission"))
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

            return projects;
        }

        public async Task<bool> UpdateProject(UpdateProjectDTO projectDTO)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateProject", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProjectID", projectDTO.ProjectsID);
                        cmd.Parameters.AddWithValue("@Name", projectDTO.Name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Description", projectDTO.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Requirements", projectDTO.Requirements ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ProjectStatusID", projectDTO.ProjectStatuseID);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isUpdated = rowsAffected > 0;
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

            return isUpdated;
        }

    }
}
