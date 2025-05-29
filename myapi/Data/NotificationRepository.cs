using System.Data;
using Microsoft.Data.SqlClient;
using myapi.Models;

namespace myapi.Data
{
	public class NotificationRepository
	{
		private readonly Globals _globals;

		public NotificationRepository(Globals globals)
		{
			_globals = globals;
		}

		public IEnumerable<NotificationModel> GetAllNotifications(int id)
		{
			SqlCommand cmd = _globals.Connection();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "PR_Notification_SelectAll";
			cmd.Parameters.AddWithValue("@HostelID", id);
			SqlDataReader reader = cmd.ExecuteReader();

			List<NotificationModel> nm = new List<NotificationModel>();

			while(reader.Read())
			{
				NotificationModel n = new NotificationModel();

				n.NotificationID = reader.GetInt32("NotificationID");
				n.Title = reader.GetString("Title");
				n.Message = reader.GetString("Message");
				n.NoOfDays = reader.GetInt32("NoOfDays");
				n.SentAt = reader.GetDateTime("SentAt");
				n.HostelID = reader.GetInt32("HostelID");
				n.CreatedAt = reader.GetDateTime("CreatedAt");
				n.UpdatedAt = reader.GetDateTime("UpdatedAt");
				n.HostelName = reader.GetString("HostelName");

				nm.Add(n);
			}

			return nm;
		}

		public NotificationModel GetNotificationById(int notificationId)
		{
			SqlCommand cmd = _globals.Connection();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "PR_Notification_SelectByID";
			cmd.Parameters.AddWithValue("@NotificationID", notificationId);

			SqlDataReader reader = cmd.ExecuteReader();

			if (reader.Read())
			{
				NotificationModel n = new NotificationModel
				{
					NotificationID = reader.GetInt32("NotificationID"),
					Title = reader.GetString("Title"),
					Message = reader.GetString("Message"),
					NoOfDays = reader.GetInt32("NoOfDays"),
					SentAt = reader.GetDateTime("SentAt"),
					HostelID = reader.GetInt32("HostelID"),
					CreatedAt = reader.GetDateTime("CreatedAt"),
					UpdatedAt = reader.GetDateTime("UpdatedAt"),
					HostelName = reader.GetString("HostelName")
				};

				return n;
			}

			return null;
		}
		
		public bool InsertNotification(NotificationModel notification)
		{
			try
			{
				SqlCommand cmd = _globals.Connection();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_Notification_Insert";

				cmd.Parameters.AddWithValue("@Title", notification.Title);
				cmd.Parameters.AddWithValue("@Message", notification.Message);
				cmd.Parameters.AddWithValue("@NoOfDays", notification.NoOfDays);
				cmd.Parameters.AddWithValue("@HostelID", notification.HostelID);

				int rowsAffected = cmd.ExecuteNonQuery();

				return rowsAffected > 0;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}
		
		public bool UpdateNotification(int id,NotificationModel notification)
		{
			try
			{
				SqlCommand cmd = _globals.Connection();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_Notification_Update";

				cmd.Parameters.AddWithValue("@NotificationID", id);
				cmd.Parameters.AddWithValue("@Title", notification.Title);
				cmd.Parameters.AddWithValue("@Message", notification.Message);
				cmd.Parameters.AddWithValue("@NoOfDays", notification.NoOfDays);
				cmd.Parameters.AddWithValue("@HostelID", notification.HostelID);

				int rowsAffected = cmd.ExecuteNonQuery();

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}
		
		public bool DeleteNotification(int notificationId)
		{
			try
			{
				SqlCommand cmd = _globals.Connection();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_Notification_Delete";
				cmd.Parameters.AddWithValue("@NotificationID", notificationId);

				int rowsAffected = cmd.ExecuteNonQuery();

				return rowsAffected > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}
	}
}
