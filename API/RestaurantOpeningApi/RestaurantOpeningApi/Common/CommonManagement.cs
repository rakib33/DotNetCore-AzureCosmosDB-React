using Microsoft.Azure.Cosmos.Spatial;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Models;
using System.Text.RegularExpressions;

namespace RestaurantOpeningApi.Common
{
    public class CommonManagement
    {
       
        // Function to convert three-character day names to DayOfWeek enum
        static DayOfWeek GetDayOfWeek(string day)
        {
            switch (day.ToLower())
            {
                case "sun": return DayOfWeek.Sunday;
                case "mon": return DayOfWeek.Monday;
                case "tue": return DayOfWeek.Tuesday;
                case "tues": return DayOfWeek.Thursday;
                case "wed": return DayOfWeek.Wednesday;
                case "weds": return DayOfWeek.Wednesday;
                case "thu": return DayOfWeek.Thursday;
                case "thurs": return DayOfWeek.Thursday;                
                case "fri": return DayOfWeek.Friday;
                case "sat": return DayOfWeek.Saturday;
                default: throw new ArgumentException("Invalid day name");
            }
        }
        static List<string> GetDayRange(string startDay, string endDay)
        {

                // Get the DayOfWeek enum representation of start and end days
                DayOfWeek startDayOfWeek = GetDayOfWeek(startDay);
                DayOfWeek endDayOfWeek = GetDayOfWeek(endDay);

                List<string> days = new List<string>();

                // Loop through the days and print the names
                for (int i = (int)startDayOfWeek; i % 7 != (int)endDayOfWeek; i++)
                {
                    days.Add(((DayOfWeek)(i % 7)).ToString());
                }
                days.Add(endDayOfWeek.ToString());

                return days;
         
        }
        public  static TimeSpan ParseTime(string timeString)
        {
            // Define custom time formats
            string[] formats = { "h tt", "h:mmtt", "htt", "hhtt" }; // For example: "11 am", "11:00am", "11am", "11am"

            // Parse the time string using custom formats
            if (DateTime.TryParseExact(timeString, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
            {
                return parsedTime.TimeOfDay;
            }
            else
            {
                throw new ArgumentException("Invalid time format");
            }
        }
        public   static List<RestaurantTimeDTO> ParseRestaurantTimeString(string operatingTimeString)
        {

            string[] splitbySlash = operatingTimeString.Split('/');
            List<string> operatingTime = new List<string>(splitbySlash);


            List<RestaurantTimeDTO> restaurantTime = new List<RestaurantTimeDTO>();

            foreach (var item in operatingTime)
            {

                string itm = Regex.Replace(item, @"\s+", "");

                char[] numberArr = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                int index = itm.IndexOfAny(numberArr);
                string dayPart = itm.Substring(0, index);
                string timePart = itm[index..itm.Length];

                var days = dayPart.Split(',');

                //Time part 
                var splitTime = timePart.Split("-");
                TimeSpan startTime = ParseTime(splitTime[0]);
                TimeSpan endTime = ParseTime(splitTime[1]);

                //if(endTime < startTime)
                //{
                //    TimeSpan time = startTime;
                //    startTime = endTime;
                //    endTime = time;
                //}

                try
                {
                    foreach (var day in days)
                        if (day.Contains("-"))
                        {
                            var dayRange = day.Split('-');

                            List<string> getDaysRange = GetDayRange(dayRange[0], dayRange[1]);

                            foreach (var dayname in getDaysRange)
                            {
                                restaurantTime.Add(new RestaurantTimeDTO
                                {
                                    OpeningDay = dayname,
                                    OpeningTime = startTime,
                                    ClosingTime = endTime,
                                });
                            }
                        }
                        else
                            restaurantTime.Add(new RestaurantTimeDTO
                            {
                                OpeningDay = day,
                                OpeningTime = startTime,
                                ClosingTime = endTime,
                            });
                }catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }

            return restaurantTime;

        }

        public static TimeSpan SetClosingTime(TimeSpan openTime, TimeSpan closingTime)
        {
            if (closingTime < openTime)
            {
                // Add 24 hours to the end time to make it on the same day
                closingTime = closingTime.Add(new TimeSpan(24, 0, 0));
            }

            return closingTime;
        }
        public static TimeSpan GetTotalTimeDuration(TimeSpan openTime, TimeSpan closingTime)
        {
            TimeSpan duration;
            //// If the end time is before the start time, it means it's on the next day
            if (closingTime < openTime)
            {
                // Add 24 hours to the end time to make it on the same day
                closingTime = closingTime.Add(new TimeSpan(24, 0, 0));
                // Calculate the duration between the times
                duration = closingTime - openTime;
            }
            else
            {
                duration = closingTime - openTime;
            }

            return duration;
            
        }

        public static bool IsTimeOnDuration(TimeSpan time, TimeSpan openTime, TimeSpan closingTime)
        {
            TimeSpan duration;
            //// If the end time is before the start time, it means it's on the next day
            if (closingTime < openTime)           
                // Add 24 hours to the end time to make it on the same day
                closingTime = closingTime.Add(new TimeSpan(24, 0, 0));             
            if(time < openTime)
                time = time.Add(new TimeSpan(24, 0, 0));

            if (time > openTime && time < closingTime)
                return true;
            else return false;
        }

    }
    }   


