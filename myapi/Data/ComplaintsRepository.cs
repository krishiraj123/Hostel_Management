using System.Data;
using Microsoft.Data.SqlClient;
using myapi.Models;

namespace myapi.Data
{
    public class ComplaintsRepository
    {
        private readonly Globals _global;

        public ComplaintsRepository(Globals global)
        {
            _global = global;
        }

        public IEnumerable<ComplaintsModel> GetAllComplaints(int id)
        {
            using var cmd = _global.Connection();
            cmd.CommandText = "PR_Complaints_SelectAll";
            cmd.Parameters.AddWithValue("@HostelID",id);
            var complaintsModels = new List<ComplaintsModel>();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                complaintsModels.Add(new ComplaintsModel
                {
                    ComplainID = reader.GetInt32("ComplainID"),
                    ComplainSubject = reader.GetString("ComplainSubject"),
                    ComplainBody = reader.GetString("ComplainBody"),
                    ComplainStatus = reader.GetString("ComplainStatus"),
                    HostelID = reader.GetInt32("HostelID"),
                    RoomID = reader.GetInt32("RoomID"),
                    StudentID = reader.GetInt32("StudentID"),                                      
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt"),
                    HostelName = reader.GetString("HostelName"),
                    RoomNumber = reader.GetString("RoomNumber"),
                    StudentName = reader.GetString("StudentName")
                });
            }
            return complaintsModels;
        }

        public IEnumerable<ComplaintsModel> GetAllComplaintsRoomWise(int hostelID,int roomID)
        {
            using var cmd = _global.Connection();
            cmd.CommandText = "PR_Complaints_SelectAllByRoomWise";
            cmd.Parameters.AddWithValue("@HostelID", hostelID);
            cmd.Parameters.AddWithValue("@RoomID", roomID);
            var complaintsModels = new List<ComplaintsModel>();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                complaintsModels.Add(new ComplaintsModel
                {
                    ComplainID = reader.GetInt32("ComplainID"),
                    ComplainSubject = reader.GetString("ComplainSubject"),
                    ComplainBody = reader.GetString("ComplainBody"),
                    ComplainStatus = reader.GetString("ComplainStatus"),
                    HostelID = reader.GetInt32("HostelID"),
                    RoomID = reader.GetInt32("RoomID"),
                    StudentID = reader.GetInt32("StudentID"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt"),
                    HostelName = reader.GetString("HostelName"),
                    RoomNumber = reader.GetString("RoomNumber"),
                    StudentName = reader.GetString("StudentName")
                });
            }
            return complaintsModels;
        }

        public IEnumerable<ComplaintsList> GetResolvedComplaints()
        {
            return GetComplaintsByStatus("PR_Hostel_SelectAllResolvedComplains");
        }

        public IEnumerable<ComplaintsList> GetUnResolvedComplaints()
        {
            return GetComplaintsByStatus("PR_Hostel_SelectAllUnResolvedComplains");
        }

        private IEnumerable<ComplaintsList> GetComplaintsByStatus(string storedProcedure)
        {
            using var cmd = _global.Connection();
            cmd.CommandText = storedProcedure;

            var complaintsLists = new List<ComplaintsList>();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                complaintsLists.Add(new ComplaintsList
                {
                    ComplainID = reader.GetInt32("ComplainID"),
                    StudentID = reader.GetInt32("StudentID"),
                    RoomID = reader.GetInt32("RoomID"),
                    HostelID = reader.GetInt32("HostelID"),
                    ComplainSubject = reader.GetString("ComplainSubject"),
                    ComplainBody = reader.GetString("ComplainBody"),
                    ComplainStatus = reader.GetString("ComplainStatus"),
                    HostelName = reader.GetString("HostelName"),
                    RoomNumber = reader.GetString("RoomNumber"),
                    StudentName = reader.GetString("StudentName")
                });
            }
            return complaintsLists;
        }

        public bool UpdateComplainsStatus(int id,ComplainUpdateStatusModel cm)
        {
            try
            {
                SqlCommand cmd = _global.Connection();
                cmd.CommandText = "PR_Hostel_UpdateComplainsStatus";
                cmd.Parameters.AddWithValue("ComplainID", id);
                cmd.Parameters.AddWithValue("ComplaintStatus", cm.ComplainStatus);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool ComplaintsAdd(ComplainAddEditModel cm)
        {
            try
            {
                SqlCommand cmd = _global.Connection();
                cmd.CommandText = "PR_Student_AddComplaints";
                cmd.Parameters.AddWithValue("HostelID", cm.HostelID);
                cmd.Parameters.AddWithValue("StudentID", cm.StudentID);
                cmd.Parameters.AddWithValue("RoomID", cm.RoomID);
                cmd.Parameters.AddWithValue("ComplainSubject", cm.ComplainSubject);
                cmd.Parameters.AddWithValue("ComplainBody", cm.ComplainBody);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool ComplaintsUpdate(int id,ComplainAddEditModel cm)
        {
            try
            {
                SqlCommand cmd = _global.Connection();
                cmd.CommandText = "PR_Student_UpdateComplaints";
                cmd.Parameters.AddWithValue("ComplainID", id);
                cmd.Parameters.AddWithValue("HostelID", cm.HostelID);
                cmd.Parameters.AddWithValue("StudentID", cm.StudentID);
                cmd.Parameters.AddWithValue("RoomID", cm.RoomID);
                cmd.Parameters.AddWithValue("ComplainSubject", cm.ComplainSubject);
                cmd.Parameters.AddWithValue("ComplainBody", cm.ComplainBody);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool StudentDeleteComplaint(int id)
        {
            try
            {
                SqlCommand cmd = _global.Connection();
                cmd.CommandText = "PR_Student_DeleteComplaints";
                cmd.Parameters.AddWithValue("ComplainID", id!);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool StudentDeleteComplaintPermanently(int id)
        {
            try
            {
                SqlCommand cmd = _global.Connection();
                cmd.CommandText = "PR_Complaints_DeleteComplaints";
                cmd.Parameters.AddWithValue("ComplainID", id!);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public ComplaintsModel GetAllComplaintsByID(int id)
        {
            using var cmd = _global.Connection();
            cmd.CommandText = "PR_Complaints_SelectByID";
            cmd.Parameters.AddWithValue("@ComplaintID",id);

            var complaintsModels = new ComplaintsModel();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                complaintsModels.ComplainID = reader.GetInt32("ComplainID");
                complaintsModels.ComplainSubject = reader.GetString("ComplainSubject");
                complaintsModels.ComplainBody = reader.GetString("ComplainBody");
                complaintsModels.ComplainStatus = reader.GetString("ComplainStatus");
                complaintsModels.HostelID = reader.GetInt32("HostelID");
                complaintsModels.RoomID = reader.GetInt32("RoomID");
                complaintsModels.StudentID = reader.GetInt32("StudentID");
                complaintsModels.CreatedAt = reader.GetDateTime("CreatedAt");
                complaintsModels.UpdatedAt = reader.GetDateTime("UpdatedAt");
                complaintsModels.HostelName = reader.GetString("HostelName");
                complaintsModels.RoomNumber = reader.GetString("RoomNumber");
                complaintsModels.StudentName = reader.GetString("StudentName");             
            }
            return complaintsModels;
        }
    }
}
