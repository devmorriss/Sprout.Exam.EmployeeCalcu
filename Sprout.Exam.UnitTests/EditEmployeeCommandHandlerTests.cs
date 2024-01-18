using AutoMapper;
using FluentValidation;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sprout.Exam.Business.Core;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.DataAccess.Models;
using Sprout.Exam.WebApp.Business.Employees;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Sprout.Exam.WebApp.Business.Employees.Edit;

namespace Sprout.Exam.UnitTests
{
    public class EditEmployeeCommandHandlerTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<Edit.Command> _validator;

        public EditEmployeeCommandHandlerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _dbContext = new ApplicationDbContext(_options, Options.Create(new OperationalStoreOptions()));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            _mapper = mappingConfig.CreateMapper();

            _validator = new CommandValidator();

        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFullNameIsEmpty()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var existingEmployee = new Employee
            {
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            };

            _dbContext.Employees.Add(existingEmployee);
            await _dbContext.SaveChangesAsync();

            var command = new Edit.Command
            {
                Employee = new Business.DataTransferObjects.EditEmployeeDto
                {
                    Id = existingEmployee.Id,
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "",
                    Tin = "12345",
                    TypeId = (int)Common.Enums.EmployeeType.Regular
                }
            };

            var commandHandler = new Edit.Handler(_dbContext, _mapper, _validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => commandHandler.Handle(command, default));
            Assert.Contains("'Full Name' must not be empty.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenTinIsEmpty()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var existingEmployee = new Employee
            {
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            };

            _dbContext.Employees.Add(existingEmployee);
            await _dbContext.SaveChangesAsync();

            var command = new Edit.Command
            {
                Employee = new Business.DataTransferObjects.EditEmployeeDto
                {
                    Id = existingEmployee.Id,
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "Jason Doe",
                    Tin = "",
                    TypeId = (int)Common.Enums.EmployeeType.Regular
                }
            };

            var commandHandler = new Edit.Handler(_dbContext, _mapper, _validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => commandHandler.Handle(command, default));
            Assert.Contains("'Tin' must not be empty.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFullNameHasNumbers()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var existingEmployee = new Employee
            {
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            };

            _dbContext.Employees.Add(existingEmployee);
            await _dbContext.SaveChangesAsync();

            var command = new Edit.Command
            {
                Employee = new Business.DataTransferObjects.EditEmployeeDto
                {
                    Id = existingEmployee.Id,
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "Jason Doe12",
                    Tin = "12345",
                    TypeId = (int)Common.Enums.EmployeeType.Regular
                }
            };

            var commandHandler = new Edit.Handler(_dbContext, _mapper, _validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => commandHandler.Handle(command, default));
            Assert.Contains("Full Name must not contain numbers.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenAgeIsBelow18()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var existingEmployee = new Employee
            {
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            };

            _dbContext.Employees.Add(existingEmployee);
            await _dbContext.SaveChangesAsync();

            var command = new Edit.Command
            {
                Employee = new Business.DataTransferObjects.EditEmployeeDto
                {
                    Id = existingEmployee.Id,
                    Birthdate = DateTime.Now.AddYears(-2),
                    FullName = "Jason Doe",
                    Tin = "12345",
                    TypeId = (int)Common.Enums.EmployeeType.Regular
                }
            };

            var commandHandler = new Edit.Handler(_dbContext, _mapper, _validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => commandHandler.Handle(command, default));
            Assert.Contains("You must be at least 18 years old.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenTypeIdIsInvalid()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var existingEmployee = new Employee
            {
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            };

            _dbContext.Employees.Add(existingEmployee);
            await _dbContext.SaveChangesAsync();

            var command = new Edit.Command
            {
                Employee = new Business.DataTransferObjects.EditEmployeeDto
                {
                    Id = existingEmployee.Id,
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "Jason Doe",
                    Tin = "12345",
                    TypeId = 5
                }
            };

            var commandHandler = new Edit.Handler(_dbContext, _mapper, _validator);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenAllFieldsArePopulatedCorrectly()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var existingEmployee = new Employee
            {
                BirthDate = DateTime.Now.AddYears(-25),
                FullName = "Jason Doe",
                Tin = "12345",
                EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular
            };

            _dbContext.Employees.Add(existingEmployee);
            await _dbContext.SaveChangesAsync();

            var command = new Edit.Command
            {
                Employee = new Business.DataTransferObjects.EditEmployeeDto
                {
                    Id = existingEmployee.Id,
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "Jason Doe",
                    Tin = "12345",
                    TypeId = (int)Common.Enums.EmployeeType.Contractual
                }
            };

            var commandHandler = new Edit.Handler(_dbContext, _mapper, _validator);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<EmployeeDto>(result);
        }
    }
}
