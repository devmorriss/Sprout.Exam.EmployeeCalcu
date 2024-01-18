using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Options;
using Moq;
using Sprout.Exam.Business.Core;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Business.Employees;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Sprout.Exam.WebApp.Business.Employees.Create;

namespace Sprout.Exam.UnitTests
{
    public class CreateEmployeeCommandHandlerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<Create.Command> _validator;

        public CreateEmployeeCommandHandlerTests()
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
        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFullNameIsEmpty()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var command = new Create.Command
            {
                Employee = new Business.DataTransferObjects.CreateEmployeeDto
                { 
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "",
                    Tin = "12345",
                    TypeId = (int)EmployeeType.Regular
                } 
            };

            var commandHandler = new Create.Handler(_dbContext, _mapper, _validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => commandHandler.Handle(command, default));
            Assert.Contains("'Full Name' must not be empty.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenTinIsEmpty()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var command = new Create.Command
            {
                Employee = new Business.DataTransferObjects.CreateEmployeeDto
                {
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "John Doe",
                    Tin = "",
                    TypeId = (int)EmployeeType.Regular
                }
            };

            var commandHandler = new Create.Handler(_dbContext, _mapper, _validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => commandHandler.Handle(command, default));
            Assert.Contains("'Tin' must not be empty.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFullNameHasNumbers()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var command = new Create.Command
            {
                Employee = new Business.DataTransferObjects.CreateEmployeeDto
                {
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "John Doe12",
                    Tin = "12345",
                    TypeId = (int)EmployeeType.Regular
                }
            };

            var commandHandler = new Create.Handler(_dbContext, _mapper, _validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => commandHandler.Handle(command, default));
            Assert.Contains("Full Name must not contain numbers.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenAgeIsBelow18()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var command = new Create.Command
            {
                Employee = new Business.DataTransferObjects.CreateEmployeeDto
                {
                    Birthdate = DateTime.Now.AddYears(-2),
                    FullName = "John Doe",
                    Tin = "12345",
                    TypeId = (int)EmployeeType.Regular
                }
            };

            var commandHandler = new Create.Handler(_dbContext, _mapper, _validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => commandHandler.Handle(command, default));
            Assert.Contains("You must be at least 18 years old.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenTypeIdIsInvalid()
        {
            //Arrange
            _dbContext.Database.EnsureCreated();

            var command = new Create.Command
            {
                Employee = new Business.DataTransferObjects.CreateEmployeeDto
                {
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "John Doe",
                    Tin = "12345",
                    TypeId = 5
                }
            };

            var commandHandler = new Create.Handler(_dbContext, _mapper, _validator);

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

            var command = new Create.Command
            {
                Employee = new Business.DataTransferObjects.CreateEmployeeDto
                {
                    Birthdate = DateTime.Now.AddYears(-25),
                    FullName = "John Doe",
                    Tin = "12345",
                    TypeId = (int)EmployeeType.Regular
                }
            };

            var commandHandler = new Create.Handler(_dbContext, _mapper, _validator);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<int>(result);
        }
    }
}
