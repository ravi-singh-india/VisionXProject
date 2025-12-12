using System.ComponentModel.DataAnnotations;

namespace VisionX.ATMS.Web.Models
{
    public class VASD
    {
    }

    // Main VASD View Model
    public class VASDViewModel
    {
        public int TotalDevices { get; set; }
        public int ActiveDevices { get; set; }
        public int SpeedViolations { get; set; }
        public int VehiclesDetectedToday { get; set; }
        public int AverageSpeed { get; set; }
        public List<VASDDeviceModel> VASDDevices { get; set; }
        public List<VehicleDetectionModel> RecentDetections { get; set; }
    }

    // VASD Device Model
    public class VASDDeviceModel
    {
        [Required]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(100)]
        public string DeviceName { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        public int CurrentSpeed { get; set; }

        [Range(20, 120)]
        public int SpeedLimit { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastUpdate { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string IPAddress { get; set; }

        public int Brightness { get; set; } = 80;

        public string DisplayMode { get; set; } = "continuous";

        public int WarningThreshold { get; set; } = 90;

        // Statistics
        public int TotalDetectionsToday { get; set; }
        public int ViolationsToday { get; set; }
        public string FirmwareVersion { get; set; }
        public DateTime? LastCalibration { get; set; }
    }

    // Vehicle Detection Model
    public class VehicleDetectionModel
    {
        public int Id { get; set; }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string Location { get; set; }

        public int Speed { get; set; }

        public int SpeedLimit { get; set; }

        public bool IsViolation { get; set; }

        public DateTime Timestamp { get; set; }

        public string VehicleType { get; set; }

        public string LicensePlate { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public double? VehicleLength { get; set; }

        public string Direction { get; set; } // Inbound/Outbound

        public int? LaneNumber { get; set; }
    }

    // VASD Settings Model
    public class VASDSettingsModel
    {
        [Required]
        public string DeviceId { get; set; }

        [Range(20, 120)]
        public int SpeedLimit { get; set; }

        [Range(50, 100)]
        public int WarningThreshold { get; set; }

        [Required]
        public string DisplayMode { get; set; } // continuous, trigger, scheduled

        [Range(0, 100)]
        public int Brightness { get; set; }

        public bool EnableNightMode { get; set; }

        public TimeSpan? NightModeStartTime { get; set; }

        public TimeSpan? NightModeEndTime { get; set; }

        public bool EnableDataLogging { get; set; }

        public int DataRetentionDays { get; set; } = 30;

        public bool EnableAlerts { get; set; }

        public string AlertEmail { get; set; }

        public string AlertPhoneNumber { get; set; }
    }

    // VASD Analytics Model
    public class VASDAnalyticsModel
    {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string DeviceId { get; set; }

            // Speed Distribution
            public Dictionary<string, int> SpeedDistribution { get; set; }

            // Hourly Traffic Pattern
            public Dictionary<int, int> HourlyTraffic { get; set; }

            // Violation Statistics
            public int TotalViolations { get; set; }
            public int MinorViolations { get; set; }
            public int ModerateViolations { get; set; }
            public int SevereViolations { get; set; }

            // Vehicle Type Distribution
            public Dictionary<string, int> VehicleTypeDistribution { get; set; }

            // Peak Hours
            public List<PeakHourModel> PeakHours { get; set; }

            // Average Speeds
            public double AverageSpeed { get; set; }
            public double MedianSpeed { get; set; }

            public int ModeSpeed { get; set; }
    }

    // Peak Hour Model
    public class PeakHourModel
    {
        public int Hour { get; set; }
        public int VehicleCount { get; set; }
        public double AverageSpeed { get; set; }
        public int ViolationCount { get; set; }
    }

    // VASD Alert Model
    public class VASDAlertModel
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string AlertType { get; set; } // SpeedViolation, DeviceOffline, CalibrationRequired
        public string Severity { get; set; } // Low, Medium, High, Critical
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public string AcknowledgedBy { get; set; }
    }

    // VASD Calibration Model
    public class VASDCalibrationModel
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime CalibrationDate { get; set; }
        public string CalibratedBy { get; set; }
        public string CalibrationMethod { get; set; }
        public double AccuracyBefore { get; set; }
        public double AccuracyAfter { get; set; }
        public string Notes { get; set; }
        public bool IsSuccessful { get; set; }
        public Dictionary<string, string> CalibrationParameters { get; set; }
    }

    // VASD Maintenance Model
    public class VASDMaintenanceModel
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } // Routine, Repair, Upgrade
        public string PerformedBy { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public List<string> ReplacedParts { get; set; }
    }

    // VASD Report Model
    public class VASDReportModel
    {
        public DateTime GeneratedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReportType { get; set; } // Daily, Weekly, Monthly, Custom

        // Summary Statistics
        public int TotalVehicles { get; set; }
        public int TotalViolations { get; set; }
        public double AverageSpeed { get; set; }
        public int PeakHourTraffic { get; set; }

        // Device Performance
        public List<DevicePerformanceModel> DevicePerformance { get; set; }

        // Top Violation Locations
        public List<ViolationLocationModel> TopViolationLocations { get; set; }
    }

    // Device Performance Model
    public class DevicePerformanceModel
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public double UptimePercentage { get; set; }
        public int TotalDetections { get; set; }
        public int ErrorCount { get; set; }
        public double AccuracyScore { get; set; }
    }

    // Violation Location Model
    public class ViolationLocationModel
    {
        public string Location { get; set; }
        public int ViolationCount { get; set; }
        public double AverageExcessSpeed { get; set; }
        public string MostCommonVehicleType { get; set; }
    }

}
