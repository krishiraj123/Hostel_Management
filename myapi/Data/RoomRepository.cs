using System.Data;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using myapi.Models;

namespace myapi.Data
{
	public class RoomRepository
	{
		private readonly Globals _globals;

		public RoomRepository(Globals globals)
		{
			_globals = globals;
		}

		public IEnumerable<RoomModel> GetAllRooms(int id)
		{
			SqlCommand cmd = _globals.Connection();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "PR_Room_SelectAll";
			cmd.Parameters.AddWithValue("@HostelID",id);
			SqlDataReader reader = cmd.ExecuteReader();

			List<RoomModel> roomList = new List<RoomModel>();

			while (reader.Read())
			{
				RoomModel room = new RoomModel
				{
					RoomID = reader.GetInt32("RoomID"),
					RoomNumber = reader.GetString("RoomNumber"),
					RoomCapacity = reader.GetInt32("RoomCapacity"),
					CurrentVacancy = reader.GetInt32("CurrentVacancy"),
					RoomFloor = reader.GetInt32("RoomFloor"),
					RoomRent = reader.GetInt32("RoomRent"),
					RoomType = reader.GetString("RoomType"),
					RoomStatus = reader.GetString("RoomStatus"),			
					HostelID = reader.GetInt32("HostelID"),					
					HostelName = reader.GetString("HostelName")
				};

				roomList.Add(room);
			}

			return roomList;
		}

        public IEnumerable<RoomMatesModel> GetRoomMates(int roomId, int hostelId)
        {
            SqlCommand cmd = _globals.Connection();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Student_RoomMates";
            cmd.Parameters.AddWithValue("@RoomID", roomId);
            cmd.Parameters.AddWithValue("@HostelID", hostelId);
            SqlDataReader reader = cmd.ExecuteReader();

            List<RoomMatesModel> studentList = new List<RoomMatesModel>();

            while (reader.Read())
            {
                RoomMatesModel student = new RoomMatesModel
                {
                    StudentName = reader.GetString("StudentName"),
                    StudentEmail = reader.GetString("StudentEmail"),
                    StudentPhoneNumber = reader.GetString("StudentPhoneNumber"),
                    StudentCity = reader.GetString("StudentCity"),
                    StudentState = reader.GetString("StudentState"),
                    StudentCountry = reader.GetString("StudentCountry"),
                    StudentDOB = reader.GetDateTime("StudentDOB"),
                    EmergencyContactNumber = reader.GetString("EmergencyContactNumber"),
                    GuardianName = reader.GetString("GuardianName"),
                    GuardianPhoneNumber = reader.GetString("GuardianPhoneNumber"),
                    StudentEducationStatus = reader.GetString("StudentEducationStatus"),
                    StudentInstituteName = reader.GetString("StudentInstituteName"),
                    HostelName = reader.GetString("HostelName"),
					ProfileImage = reader.GetString("ProfileImage")
                };

                studentList.Add(student);
            }

            return studentList;
        }

        public RoomModel GetRoomByID(int RoomID)
		{
			SqlCommand cmd = _globals.Connection();
			cmd.CommandText = "PR_Room_SelectByID";
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@RoomID", RoomID);

			SqlDataReader reader = cmd.ExecuteReader();

			RoomModel rm = new RoomModel();

			while (reader.Read())
			{
				rm.RoomID = reader.GetInt32("RoomID");
				rm.RoomNumber = reader.GetString("RoomNumber");
				rm.RoomCapacity = reader.GetInt32("RoomCapacity");
				rm.CurrentVacancy = reader.GetInt32("CurrentVacancy");
				rm.RoomFloor = reader.GetInt32("RoomFloor");
				rm.RoomRent = reader.GetInt32("RoomRent");
				rm.RoomType = reader.GetString("RoomType");
				rm.RoomStatus = reader.GetString("RoomStatus");				
				rm.HostelID = reader.GetInt32("HostelID");				
				rm.HostelName = reader.GetString("HostelName");
			}

			return rm;
		}

		public bool DeleteRoom(int RoomID)
		{
			try
			{
				SqlCommand cmd = _globals.Connection();
				cmd.CommandText = "PR_Room_Delete";
				cmd.Parameters.AddWithValue("@RoomID", RoomID);

				int rowsAffected = cmd.ExecuteNonQuery();

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				return false;
			}
		}

		public bool InsertRoom(RoomAddEditModel rm)
		{
			try
			{
				SqlCommand cmd = _globals.Connection();
				cmd.CommandText = "PR_Room_Insert";

				cmd.Parameters.AddWithValue("@RoomNumber", rm.RoomNumber);
				cmd.Parameters.AddWithValue("@RoomCapacity", rm.RoomCapacity);				
				cmd.Parameters.AddWithValue("@RoomFloor", rm.RoomFloor);
				cmd.Parameters.AddWithValue("@RoomType", rm.RoomType);
				cmd.Parameters.AddWithValue("@RoomStatus", rm.RoomStatus);
				cmd.Parameters.AddWithValue("@RoomRent", rm.RoomRent);
				cmd.Parameters.AddWithValue("@HostelID", rm.HostelID);

				int rowsAffected = cmd.ExecuteNonQuery();

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				return false;
			}
		}

		public bool UpdateRoom(int id,RoomAddEditModel rm)
		{
			try
			{
				SqlCommand cmd = _globals.Connection();
				cmd.CommandText = "PR_Room_Update";

				cmd.Parameters.AddWithValue("@RoomID", id);
				cmd.Parameters.AddWithValue("@RoomNumber", rm.RoomNumber);
				cmd.Parameters.AddWithValue("@RoomCapacity", rm.RoomCapacity);
				cmd.Parameters.AddWithValue("@CurrentVacancy", rm.CurrentVacancy);
				cmd.Parameters.AddWithValue("@RoomFloor", rm.RoomFloor);
				cmd.Parameters.AddWithValue("@RoomType", rm.RoomType);
				cmd.Parameters.AddWithValue("@RoomStatus", rm.RoomStatus);
				cmd.Parameters.AddWithValue("@RoomRent", rm.RoomRent);
				cmd.Parameters.AddWithValue("@HostelID", rm.HostelID);

				int rowsAffected = cmd.ExecuteNonQuery();

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				return false;
			}
		}

		public IEnumerable<AvaiableRoomModel> AvailableRoomList(int id)
		{
			SqlCommand cmd = _globals.Connection();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "PR_Room_AvailableRoomList";
			cmd.Parameters.AddWithValue("@HostelID", id);

			SqlDataReader reader = cmd.ExecuteReader();

			List<AvaiableRoomModel> roomList = new List<AvaiableRoomModel>();

			while(reader.Read())
			{
				AvaiableRoomModel am = new AvaiableRoomModel();

				am.RoomID = reader.GetInt32("RoomID");
				am.RoomNumber = reader.GetString("RoomNumber");

				roomList.Add(am);
			}

			return roomList;
		}

		public IEnumerable<AvaiableRoomModel> AllAvailableRoomList(int HostelID,int RoomID)
		{
			SqlCommand cmd = _globals.Connection();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "PR_Room_AllAvailableRoomList";
			cmd.Parameters.AddWithValue("@HostelID", HostelID);
			cmd.Parameters.AddWithValue("@RoomID",RoomID);

            SqlDataReader reader = cmd.ExecuteReader();

			List<AvaiableRoomModel> roomList = new List<AvaiableRoomModel>();

			while (reader.Read())
			{
				AvaiableRoomModel am = new AvaiableRoomModel();

				am.RoomID = reader.GetInt32("RoomID");
				am.RoomNumber = reader.GetString("RoomNumber");

				roomList.Add(am);
			}

			return roomList;
		}
	}
}

