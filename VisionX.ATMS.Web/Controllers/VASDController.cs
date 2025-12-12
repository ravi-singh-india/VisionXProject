using Microsoft.AspNetCore.Mvc;
using VisionX.ATMS.Web.Models;

namespace VisionX.ATMS.Web.Controllers
{
    public class VASDController : Controller
    {
        public IActionResult Index()
        {
            var model = new VASDViewModel
            {
                TotalDevices = 45,
                ActiveDevices = 38,
                SpeedViolations = 316,
                VehiclesDetectedToday = 2847,
                AverageSpeed = 65,
                VASDDevices = GetVASDDevices(),
                RecentDetections = GetRecentDetections()
            };

            return View(model);
        }

        // API Endpoints
        [HttpGet]
        public JsonResult GetDeviceData(string deviceId)
        {
            var device = GetVASDDevices().FirstOrDefault(d => d.DeviceId == deviceId);
            if (device == null)
            {
                return Json(new { success = false, message = "Device not found" });
            }

            return Json(new
            {
                success = true,
                device = new
                {
                    device.DeviceId,
                    device.DeviceName,
                    device.Location,
                    device.CurrentSpeed,
                    device.SpeedLimit,
                    device.IsActive,
                    device.LastUpdate,
                    Statistics = new
                    {
                        TotalDetections = 2456,
                        TodayDetections = 324,
                        Violations = 234,
                        AverageSpeed = 67
                    }
                }
            });
        }

        [HttpGet]
        public JsonResult GetRealtimeSpeed(string deviceId)
        {
            // Simulate real-time speed data
            var random = new Random();
            return Json(new
            {
                deviceId,
                speed = random.Next(30, 120),
                timestamp = DateTime.Now
            });
        }

        [HttpPost]
        public JsonResult SaveSettings([FromBody] VASDSettingsModel settings)
        {
            try
            {
                // Save settings to database
                // Implementation here

                return Json(new
                {
                    success = true,
                    message = "Settings saved successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult CalibrateDevice(string deviceId)
        {
            try
            {
                // Implement calibration logic
                // Send command to device, wait for response

                return Json(new
                {
                    success = true,
                    message = "Device calibration started"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult ResetDevice(string deviceId)
        {
            try
            {
                // Implement reset logic
                // Send reset command to device

                return Json(new
                {
                    success = true,
                    message = "Device reset successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public JsonResult GetDetections(int page = 1, int pageSize = 20, string deviceId = null, string filter = null)
        {
            var detections = GetRecentDetections();

            // Apply filters
            if (!string.IsNullOrEmpty(deviceId))
            {
                detections = detections.Where(d => d.DeviceId == deviceId).ToList();
            }

            if (filter == "violation")
            {
                detections = detections.Where(d => d.IsViolation).ToList();
            }
            else if (filter == "safe")
            {
                detections = detections.Where(d => !d.IsViolation).ToList();
            }

            // Pagination
            var totalRecords = detections.Count;
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var pagedDetections = detections
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Json(new
            {
                success = true,
                data = pagedDetections,
                page,
                pageSize,
                totalPages,
                totalRecords
            });
        }

        [HttpGet]
        public JsonResult GetAnalytics(string timeframe = "today")
        {
            // Get analytics data based on timeframe
            return Json(new
            {
                success = true,
                speedDistribution = new
                {
                    labels = new[] { "0-20", "20-40", "40-60", "60-80", "80-100", "100+" },
                    data = new[] { 45, 320, 850, 1240, 380, 95 }
                },
                hourlyTraffic = new
                {
                    labels = new[] { "00:00", "03:00", "06:00", "09:00", "12:00", "15:00", "18:00", "21:00" },
                    data = new[] { 45, 32, 156, 485, 624, 548, 698, 324 }
                },
                violations = new
                {
                    minor = 156,
                    moderate = 98,
                    severe = 62
                }
            });
        }

        [HttpGet]
        public IActionResult ExportReport(string format = "pdf", DateTime? startDate = null, DateTime? endDate = null)
        {
            // Implement report generation
            // Return file based on format (PDF, Excel, etc.)

            var fileName = $"VASD_Report_{DateTime.Now:yyyyMMdd}.{format}";

            // Generate report content
            byte[] fileContent = GenerateReport(format, startDate, endDate);

            string contentType = format == "pdf" ? "application/pdf" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileContent, contentType, fileName);
        }

        // Private helper methods
        private List<VASDDeviceModel> GetVASDDevices()
        {
            // In real application, fetch from database
            var random = new Random();
            return new List<VASDDeviceModel>
            {
                new VASDDeviceModel
                {
                    DeviceId = "VASD-001",
                    DeviceName = "Highway Entry Point",
                    Location = "KM 0 - NH48 Entry",
                    CurrentSpeed = random.Next(40, 80),
                    SpeedLimit = 80,
                    IsActive = true,
                    LastUpdate = DateTime.Now.AddMinutes(-2)
                },
                new VASDDeviceModel
                {
                    DeviceId = "VASD-002",
                    DeviceName = "City Bypass",
                    Location = "KM 15 - City Bypass",
                    CurrentSpeed = random.Next(50, 90),
                    SpeedLimit = 80,
                    IsActive = true,
                    LastUpdate = DateTime.Now.AddMinutes(-1)
                },
                new VASDDeviceModel
                {
                    DeviceId = "VASD-003",
                    DeviceName = "School Zone Display",
                    Location = "KM 25 - School Area",
                    CurrentSpeed = random.Next(20, 50),
                    SpeedLimit = 40,
                    IsActive = true,
                    LastUpdate = DateTime.Now.AddMinutes(-3)
                },
                new VASDDeviceModel
                {
                    DeviceId = "VASD-004",
                    DeviceName = "Hospital Zone",
                    Location = "KM 32 - Medical District",
                    CurrentSpeed = random.Next(25, 55),
                    SpeedLimit = 50,
                    IsActive = true,
                    LastUpdate = DateTime.Now.AddMinutes(-5)
                },
                new VASDDeviceModel
                {
                    DeviceId = "VASD-005",
                    DeviceName = "Industrial Area",
                    Location = "KM 45 - Industrial Zone",
                    CurrentSpeed = 0,
                    SpeedLimit = 60,
                    IsActive = false,
                    LastUpdate = DateTime.Now.AddHours(-2)
                },
                new VASDDeviceModel
                {
                    DeviceId = "VASD-006",
                    DeviceName = "Toll Plaza Approach",
                    Location = "KM 58 - Toll Plaza",
                    CurrentSpeed = random.Next(30, 70),
                    SpeedLimit = 60,
                    IsActive = true,
                    LastUpdate = DateTime.Now.AddSeconds(-30)
                }
            };
        }

        private List<VehicleDetectionModel> GetRecentDetections()
        {
            // In real application, fetch from database
            var random = new Random();
            var detections = new List<VehicleDetectionModel>();
            var devices = GetVASDDevices();

            for (int i = 0; i < 50; i++)
            {
                var device = devices[random.Next(devices.Count)];
                var speed = random.Next(30, 120);

                detections.Add(new VehicleDetectionModel
                {
                    Id = i + 1,
                    DeviceId = device.DeviceId,
                    DeviceName = device.DeviceName,
                    Location = device.Location,
                    Speed = speed,
                    SpeedLimit = device.SpeedLimit,
                    IsViolation = speed > device.SpeedLimit,
                    Timestamp = DateTime.Now.AddMinutes(-i * 5),
                    VehicleType = GetRandomVehicleType()
                });
            }

            return detections.OrderByDescending(d => d.Timestamp).ToList();
        }

        private string GetRandomVehicleType()
        {
            var types = new[] { "Car", "Truck", "Bus", "Motorcycle", "Van" };
            return types[new Random().Next(types.Length)];
        }

        private byte[] GenerateReport(string format, DateTime? startDate, DateTime? endDate)
        {
            // Implement actual report generation logic
            // This is a placeholder
            return new byte[] { 0x25, 0x50, 0x44, 0x46 }; // PDF header
        }
    }
}
