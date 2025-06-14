﻿using System.Data;
using System.IO.Pipelines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using myapi.Controllers;
using myapi.Models;

namespace myapi.Data
{
    public class StudentRepository
    {
        private readonly Globals _globals;
        private readonly EmailServiceController _emailService;
        public StudentRepository(Globals globals, EmailServiceController _emailService) 
        { 
            this._globals = globals;
            this._emailService = _emailService;
        }

        public Dictionary<string,dynamic> StudentLogin(StudentLoginModel sm)
        {
            try
            {               
                using SqlCommand cmd = _globals.Connection();
                cmd.CommandText = "PR_Student_Login";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", sm.UserName);
                cmd.Parameters.AddWithValue("@Password", sm.Password);

                using SqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.Read())
                {
                    var HostelID = reader["HostelID"];
                    var StudentID = reader["StudentID"];
                    var RoomID = reader["RoomID"];
                    var StudentName = reader["StudentName"];
                    var RoomNumber = reader["RoomNumber"];
                    var RoomRent = reader["RoomRent"];
                    var HostelName = reader["HostelName"].ToString().Trim();
                    var ProfileImage = reader["ProfileImage"].ToString();

                    Dictionary<string, object> list = new Dictionary<string, object>
                    {
                        { "HostelID", HostelID },
                        { "StudentID", StudentID },
                        { "RoomID", RoomID },
                        { "StudentName", StudentName },
                        {"RoomNumber", RoomNumber},
                        {"RoomRent",RoomRent},
                        {"HostelName",HostelName },
                        {"ProfileImage",ProfileImage }
                    };
                    
                    return list; 
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null; 
            }
        }


        public IEnumerable<StudentModel> SelectAllStudent(int id)
        {
            SqlCommand cmd = _globals.Connection();
            cmd.CommandText = "PR_Student_SelectAll";
            cmd.Parameters.AddWithValue("@HostelID", id);
            SqlDataReader reader = cmd.ExecuteReader();

            List<StudentModel> students = new List<StudentModel>();

            while (reader.Read())
            {
                StudentModel student = new StudentModel();
                student.StudentID = reader.GetInt32("StudentID");
                student.StudentName = reader.GetString("StudentName");
                student.StudentPhoneNumber = reader.GetString("StudentPhoneNumber");
                student.StudentEmail = reader.GetString("StudentEmail");
                student.StudentAddress = reader.GetString("StudentAddress");
                student.StudentGender = reader.GetString("StudentGender");
                student.StudentDOB = reader.GetDateTime("StudentDOB");
                student.StudentEducationStatus = reader.GetString("StudentEducationStatus");
                student.StudentInstituteName = reader.GetString("StudentInstituteName");
                student.EmergencyContactNumber = reader.GetString("EmergencyContactNumber");
                student.StudentCity = reader.GetString("StudentCity");
                student.StudentState = reader.GetString("StudentState");
                student.StudentCountry = reader.GetString("StudentCountry");
                student.StudentPincode = reader.GetString("StudentPincode");
                student.GuardianName = reader.GetString("GuardianName");
                student.GuardianPhoneNumber = reader.GetString("GuardianPhoneNumber");
                student.AdmissionDate = reader.GetDateTime("AdmissionDate");
                student.ProfileImage = reader.GetString("ProfileImage");
                student.StudentPassword = reader.GetString("StudentPassword");
                student.RoomID = reader.GetInt32("RoomID");
                student.HostelID = reader.GetInt32("HostelID");
                student.IsDeleted = reader.GetBoolean("IsDeleted");
                student.CreatedAt = reader.GetDateTime("CreatedAt");
                student.UpdatedAt = reader.GetDateTime("UpdatedAt");
                student.HostelName = reader.GetString("HostelName");
                student.RoomNumber = reader.GetString("RoomNumber");
                students.Add(student);
            }
            return students;
        }

        public bool StudentDelete(int id)
        {
            try
            {
                SqlCommand cmd = _globals.Connection();
                cmd.CommandText = "PR_Student_Delete";
                cmd.Parameters.AddWithValue("StudentID", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
                Console.WriteLine(ex.Message);
            }
        }

        public StudentModel StudentSelectByID(int id)
        {
            try
            {                
                using SqlCommand cmd = _globals.Connection();
                cmd.CommandText = "PR_Student_SelectByID";
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@StudentID", id);
                
                using SqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.Read())
                {
                    return new StudentModel
                    {
                        StudentID = reader.IsDBNull("StudentID") ? null : reader.GetInt32("StudentID"),
                        StudentName = reader.GetString("StudentName"),
                        StudentPhoneNumber = reader.GetString("StudentPhoneNumber"),
                        StudentEmail = reader.GetString("StudentEmail"),
                        StudentAddress = reader.GetString("StudentAddress"),
                        StudentGender = reader.GetString("StudentGender"),
                        StudentDOB = reader.GetDateTime("StudentDOB"),
                        StudentEducationStatus = reader.GetString("StudentEducationStatus"),
                        StudentInstituteName = reader.GetString("StudentInstituteName"),
                        EmergencyContactNumber = reader.GetString("EmergencyContactNumber"),
                        StudentCity = reader.GetString("StudentCity"),
                        StudentState = reader.GetString("StudentState"),
                        StudentCountry = reader.GetString("StudentCountry"),
                        StudentPincode = reader.GetString("StudentPincode"),
                        GuardianName = reader.GetString("GuardianName"),
                        GuardianPhoneNumber = reader.GetString("GuardianPhoneNumber"),
                        AdmissionDate = reader.GetDateTime("AdmissionDate"),
                        ProfileImage = reader.GetString("ProfileImage"),
                        StudentPassword = reader.GetString("StudentPassword"),
                        RoomID = reader.GetInt32("RoomID"),
                        HostelID = reader.GetInt32("HostelID"),
                        IsDeleted = reader.GetBoolean("IsDeleted"),
                        HostelName = reader.GetString("HostelName"),
                        RoomNumber = reader.GetString("RoomNumber"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        UpdatedAt = reader.GetDateTime("UpdatedAt")
                    };
                }
                
                return null;
            }
            catch (Exception ex)
            {               
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public bool StudentInsert(StudentAddEditModel student)
        {
            try
            {
                SqlCommand cmd = _globals.Connection();
                cmd.CommandText = "PR_Student_Insert";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentName", student.StudentName);
                cmd.Parameters.AddWithValue("@StudentPhoneNumber", student.StudentPhoneNumber);
                cmd.Parameters.AddWithValue("@StudentEmail", student.StudentEmail);
                cmd.Parameters.AddWithValue("@StudentAddress", student.StudentAddress);
                cmd.Parameters.AddWithValue("@StudentGender", student.StudentGender);
                cmd.Parameters.AddWithValue("@StudentDOB", student.StudentDOB);
                cmd.Parameters.AddWithValue("@StudentEducationStatus", student.StudentEducationStatus);
                cmd.Parameters.AddWithValue("@StudentInstituteName", student.StudentInstituteName);
                cmd.Parameters.AddWithValue("@EmergencyContactNumber", student.EmergencyContactNumber);
                cmd.Parameters.AddWithValue("@StudentCity", student.StudentCity);
                cmd.Parameters.AddWithValue("@StudentState", student.StudentState);
                cmd.Parameters.AddWithValue("@StudentCountry", student.StudentCountry);
                cmd.Parameters.AddWithValue("@StudentPincode", student.StudentPincode);
                cmd.Parameters.AddWithValue("@GuardianName", student.GuardianName);
                cmd.Parameters.AddWithValue("@GuardianPhoneNumber", student.GuardianPhoneNumber);
                cmd.Parameters.AddWithValue("@AdmissionDate", student.AdmissionDate);
                cmd.Parameters.AddWithValue("@ProfileImage", student.ProfileImage);
                cmd.Parameters.AddWithValue("@RoomID", student.RoomID);
                cmd.Parameters.AddWithValue("@HostelID", student.HostelID);
                
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {					
					SqlCommand passwordCmd = _globals.Connection();
                    passwordCmd.CommandType = CommandType.Text;
					passwordCmd.CommandText = "SELECT StudentPassword FROM Student WHERE StudentEmail = @StudentEmail";
					passwordCmd.Parameters.AddWithValue("@StudentEmail", student.StudentEmail);

					SqlDataReader reader = passwordCmd.ExecuteReader();
					if (reader.Read())
					{
						string updatedPassword = reader.GetString("StudentPassword");
						
						_emailService.SendLoginEmail(student.StudentEmail, updatedPassword);
					}
					reader.Close();
				}

				return rowsAffected > 0;
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool StudentUpdate(int id, StudentAddEditModel student)
        {
            try
            {
                SqlCommand cmd = _globals.Connection();
                cmd.CommandText = "PR_Student_Update";
                cmd.CommandType = CommandType.StoredProcedure;
               
                cmd.Parameters.AddWithValue("@StudentID", id);
                cmd.Parameters.AddWithValue("@StudentName", student.StudentName);
                cmd.Parameters.AddWithValue("@StudentPhoneNumber", student.StudentPhoneNumber);
                cmd.Parameters.AddWithValue("@StudentEmail", student.StudentEmail);
                cmd.Parameters.AddWithValue("@StudentAddress", student.StudentAddress);
                cmd.Parameters.AddWithValue("@StudentGender", student.StudentGender);
                cmd.Parameters.AddWithValue("@StudentDOB", student.StudentDOB);
                cmd.Parameters.AddWithValue("@StudentEducationStatus", student.StudentEducationStatus);
                cmd.Parameters.AddWithValue("@StudentInstituteName", student.StudentInstituteName);
                cmd.Parameters.AddWithValue("@EmergencyContactNumber", student.EmergencyContactNumber);
                cmd.Parameters.AddWithValue("@StudentCity", student.StudentCity);
                cmd.Parameters.AddWithValue("@StudentState", student.StudentState);
                cmd.Parameters.AddWithValue("@StudentCountry", student.StudentCountry);
                cmd.Parameters.AddWithValue("@StudentPincode", student.StudentPincode);
                cmd.Parameters.AddWithValue("@GuardianName", student.GuardianName);
                cmd.Parameters.AddWithValue("@GuardianPhoneNumber", student.GuardianPhoneNumber);
                cmd.Parameters.AddWithValue("@AdmissionDate", student.AdmissionDate);
                cmd.Parameters.AddWithValue("@ProfileImage", student.ProfileImage);
                cmd.Parameters.AddWithValue("@RoomID", student.RoomID);
                cmd.Parameters.AddWithValue("@HostelID", student.HostelID);
                
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool UpdateStudentPassword(StudentUpdatePasswordModel sm)
        {
            try
            {
                SqlCommand cmd = _globals.Connection();

                cmd.CommandText = "PR_Student_ChangePassword";

                cmd.Parameters.AddWithValue("@StudentID", sm.StudentID);
                cmd.Parameters.AddWithValue("@CurrentPassword", sm.CurrentPassword);
                cmd.Parameters.AddWithValue("@NewPassword", sm.NewPassword);
                cmd.Parameters.AddWithValue("@ConfirmPassword", sm.ConfirmPassword);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
