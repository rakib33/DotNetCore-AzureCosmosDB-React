using System.Text.RegularExpressions;

namespace RestaurantOpeningApi.Common
{
    public class CommonManagement
    {
       
    // Parses the schedule string and returns a dictionary with day names as keys and time ranges as values
    public static Dictionary<string, string> ParseOperatingDayAndTime(string schedule)
        {
            // Initialize a dictionary to hold the parsed data
            var scheduleDict = new Dictionary<string, string>();

            // Regular expression to match day ranges and time ranges
            string pattern = @"((?:[A-Za-z]+(?:[,\s*-][A-Za-z]+)?(?:,\s*)?)+)\s+(\d{1,2}\s+[ap]m\s+-\s+\d{1,2}:\d{2}\s+[ap]m)";
      
            // Find matches in the schedule string
            var matches = Regex.Matches(schedule, pattern);

            foreach (Match match in matches)
            {
                // Extract the day and time range from the match
                string days = match.Groups[1].Value.Trim();
                string timeRange = match.Groups[2].Value.Trim();

                // Split the days part by commas to handle multiple day ranges
                var dayRanges = days.Split(',');

                foreach (var dayRange in dayRanges)
                {
                    // Trim the day range and add it to the dictionary
                    string trimmedDayRange = dayRange.Trim();
                    scheduleDict[trimmedDayRange] = timeRange;
                }
            }

            return scheduleDict;
        }
    }
}
