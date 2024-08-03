using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientManagementApi.Controllers;
using PatientManagementApi.Dtos;
using PatientManagementApi.Dtos.Statistics;
using PatientManagementApi.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagementApi.Tests.ControllerTests
{
    public class StatisticsAPIControllerTests
    {
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly StatisticsAPIController _controller;
        private readonly ResponseDto _response;

        public StatisticsAPIControllerTests()
        {
            _mockPatientService = new Mock<IPatientService>();
            _response = new ResponseDto();
            _controller = new StatisticsAPIController(_mockPatientService.Object);
        }
        #region  GetPatientStatistics
        [Fact]
        public async Task GetPatientStatistics_WhenDataExists_ReturnsOk()
        {
            // Arrange
            var statistics = new PatientsStatistic { TotalPatient = 100 };
            _mockPatientService.Setup(x => x.GetPatientsStatistic()).ReturnsAsync(statistics);
            _response.Result = statistics;

            // Act
            var result = await _controller.GetPatientStatistics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(statistics, ((ResponseDto)okResult.Value).Result);
        }
        #endregion
        #region  GetPatientStatistics
        [Fact]
        public async Task GetTodayPatientStatistics_WhenDataExists_ReturnsOk()
        {
            // Arrange
            var todayStatistics = new TodayPatientsStatistic { TodayNewPatient = 10 };
            _mockPatientService.Setup(x => x.GetTodayPatientsStatistic()).ReturnsAsync(todayStatistics);
            _response.Result = todayStatistics;

            // Act
            var result = await _controller.GetTodayPatientStatistics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(todayStatistics, ((ResponseDto)okResult.Value).Result);
        }

        #endregion
    }
}
