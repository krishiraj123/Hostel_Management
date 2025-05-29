using System.Data;
using Microsoft.Data.SqlClient;
using myapi.Models;

namespace myapi.Data
{
    public class PaymentRepository
    {
        private readonly Globals _globals;
        public PaymentRepository(Globals globals) 
        {
            _globals = globals;
        }

        public IEnumerable<PaymentModel> GetPaymentDetails(int id)
        {            
            List<PaymentModel> list = new List<PaymentModel>();
           
            SqlCommand cmd = _globals.Connection();
            cmd.CommandText = "PR_Payment_SelectByHostel";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HostelID", id);
           
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {                    
                    PaymentModel pm = new PaymentModel
                    {
                        PaymentID = reader.GetInt32("PaymentID"),
                        TransactionID = reader.GetString("TransactionID"),
                        Amount = reader.GetDecimal("Amount"),
                        PaymentDate = reader.GetDateTime("PaymentDate"),
                        PaymentStatus = reader.GetString("PaymentStatus"),                        
                        StudentID = reader.GetInt32("StudentID"),
                        StudentName = reader.GetString("StudentName"),
                        StudentEmail = reader.GetString("StudentEmail"),                       
                        StudentEducationStatus = reader.GetString("StudentEducationStatus"),
                        StudentInstituteName = reader.GetString("StudentInstituteName"),
                        StudentPhoneNumber = reader.GetString("StudentPhoneNumber"),
                        RoomID = reader.GetInt32("RoomID"),
                        RoomNumber = reader.GetString("RoomNumber"),
                        RoomCapacity = reader.GetInt32("RoomCapacity"),
                        CurrentVacancy = reader.GetInt32("CurrentVacancy"),
                        RoomRent = reader.GetInt32("RoomRent"),
                        RoomType = reader.GetString("RoomType"),
                        HostelID = reader.GetInt32("HostelID"),
                        HostelName = reader.GetString("HostelName"),
                        HostelContactNumber = reader.GetString("HostelContactNumber"),
                        HostelEmail = reader.GetString("HostelEmail"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        UpdatedAt = reader.GetDateTime("UpdatedAt")
                    };

                    list.Add(pm);
                }
            }

            return list;
        }

        public int CreatePayment(PaymentCreateModel payment)
        {
            using SqlCommand cmd = _globals.Connection();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Payment_Insert";

            cmd.Parameters.AddWithValue("@StudentID", payment.StudentID);
            cmd.Parameters.AddWithValue("@RoomID", payment.RoomID);
            cmd.Parameters.AddWithValue("@HostelID", payment.HostelID);
            cmd.Parameters.AddWithValue("@Amount", payment.Amount);
            
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public IEnumerable<PaymentModel> GetPaymentsByStudent(int studentId)
        {
            using SqlCommand cmd = _globals.Connection();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Payment_GetByStudent";
            cmd.Parameters.AddWithValue("@StudentID", studentId);

            using SqlDataReader reader = cmd.ExecuteReader();
            var payments = new List<PaymentModel>();

            while (reader.Read())
            {
                payments.Add(new PaymentModel
                {
                    PaymentID = reader.GetInt32("PaymentID"),
                    StudentID = reader.GetInt32("StudentID"),
                    RoomID = reader.GetInt32("RoomID"),
                    HostelID = reader.GetInt32("HostelID"),
                    Amount = reader.GetDecimal("Amount"),
                    PaymentDate = reader.GetDateTime("PaymentDate"),
                    PaymentStatus = reader.GetString("PaymentStatus"),                    
                    StudentName = reader.GetString("StudentName"),
                    RoomNumber = reader.GetString("RoomNumber"),
                    HostelName = reader.GetString("HostelName"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                });
            }

            return payments;
        }
        public PaymentModel GetPaymentByID(int paymentId)
        {
            PaymentModel payment = null;
            using SqlCommand cmd = _globals.Connection();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Payment_GetByID";
            cmd.Parameters.AddWithValue("@PaymentID", paymentId);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                payment = new PaymentModel
                {
                    PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),
                    TransactionID = reader.GetString(reader.GetOrdinal("TransactionID")),
                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                    PaymentDate = reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                    PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                    StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                    StudentEmail = reader.GetString(reader.GetOrdinal("StudentEmail")),                   
                    StudentEducationStatus = reader.GetString(reader.GetOrdinal("StudentEducationStatus")),
                    StudentInstituteName = reader.GetString(reader.GetOrdinal("StudentInstituteName")),
                    StudentPhoneNumber = reader.GetString(reader.GetOrdinal("StudentPhoneNumber")),
                    RoomID = reader.GetInt32(reader.GetOrdinal("RoomID")),
                    RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                    RoomCapacity = reader.GetInt32(reader.GetOrdinal("RoomCapacity")),
                    CurrentVacancy = reader.GetInt32(reader.GetOrdinal("CurrentVacancy")),
                    RoomRent = reader.GetInt32(reader.GetOrdinal("RoomRent")),
                    RoomType = reader.GetString(reader.GetOrdinal("RoomType")),
                    HostelID = reader.GetInt32(reader.GetOrdinal("HostelID")),
                    HostelName = reader.GetString(reader.GetOrdinal("HostelName")),
                    HostelContactNumber = reader.GetString(reader.GetOrdinal("HostelContactNumber")),
                    HostelEmail = reader.GetString(reader.GetOrdinal("HostelEmail")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                };
            }
            return payment;
        }
    }
}   
