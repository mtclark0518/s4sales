using System;
using System.ComponentModel.DataAnnotations;
namespace S4Sales.Models
{
    /// <summary>
    // model definitions for Crash, Participant && Vehicle
    ///</summary>
    public class CrashEvent
    {
        [Key]
        public int hsmv_report_number { get; set; }
        public DateTime crash_date_and_time { get; set; }
        public DateTime hsmv_entry_date { get; set; }
        public string county_of_crash { get; set; }
        public string city_of_crash { get; set; }
        public string crash_on_street { get; set; }
        public string crash_intersecting_street { get; set; }
        public string reporting_agency { get; set; }
        public string path_to_image { get; set; }
    }
    public class CrashParticipant
    {
        [Key]
        public int participant_key {get; set;}
        public int hsmv_report_number { get; set; }
        public int participant_number {get; set;}
        public string participant_type {get; set;}
        public string participant_first_name {get; set;}
        public string participant_last_name {get; set;}
        public int participant_age {get; set;}
        public string participant_gender {get; set;}
    }
    public class Vehicle
    {
        [Key]
        public int vehicle_key {get;set;}
        public int hsmv_report_number {get;set;}
        public string vehicle_identification_number {get; set;}
        public int vehicle_number {get;set;}
        public int vehicle_year {get;set;}
        public string vehicle_make {get;set;}
        public string vehicle_model {get;set;}
        public string vehicle_color {get;set;}
        
    }
}

