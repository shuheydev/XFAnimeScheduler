using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Animescheduler
{
    public class AnimeInfo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string OfficialUrl { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public IEnumerable<Schedule> Schedules { get; set; } = new List<Schedule>();
    }

    public class Schedule
    {
        public string Station { get; set; } = string.Empty;
        public string DateTimeFrom { get; set; } = string.Empty;
        public string DateFrom { get; set; } = string.Empty;
        public string TimeFrom { get; set; } = string.Empty;

        public System.DateTimeOffset GetDateTimeOffset()
        {
            if (DateTimeOffset.TryParse($"{DateFrom}{TimeFrom}", out DateTimeOffset result))
            {
                return result;
            }

            try
            {
                //アニメは25:00などの表記をするため,パースに失敗する
                if (!int.TryParse(Regex.Match(TimeFrom, @"^\d+").Value, out int hours))
                    return DateTimeOffset.MaxValue;
                string hoursStr = (hours - 24).ToString().PadLeft(2, '0');

                string minutesStr = Regex.Match(TimeFrom, @"(?<=:)\d+$").Value;

                if (!DateTimeOffset.TryParse($"{DateFrom}{hoursStr}:{minutesStr}", out DateTimeOffset dateTime))
                    return DateTimeOffset.MaxValue;

                return dateTime.AddDays(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}