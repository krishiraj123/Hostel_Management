using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using myapi.Models;

namespace myapi.Data
{
    public class DashboardRepository
    {
        private readonly Globals _globals;
        public DashboardRepository(Globals globals)
        {
            _globals = globals;
        }

        public async Task<DashboardModel> GetDashboardData()
        {
            DashboardModel dashboardModel = new DashboardModel
            {
                Counts = new List<DashboardCounts>(),
                NewestAdmission = new List<NewestAdmission>(),
                LatestComplaints = new List<LatestComplaints>(),
                ReservationStatistic = new List<ReservationStatistic>(),
            };
            try
            {
                SqlCommand cmd = _globals.Connection();
                cmd.CommandText = "usp_GetDashboardData";

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    //counts
                    while (await reader.ReadAsync())
                    {
                        DashboardCounts counts = new DashboardCounts();
                        counts.Metrics = reader["Metrics"].ToString();
                        counts.Value = Convert.ToInt32(reader["Value"]);

                        dashboardModel.Counts.Add(counts);
                    }

                    if (await reader.NextResultAsync())
                    {
                        //new admissions
                        while (await reader.ReadAsync())
                        {
                            NewestAdmission newestAdmission = new NewestAdmission();
                            newestAdmission.StudentName = reader["StudentName"].ToString();
                            newestAdmission.StudentImage = reader["StudentImage"].ToString();
                            newestAdmission.AdmissionDate = Convert.ToDateTime(reader["AdmissionDate"]);
                            newestAdmission.RoomNumber = reader["RoomNumber"].ToString();
                            newestAdmission.RoomCapacity = Convert.ToInt32(reader["RoomCapacity"]);

                            dashboardModel.NewestAdmission.Add(newestAdmission);
                        }
                    }

                    if (await reader.NextResultAsync())
                    {
                        //latest complaints
                        while (await reader.ReadAsync())
                        {
                            LatestComplaints complaints = new LatestComplaints();
                            complaints.StudentName = reader["StudentName"].ToString();
                            complaints.PostedAt = Convert.ToDateTime(reader["PostedAt"]);
                            complaints.RoomNumber = reader["RoomNumber"].ToString();
                            complaints.ComplaintStatus = reader["ComplaintStatus"].ToString();
                            complaints.ComplainSubject = reader["ComplainSubject"].ToString();
                            complaints.ComplaintBody = reader["ComplaintBody"].ToString();
                            complaints.ProfileImage = reader["ProfileImage"].ToString();

                            dashboardModel.LatestComplaints.Add(complaints);
                        }
                    }

                    if (await reader.NextResultAsync())
                    {
                        //reservation statistic
                        while (await reader.ReadAsync())
                        {
                            ReservationStatistic stats = new ReservationStatistic();                          
                            stats.CountOfStudent = reader["CountOfStudent"].ToString();

                            dashboardModel.ReservationStatistic.Add(stats);
                        }
                    }
                }

                return dashboardModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}

