using System.Collections.Generic;
using System.Data;
using System.IO.Pipelines;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Data.SqlClient;
using myapi.Controllers;
using myapi.Models;

namespace myapi.Data
{
    public class HostelRepository
    {
        private Globals _global;
        private readonly EmailServiceController _emailService;
        public HostelRepository(Globals global,EmailServiceController emailService)
        {
            _global = global;
            _emailService = emailService;
        }

        public IEnumerable<HostelModel> GetAllHostels()
        {
            SqlCommand cmd = _global.Connection();
            cmd.CommandText = "PR_Hostel_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
           
            List<HostelModel> list = new List<HostelModel>();

            while (reader.Read())
            {
                HostelModel model = new HostelModel();
                model.HostelID = reader.GetInt32("HostelID");
                model.HostelName = reader.GetString("HostelName");
                model.HostelAddress = reader.GetString("HostelAddress");
                model.HostelContactNumber = reader.GetString("HostelContactNumber");
                model.HostelEmail = reader.GetString("HostelEmail");
                model.HostelAdminPassword = reader.GetString("HostelAdminPassword");
                model.HostelPincode = reader.GetString("HostelPincode");
                model.HostelCity = reader.GetString("HostelCity");
                model.HostelCountry = reader.GetString("HostelCountry");
                model.HostelState = reader.GetString("HostelState");
                model.HostelCapacity = reader.GetInt32("HostelCapacity");
                model.Amenities = reader.GetString("Amenities");
                model.HostelType = reader.GetString("HostelType");
                model.HostelPolicies = reader.GetString("HostelPolicies");
                model.CreatedAt = reader.GetDateTime("CreatedAt");
                model.UpdatedAt = reader.GetDateTime("UpdatedAt");
                list.Add(model);
            }

            return list;
        }

        public HostelModel GetHostelsByID(int id)
        {
            SqlCommand cmd = _global.Connection();
            cmd.CommandText = "PR_Hostel_SelectByID";
            cmd.Parameters.AddWithValue("@HostelID", id);
            SqlDataReader reader = cmd.ExecuteReader();

            HostelModel list = new HostelModel();

            while (reader.Read())
            {               
                list.HostelID = reader.GetInt32("HostelID");
                list.HostelName = reader.GetString("HostelName");
                list.HostelAddress = reader.GetString("HostelAddress");
                list.HostelContactNumber = reader.GetString("HostelContactNumber");
                list.HostelEmail = reader.GetString("HostelEmail");
                list.HostelAdminPassword = reader.GetString("HostelAdminPassword");
                list.HostelPincode = reader.GetString("HostelPincode");
                list.HostelCity = reader.GetString("HostelCity");
                list.HostelCountry = reader.GetString("HostelCountry");
                list.HostelState = reader.GetString("HostelState");
                list.HostelCapacity = reader.GetInt32("HostelCapacity");
                list.Amenities = reader.GetString("Amenities");
                list.HostelType = reader.GetString("HostelType");
                list.HostelPolicies = reader.GetString("HostelPolicies");
                list.CreatedAt = reader.GetDateTime("CreatedAt");
                list.UpdatedAt = reader.GetDateTime("UpdatedAt");
                list.TotalRooms = reader.GetInt32("TotalRooms");
                list.VacantRooms = reader.GetInt32("VacantRooms");
                list.FullRooms = reader.GetInt32("FullRooms");
            }

            return list;
        }

        public Dictionary<string,dynamic> HostelLogin(HostelLoginModel hm)
        {
            try
            {                
                SqlCommand cmd = _global.Connection();
                cmd.CommandText = "PR_Hostel_Login";
                cmd.Parameters.AddWithValue("UserName", hm.UserName);
                cmd.Parameters.AddWithValue("Password", hm.Password);
                
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var HostelID = reader["HostelID"];
                    var HostelName = reader["HostelName"];

                    Dictionary<string, dynamic> list = new Dictionary<string, dynamic>
                    {
                        {"HostelID",HostelID},
                        {"HostelName",HostelName }
                    };
                    return list ;
                }
                else return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool UpdateHostelPassword(HostelUpdatePasswordModel hm)
        {
            try
            {                
                SqlCommand cmd = _global.Connection();
                
                cmd.CommandText = "PR_Hostel_ChangePassword";
                
                cmd.Parameters.AddWithValue("@HostelID", hm.HostelID);
                cmd.Parameters.AddWithValue("@CurrentPassword", hm.CurrentPassword);
                cmd.Parameters.AddWithValue("@NewPassword", hm.NewPassword);
                cmd.Parameters.AddWithValue("@ConfirmPassword", hm.ConfirmPassword);
                
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
