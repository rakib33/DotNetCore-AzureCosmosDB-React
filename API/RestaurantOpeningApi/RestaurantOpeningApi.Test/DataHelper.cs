using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOpeningApi.Test
{
    public class DataHelper
    {
        public static List<Restaurant> GetFakeRestaurantData()
        {
            return new List<Restaurant>()
            {
                new Restaurant {Id ="1", Name = "Kushi Tsuru", OperatingTime ="Mon-Sun 11:30 am - 9 pm",
                restaurantTimes= new List<RestaurantTime>(){ 
                 new RestaurantTime {Id ="1", OpeningDay="Monday", OpeningTime= new TimeSpan(11,30,0), ClosingTime= new TimeSpan(21,0,0),RestaurantId="1",TotalTimeDuration = new TimeSpan(9,30,0)},
                 new RestaurantTime {Id ="2", OpeningDay="Tuesday", OpeningTime= new TimeSpan(11,30,0), ClosingTime= new TimeSpan(21,0,0),RestaurantId="1",TotalTimeDuration = new TimeSpan(9,30,0)},
                 new RestaurantTime {Id ="3", OpeningDay="Wednesday", OpeningTime= new TimeSpan(11,30,0), ClosingTime= new TimeSpan(21,0,0),RestaurantId="1",TotalTimeDuration = new TimeSpan(9,30,0)},
                 new RestaurantTime {Id ="4", OpeningDay="Thursday", OpeningTime= new TimeSpan(11,30,0), ClosingTime= new TimeSpan(21,0,0),RestaurantId="1", TotalTimeDuration = new TimeSpan(9, 30, 0) },
                 new RestaurantTime {Id ="5", OpeningDay="Friday", OpeningTime= new TimeSpan(11,30,0), ClosingTime= new TimeSpan(21,0,0),RestaurantId="1", TotalTimeDuration = new TimeSpan(9, 30, 0)},
                 new RestaurantTime {Id ="6", OpeningDay="Saturday", OpeningTime= new TimeSpan(11,30,0), ClosingTime= new TimeSpan(21,0,0),RestaurantId="1", TotalTimeDuration = new TimeSpan(9, 30, 0)},
                 new RestaurantTime {Id ="7", OpeningDay="Sunday", OpeningTime= new TimeSpan(11,30,0), ClosingTime= new TimeSpan(21,0,0),RestaurantId="1", TotalTimeDuration = new TimeSpan(9, 30, 0)},
                
                } },
                new Restaurant {Id ="2", Name = "The Cheesecake Factory", OperatingTime ="Mon-Thu 11 am - 11 pm  / Fri-Sat 11 am - 12:30 am  / Sun 10 am - 11 pm",
                    restaurantTimes = new List<RestaurantTime>(){
                     new RestaurantTime {Id ="8", OpeningDay="Monday",TotalTimeDuration = new TimeSpan(12,0,0), OpeningTime= new TimeSpan(11,0,0), ClosingTime= new TimeSpan(23,0,0),RestaurantId="2"},
                     new RestaurantTime {Id ="9", OpeningDay="Tuesday",TotalTimeDuration = new TimeSpan(12,0,0), OpeningTime= new TimeSpan(11,0,0), ClosingTime= new TimeSpan(23,0,0),RestaurantId="2"},
                     new RestaurantTime {Id ="10", OpeningDay="Wednesday", TotalTimeDuration = new TimeSpan(12,0,0), OpeningTime= new TimeSpan(11,0,0), ClosingTime= new TimeSpan(23,0,0),RestaurantId="2"},
                     new RestaurantTime {Id ="11", OpeningDay="Thursday",TotalTimeDuration = new TimeSpan(12,0,0), OpeningTime= new TimeSpan(11,0,0), ClosingTime= new TimeSpan(23,0,0),RestaurantId="2"},
                     new RestaurantTime {Id ="12", OpeningDay="Friday",TotalTimeDuration = new TimeSpan(12,0,0), OpeningTime= new TimeSpan(11,0,0), ClosingTime= new TimeSpan(1,30,0),RestaurantId="2"},
                     new RestaurantTime {Id ="13", OpeningDay="Saturday",TotalTimeDuration = new TimeSpan(12,0,0), OpeningTime= new TimeSpan(11,0,0), ClosingTime= new TimeSpan(1,30,0),RestaurantId="2"},
                     new RestaurantTime {Id ="14", OpeningDay="Sunday",TotalTimeDuration = new TimeSpan(13,0,0), OpeningTime= new TimeSpan(10,0,0), ClosingTime= new TimeSpan(23,0,0),RestaurantId="2"},
                    } },
                new Restaurant {Id ="3", Name = "Sudachi", OperatingTime ="Mon-Wed 5 pm - 12:30 am  / Thu-Fri 5 pm - 1:30 am  / Sat 3 pm - 1:30 am  / Sun 3 pm - 11:30 pm",
                  restaurantTimes = new List<RestaurantTime>
                  {
                       new RestaurantTime {Id ="15", OpeningDay="Monday",OpeningTime= new TimeSpan(17,0,0), ClosingTime= new TimeSpan(1,30,0),RestaurantId="3",TotalTimeDuration = new TimeSpan(7,30,0), },
                       new RestaurantTime {Id ="16", OpeningDay="Tuesday",OpeningTime= new TimeSpan(17,0,0), ClosingTime= new TimeSpan(1,30,0),RestaurantId="3",TotalTimeDuration = new TimeSpan(7,30,0), },
                       new RestaurantTime {Id ="17", OpeningDay="Wednesday",OpeningTime= new TimeSpan(17,0,0), ClosingTime= new TimeSpan(1,30,0),RestaurantId="3",TotalTimeDuration = new TimeSpan(7,30,0), },
                       new RestaurantTime {Id ="18", OpeningDay="Thursday",OpeningTime= new TimeSpan(17,0,0), ClosingTime= new TimeSpan(1,30,0),RestaurantId="3",TotalTimeDuration = new TimeSpan(8,30,0), },
                       new RestaurantTime {Id ="19", OpeningDay="Friday",OpeningTime= new TimeSpan(17,0,0), ClosingTime= new TimeSpan(1,30,0),RestaurantId="3",TotalTimeDuration = new TimeSpan(8,30,0), },
                       new RestaurantTime {Id ="20", OpeningDay="Saturday",OpeningTime= new TimeSpan(15,0,0), ClosingTime= new TimeSpan(1,30,0),RestaurantId="3",TotalTimeDuration = new TimeSpan(10,30,0), },
                       new RestaurantTime {Id ="21", OpeningDay="Sunday",OpeningTime= new TimeSpan(15,0,0), ClosingTime= new TimeSpan(23,30,0),RestaurantId="3",TotalTimeDuration = new TimeSpan(8,30,0), },
                  }
                },
            };

        }

        public static List<Restaurant> GetFakeRestaurantEmptyList()
        {
            return new List<Restaurant>();

        }

        public static RestaurantParameters parameters(string name,string day,string time,Pagination page)   
        {
            RestaurantParameters parms = new RestaurantParameters();
            Pagination pagination = new Pagination();
            pagination.Page = page.Page;
            pagination.PageSize = page.PageSize;
            parms.name = name;
            parms.day = day;
            parms.time = CommonManagement.GetTimeSpanFromString(time);
            parms.Pagination = pagination;

            return parms;
        }


    }
}
