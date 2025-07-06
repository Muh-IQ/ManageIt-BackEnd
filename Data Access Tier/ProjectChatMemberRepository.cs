using Interface_Tier.DTOs.Project_Chat_Member_DTOs;
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
    public class ProjectChatMemberRepository : IProjectChatMemberRepository
    {
        public async Task<int> GetCountProjectChatMembers(int projectID)
        {
            int count = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetCountProjectChatMembers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int value))
                        {
                            count = value;
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

        public async Task<IEnumerable<ProjectChatMemberPageDTO>> GetProjectChatMembersPaged(int ProjectID, int pageNumber, int pageSize)
        {
            List<ProjectChatMemberPageDTO> members = new List<ProjectChatMemberPageDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetProjectChatMembersPaged", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProjectChatMemberPageDTO member = new ProjectChatMemberPageDTO
                                {
                                    ChatMemberID = reader.GetInt32(reader.GetOrdinal("ChatMemberID")),
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    Permission = (Permission)reader.GetByte(reader.GetOrdinal("Permission")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    ImagePath = reader.GetString(reader.GetOrdinal("ImagePath"))
                                };

                                members.Add(member);
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

    }
}
