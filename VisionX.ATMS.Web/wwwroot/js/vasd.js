// VASD Page JavaScript

(function () {
    'use strict';

    let currentSpeed = 45;
    let speedLimit = 80;
    let selectedDeviceId = null;
    let charts = {};

    // Initialize on DOM load
    document.addEventListener('DOMContentLoaded', function () {
        initializeEventListeners();
        initializeCharts();
        startSpeedSimulation();
        loadDetections();
    });

    // Event Listeners
    function initializeEventListeners() {
        // Refresh button
        document.getElementById('refreshData')?.addEventListener('click', refreshData);

        // Export report
        document.getElementById('exportReport')?.addEventListener('click', exportReport);

        // Device selection
        document.getElementById('selectedDevice')?.addEventListener('change', handleDeviceSelection);

        // Device list items
        document.querySelectorAll('.device-item').forEach(item => {
            item.addEventListener('click', function () {
                selectDevice(this.dataset.deviceId);
            });
        });

        // Display controls
        document.getElementById('testDisplay')?.addEventListener('click', testDisplay);
        document.getElementById('calibrateDevice')?.addEventListener('click', calibrateDevice);
        document.getElementById('resetDevice')?.addEventListener('click', resetDevice);

        // Settings
        document.getElementById('saveSettings')?.addEventListener('click', saveSettings);
        document.getElementById('speedLimitInput')?.addEventListener('change', updateSpeedLimit);
        document.getElementById('brightness')?.addEventListener('input', updateBrightness);

        // Device search
        document.getElementById('deviceSearch')?.addEventListener('input', searchDevices);

        // Table filters
        document.getElementById('filterDevice')?.addEventListener('change', filterDetections);
        document.getElementById('filterViolation')?.addEventListener('change', filterDetections);

        // Pagination
        document.getElementById('prevPage')?.addEventListener('click', () => changePage(-1));
        document.getElementById('nextPage')?.addEventListener('click', () => changePage(1));

        // View device details
        document.querySelectorAll('.view-device').forEach(btn => {
            btn.addEventListener('click', function (e) {
                e.stopPropagation();
                viewDeviceDetails(this.dataset.deviceId);
            });
        });

        // Edit device
        document.querySelectorAll('.edit-device').forEach(btn => {
            btn.addEventListener('click', function (e) {
                e.stopPropagation();
                editDevice(this.dataset.deviceId);
            });
        });

        // View detection details
        document.querySelectorAll('.view-details').forEach(btn => {
            btn.addEventListener('click', function () {
                viewDetectionDetails(this.dataset.detectionId);
            });
        });
    }

    // Speed Simulation
    function startSpeedSimulation() {
        setInterval(() => {
            // Simulate random speed changes
            const change = (Math.random() - 0.5) * 10;
            currentSpeed = Math.max(20, Math.min(120, currentSpeed + change));
            updateSpeedDisplay();
        }, 2000);
    }

    function updateSpeedDisplay() {
        const speedElement = document.getElementById('displaySpeed');
        const statusElement = document.getElementById('displayStatus');

        if (speedElement) {
            speedElement.textContent = Math.round(currentSpeed);

            // Update color based on speed
            speedElement.className = 'speed-value';
            if (currentSpeed > speedLimit) {
                speedElement.classList.add('danger');
            } else if (currentSpeed > speedLimit * 0.9) {
                speedElement.classList.add('warning');
            }
        }

        if (statusElement) {
            if (currentSpeed > speedLimit) {
                statusElement.className = 'display-status danger';
                statusElement.innerHTML = '<i class="fas fa-exclamation-triangle"></i><span>SPEED VIOLATION</span>';
            } else if (currentSpeed > speedLimit * 0.9) {
                statusElement.className = 'display-status warning';
                statusElement.innerHTML = '<i class="fas fa-exclamation-circle"></i><span>SLOW DOWN</span>';
            } else {
                statusElement.className = 'display-status';
                statusElement.innerHTML = '<i class="fas fa-check-circle"></i><span>SAFE SPEED</span>';
            }
        }
    }

    // Initialize Charts
    function initializeCharts() {
        // Speed Distribution Chart
        const speedCtx = document.getElementById('speedDistributionChart');
        if (speedCtx) {
            charts.speedDistribution = new Chart(speedCtx, {
                type: 'bar',
                data: {
                    labels: ['0-20', '20-40', '40-60', '60-80', '80-100', '100+'],
                    datasets: [{
                        label: 'Vehicles',
                        data: [45, 320, 850, 1240, 380, 95],
                        backgroundColor: [
                            'rgba(0, 255, 136, 0.6)',
                            'rgba(0, 255, 136, 0.6)',
                            'rgba(0, 212, 255, 0.6)',
                            'rgba(255, 170, 0, 0.6)',
                            'rgba(255, 68, 68, 0.6)',
                            'rgba(204, 0, 0, 0.6)'
                        ],
                        borderColor: [
                            '#00ff88',
                            '#00ff88',
                            '#00d4ff',
                            '#ffaa00',
                            '#ff4444',
                            '#cc0000'
                        ],
                        borderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            grid: {
                                color: 'rgba(255, 255, 255, 0.1)'
                            },
                            ticks: {
                                color: '#8b92a7'
                            }
                        },
                        x: {
                            grid: {
                                color: 'rgba(255, 255, 255, 0.1)'
                            },
                            ticks: {
                                color: '#8b92a7'
                            }
                        }
                    }
                }
            });
        }

        // Hourly Traffic Chart
        const trafficCtx = document.getElementById('hourlyTrafficChart');
        if (trafficCtx) {
            charts.hourlyTraffic = new Chart(trafficCtx, {
                type: 'line',
                data: {
                    labels: ['00:00', '03:00', '06:00', '09:00', '12:00', '15:00', '18:00', '21:00'],
                    datasets: [{
                        label: 'Vehicle Count',
                        data: [45, 32, 156, 485, 624, 548, 698, 324],
                        borderColor: '#00d4ff',
                        backgroundColor: 'rgba(0, 212, 255, 0.1)',
                        borderWidth: 2,
                        fill: true,
                        tension: 0.4
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            grid: {
                                color: 'rgba(255, 255, 255, 0.1)'
                            },
                            ticks: {
                                color: '#8b92a7'
                            }
                        },
                        x: {
                            grid: {
                                color: 'rgba(255, 255, 255, 0.1)'
                            },
                            ticks: {
                                color: '#8b92a7'
                            }
                        }
                    }
                }
            });
        }
    }

    // Device Selection
    function selectDevice(deviceId) {
        selectedDeviceId = deviceId;

        // Update UI
        document.querySelectorAll('.device-item').forEach(item => {
            item.style.borderColor = '';
        });

        const selectedItem = document.querySelector(`.device-item[data-device-id="${deviceId}"]`);
        if (selectedItem) {
            selectedItem.style.borderColor = '#00d4ff';
        }

        // Update dropdown
        const dropdown = document.getElementById('selectedDevice');
        if (dropdown) {
            dropdown.value = deviceId;
        }

        // Load device data
        loadDeviceData(deviceId);
    }

    function handleDeviceSelection(e) {
        const deviceId = e.target.value;
        if (deviceId) {
            selectDevice(deviceId);
        }
    }

    // Load Device Data
    function loadDeviceData(deviceId) {
        // Simulate loading device-specific data
        console.log('Loading data for device:', deviceId);

        // You can make an AJAX call here
        // fetch(`/VASD/GetDeviceData?deviceId=${deviceId}`)
        //     .then(response => response.json())
        //     .then(data => updateDeviceDisplay(data));
    }

    // Display Controls
    function testDisplay() {
        alert('Testing display with sample patterns...');
        // Cycle through different speeds
        const testSpeeds = [30, 60, 85, 110];
        let index = 0;

        const testInterval = setInterval(() => {
            currentSpeed = testSpeeds[index];
            updateSpeedDisplay();
            index++;

            if (index >= testSpeeds.length) {
                clearInterval(testInterval);
            }
        }, 1500);
    }

    function calibrateDevice() {
        if (!selectedDeviceId) {
            alert('Please select a device first');
            return;
        }

        if (confirm('Start calibration process for selected device?')) {
            // Implement calibration logic
            console.log('Calibrating device:', selectedDeviceId);
            alert('Calibration started. This may take a few minutes...');
        }
    }

    function resetDevice() {
        if (!selectedDeviceId) {
            alert('Please select a device first');
            return;
        }

        if (confirm('Reset device to factory settings? This will clear all custom configurations.')) {
            // Implement reset logic
            console.log('Resetting device:', selectedDeviceId);
            alert('Device reset successfully');
        }
    }

    // Settings
    function updateSpeedLimit(e) {
        speedLimit = parseInt(e.target.value);
        document.getElementById('speedLimit').textContent = speedLimit;
        updateSpeedDisplay();
    }

    function updateBrightness(e) {
        const value = e.target.value;
        document.getElementById('brightnessValue').textContent = value + '%';
    }

    function saveSettings() {
        if (!selectedDeviceId) {
            alert('Please select a device first');
            return;
        }

        const settings = {
            deviceId: selectedDeviceId,
            speedLimit: document.getElementById('speedLimitInput').value,
            warningThreshold: document.getElementById('warningThreshold').value,
            displayMode: document.getElementById('displayMode').value,
            brightness: document.getElementById('brightness').value
        };

        console.log('Saving settings:', settings);

        // Make AJAX call to save settings
        // fetch('/VASD/SaveSettings', {
        //     method: 'POST',
        //     headers: { 'Content-Type': 'application/json' },
        //     body: JSON.stringify(settings)
        // }).then(response => response.json())
        //   .then(data => {
        //       alert('Settings saved successfully');
        //   });

        alert('Settings saved successfully');
    }

    // Search Devices
    function searchDevices(e) {
        const searchTerm = e.target.value.toLowerCase();
        const devices = document.querySelectorAll('.device-item');

        devices.forEach(device => {
            const name = device.querySelector('h4').textContent.toLowerCase();
            const location = device.querySelector('p').textContent.toLowerCase();
            const id = device.querySelector('.device-id').textContent.toLowerCase();

            if (name.includes(searchTerm) || location.includes(searchTerm) || id.includes(searchTerm)) {
                device.style.display = 'flex';
            } else {
                device.style.display = 'none';
            }
        });
    }

    // Refresh Data
    function refreshData() {
        console.log('Refreshing data...');

        // Show loading indicator
        const btn = document.getElementById('refreshData');
        const originalText = btn.innerHTML;
        btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Refreshing...';
        btn.disabled = true;

        // Simulate data refresh
        setTimeout(() => {
            // Update statistics
            document.getElementById('totalDevices').textContent = Math.floor(Math.random() * 10) + 40;
            document.getElementById('activeDevices').textContent = Math.floor(Math.random() * 5) + 35;
            document.getElementById('alertCount').textContent = Math.floor(Math.random() * 50) + 300;

            btn.innerHTML = originalText;
            btn.disabled = false;

            alert('Data refreshed successfully');
        }, 1500);
    }

    // Export Report
    function exportReport() {
        console.log('Exporting report...');
        alert('Report export feature will generate a PDF/Excel file with all VASD data');

        // Implement export logic
        // window.location.href = '/VASD/ExportReport';
    }

    // Load Detections
    function loadDetections() {
        // This would typically fetch from the server
        console.log('Loading recent detections...');
    }

    // Filter Detections
    function filterDetections() {
        const deviceFilter = document.getElementById('filterDevice').value;
        const violationFilter = document.getElementById('filterViolation').value;

        console.log('Filtering detections:', { deviceFilter, violationFilter });

        // Implement filtering logic
        const rows = document.querySelectorAll('#detectionsTableBody tr');
        rows.forEach(row => {
            let showRow = true;

            // Apply filters
            if (deviceFilter && !row.textContent.includes(deviceFilter)) {
                showRow = false;
            }

            if (violationFilter === 'violation' && !row.classList.contains('violation-row')) {
                showRow = false;
            } else if (violationFilter === 'safe' && row.classList.contains('violation-row')) {
                showRow = false;
            }

            row.style.display = showRow ? '' : 'none';
        });
    }

    // Pagination
    let currentPage = 1;
    function changePage(direction) {
        currentPage += direction;
        if (currentPage < 1) currentPage = 1;

        document.getElementById('currentPage').textContent = currentPage;

        // Load page data
        console.log('Loading page:', currentPage);
    }

    // View Device Details
    function viewDeviceDetails(deviceId) {
        console.log('Viewing device details:', deviceId);
        alert(`Device Details for ${deviceId}\n\nStatus: Active\nLast Updated: Just now\nTotal Detections: 2,456\nViolations: 234`);
    }

    // Edit Device
    function editDevice(deviceId) {
        console.log('Editing device:', deviceId);
        alert(`Edit Device ${deviceId}\n\nThis would open a modal with device configuration options.`);
    }

    // View Detection Details
    function viewDetectionDetails(detectionId) {
        console.log('Viewing detection details:', detectionId);
        alert(`Detection Details\n\nID: ${detectionId}\nVehicle Type: Car\nSpeed: 95 km/h\nLimit: 80 km/h\nImage: Available\nVideo: Available`);
    }

})();