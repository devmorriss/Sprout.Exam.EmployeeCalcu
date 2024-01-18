using AutoMapper;
using FluentValidation;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Sprout.Exam.Business.Core;
using Sprout.Exam.DataAccess.Models;
using Sprout.Exam.WebApp.Business.Employees;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Factories;
using Sprout.Exam.WebApp.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Sprout.Exam.WebApp.Business.Employees.Calculate;

namespace Sprout.Exam.UnitTests
{
    public class CalculateCommandHandlerTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly ApplicationDbContext _dbContext;
        private readonly EmployeeSalaryCalculatorFactory _calculatorFactory;

        public CalculateCommandHandlerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _dbContext = new ApplicationDbContext(_options, Options.Create(new OperationalStoreOptions()));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            _calculatorFactory= new EmployeeSalaryCalculatorFactory();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task Handle_Should_CalculateSalaryCorrectlyForRegular()
        {
            // Arrange
            _dbContext.Database.EnsureCreated();
            _dbContext.Employees.Add(new Employee
            {
                Id = 1,
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            });

            _dbContext.Employees.Add(new Employee
            {
                Id = 2,
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jane Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Contractual
            });
            _dbContext.SaveChanges();

            var testCommand = new Calculate.Command { Id = 1, Days = 5 };
            var expectedSalary = 13600m;

            // Act
            var handler = new Calculate.Handler(_dbContext, _calculatorFactory, new CommandValidator());

            var result = await handler.Handle(testCommand, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSalary, result.Value);
        }

        [Fact]
        public async Task Handle_Should_CalculateSalaryCorrectlyForContractual()
        {
            // Arrange
            _dbContext.Database.EnsureCreated();
            _dbContext.Employees.Add(new Employee
            {
                Id = 1,
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            });

            _dbContext.Employees.Add(new Employee
            {
                Id = 2,
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jane Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Contractual
            });
            _dbContext.SaveChanges();

            var testCommand = new Calculate.Command { Id = 2, Days = 2.2m };
            var expectedSalary = 1100m;

            // Act
            var handler = new Calculate.Handler(_dbContext, _calculatorFactory, new CommandValidator());

            var result = await handler.Handle(testCommand, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSalary, result.Value);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenDaysGreaterThanWorkingDays()
        {
            // Arrange
            _dbContext.Database.EnsureCreated();
            _dbContext.Employees.Add(new Employee
            {
                Id = 1,
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            });

            _dbContext.Employees.Add(new Employee
            {
                Id = 2,
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jane Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Contractual
            });
            _dbContext.SaveChanges();

            var testCommand = new Calculate.Command { Id = 2, Days = 22.1m };
            var expectedSalary = 0m;

            // Act & Assert
            var handler = new Calculate.Handler(_dbContext, _calculatorFactory, new CommandValidator());

            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(testCommand, default));
            Assert.Contains("'Days' must be less than or equal to '22'.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenDaysLessThanOne()
        {
            // Arrange
            _dbContext.Database.EnsureCreated();
            _dbContext.Employees.Add(new Employee
            {
                Id = 1,
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            });

            _dbContext.Employees.Add(new Employee
            {
                Id = 2,
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jane Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Contractual
            });
            _dbContext.SaveChanges();

            var testCommand = new Calculate.Command { Id = 1, Days = -1 };
            var expectedSalary = 0m;

            // Act & Assert
            var handler = new Calculate.Handler(_dbContext, _calculatorFactory, new CommandValidator());

            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(testCommand, default));
            Assert.Contains("'Days' must be greater than or equal to '0'.", exception.Errors.First().ErrorMessage);
        }
    }
}
