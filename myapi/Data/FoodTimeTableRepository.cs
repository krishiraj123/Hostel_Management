using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using myapi.Models;
using System.Data;
using OfficeOpenXml;

namespace myapi.Data
{
    public class FoodTimeTableRepository
    {
        private readonly Globals _global;        

        public FoodTimeTableRepository(Globals global)
        {
            _global = global;            
        }

        public List<FoodTimeTableModel> GetTimetable(int hostelId)
        {
            var timetable = new List<FoodTimeTableModel>();
            try
            {
                var weekStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);

                using SqlCommand cmd = _global.Connection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetHostelFoodTimetable";
                cmd.Parameters.AddWithValue("@HostelID", hostelId);
                cmd.Parameters.AddWithValue("@WeekStartDate", weekStartDate);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    timetable.Add(new FoodTimeTableModel
                    {
                        DayOfWeek = reader["DayOfWeek"].ToString(),
                        Breakfast = reader["MealType"].ToString() == "Breakfast"
                            ? reader["FoodItems"].ToString() : "",
                        Lunch = reader["MealType"].ToString() == "Lunch"
                            ? reader["FoodItems"].ToString() : "",
                        Dinner = reader["MealType"].ToString() == "Dinner"
                            ? reader["FoodItems"].ToString() : ""
                    });
                }

                var grouped = timetable
                    .GroupBy(t => t.DayOfWeek)
                    .Select(g => new FoodTimeTableModel
                    {
                        DayOfWeek = g.Key,
                        Breakfast = g.FirstOrDefault(x => !string.IsNullOrEmpty(x.Breakfast))?.Breakfast,
                        Lunch = g.FirstOrDefault(x => !string.IsNullOrEmpty(x.Lunch))?.Lunch,
                        Dinner = g.FirstOrDefault(x => !string.IsNullOrEmpty(x.Dinner))?.Dinner
                    }).ToList();

                return grouped;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UploadTimetable(IFormFile file, int hostelId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0) return false;

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];
                var weekStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
              
                for (int row = 2; row <= 4; row++)
                {
                    var mealType = worksheet.Cells[row, 1].Text.Trim();
                    
                    for (int col = 1; col <= 8; col++)
                    {
                        var day = worksheet.Cells[1, col].Text.Trim();
                        
                        day = day switch
                        {
                            "Thu" => "Thur",
                            "Thurs" => "Thur",
                            "Wednesday" => "Wed",
                            _ => day
                        };
                        
                        if (!new[] { "Mon", "Tue", "Wed", "Thur", "Fri", "Sat", "Sun" }.Contains(day))
                        {
                            Console.WriteLine($"Invalid day: {day}");
                            continue;
                        }

                        var foodItems = worksheet.Cells[row, col].Text.Trim();

                        using SqlCommand cmd = _global.Connection();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_UpsertHostelFoodTimetable";
                        cmd.Parameters.AddWithValue("@HostelID", hostelId);
                        cmd.Parameters.AddWithValue("@MealType", mealType);
                        cmd.Parameters.AddWithValue("@DayOfWeek", day);
                        cmd.Parameters.AddWithValue("@FoodItems", foodItems);
                        cmd.Parameters.AddWithValue("@WeekStartDate", weekStartDate);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return false;
            }
        }
    }
}