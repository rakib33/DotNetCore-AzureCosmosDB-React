using Castle.Core.Logging;
using EtiqaFreelancerApi.Common;
using EtiqaFreelancerApi.Controllers;
using EtiqaFreelancerApi.Interfaces;
using EtiqaFreelancerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EtiqaFreelancerApi.Test
{
    public class UsersControllerTest
    {
        private readonly UsersController _usersController;
        //Moq
        private readonly Mock<IUser> _user;
        private readonly Mock<ILogger<BaseApiController>> _logger;
        public UsersControllerTest()
        {
            _user = new Mock<IUser>();
            _logger= new Mock<ILogger<BaseApiController>>();
            _usersController = new UsersController(_user.Object,_logger.Object);
        }

        [Fact]
        public void GetUserList_ReturnActionResult()
        {
            //Arrange
            var ExpectedResut = new List<User> { };
            //act
            var result = _usersController.Get(null);
            //assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Task<ActionResult<List<User>>>>(result);
        }

        [Fact]
        public async void AddUser_ReturnActionResult()
        {
            //Arrange
            var userModel = Mock.Of<User>(x=>x.UserName =="Rakibul Islam" && x.Email =="rakib33mbstu@gmail.com" && x.Hobby=="Travelling" && x.SkillSets == "ASP.NET Core");
            //Act
            var result = await _usersController.RegisterUser(userModel);   
            //Assert
            Assert.NotNull(result);
            //assert            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<UserResponseModel>(okObjectResult.Value);
            Assert.True(resultValue.Status == AppStatus.SuccessStatus);
        }

        [Fact]
        public async void UpdateUser_ReturnActionResult()
        {
            //Arrange
            var userModel = Mock.Of<User>(x => x.Id ==1 && x.UserName == "Rakibul Islam" && x.Email == "rakib33mbstu@gmail.com" && x.Hobby == "Travelling" && x.SkillSets == "ASP.NET Core");
            //Act
            var result = await _usersController.UpdateUser(userModel);
            //Assert
            Assert.NotNull(result);
            //assert            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<UserResponseModel>(okObjectResult.Value);
            Assert.True(resultValue.Status == AppStatus.SuccessStatus);
        }

        [Fact]
        public async void DeleteUser_ReturnActionResult()
        {
            //Arrange
            User userModel = Mock.Of<User>(x => x.Id == 1 && x.UserName == "Rakibul Islam" && x.Email == "rakib33mbstu@gmail.com" && x.Hobby == "Travelling" && x.SkillSets == "ASP.NET Core");
            //Act     
            var result = _usersController.DeleteUser((int)userModel.Id);
            //Assert
            Assert.NotNull(result);
            //assert            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<UserResponseModel>(okObjectResult.Value);
            Assert.True(resultValue.Status == AppStatus.SuccessStatus);
        }
    }
}
