using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantOpeningApi.Test.ServiceTest
{
    public class CommonManagementTest
    {

        public CommonManagementTest() { 
        
        }   
        [Fact]
        public void TestCommonManagement()
        {
            //Arrange
            string operatingTime = "Sun-Mon 11:30 am - 9 pm";
            var expectedRestaurants = new List<RestaurantTimeDTO>
            {
                new RestaurantTimeDTO {
                 OpeningDay = "Sunday",
                 OpeningTime  = CommonManagement.ParseTime("11:30am"),
                 ClosingTime = CommonManagement.ParseTime("9pm")
                },
                new RestaurantTimeDTO {
                 OpeningDay = "Monday",
                 OpeningTime  = CommonManagement.ParseTime("11:30am"),
                 ClosingTime = CommonManagement.ParseTime("9pm")
                }
            };
            //Act
            var result = CommonManagement.ParseRestaurantTimeString(operatingTime);
            //Assert 
            Assert.NotNull(result);
            Assert.IsType<List<RestaurantTimeDTO>>(result);
            Assert.Equal(expectedRestaurants.Count, result.Count);
        }

        [Fact]
        public void TestParseTime_ReturnArgumentException()
        {
            //Arrange
            string time = " ";          
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => CommonManagement.ParseTime(time));

            Assert.IsType<ArgumentException>(ex);
            Assert.Equal("Invalid time format", ex.Message);
        }


        [Fact]
        public void TestParseTime_ReturnTimeSuccess()
        {
            //Arrange
            string time = "11 am";
            // Act & Assert
            var result =  CommonManagement.ParseTime(time);

            Assert.IsType<TimeSpan>(result);
        
        }

        [Fact]
        public void TestGetDayRange_ReturnDaysWithinRange()
        {
            List<string> days = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday","Friday", "Saturday" };
            var list = CommonManagement.GetDayRange("Sun", "Sat");
            Assert.NotNull(list);
            Assert.Equal(days, list);
        }
    }
}
