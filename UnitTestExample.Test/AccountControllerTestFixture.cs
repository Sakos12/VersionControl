using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;
using System.Activities;
using Moq;
using UnitTestExample.Abstractions;
using UnitTestExample.Entities;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        [Test,
             TestCase("abcd1234", false),
             TestCase("irf@uni-corvinus", false),
             TestCase("irf.uni-corvinus.hu", false),
             TestCase("irf@uni-corvinus.hu", true)]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            var accountController = new AccountController();

            var actualResult = accountController.ValidateEmail(email);

            Assert.AreEqual(expectedResult, actualResult);

            
        }
        [Test,
            TestCase("abCdEfgm", false),
            TestCase("ASD2HGJ34",false),
            TestCase("agtrh45th",false),
            TestCase("aRc45",false),
            TestCase("Sajtos987",true)]
        public void TestValidatePassword(string password, bool expectedResult)
        {
            var accountController = new AccountController();

            var actualResult = accountController.ValidatePassword(password);

            Assert.AreEqual(expectedResult, actualResult);
        }
        [Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "Abcd1234567"),]
        public void TestRegisterHappyPath(string email, string password)
        {
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock.Setup(m => m.CreateAccount(It.IsAny<Account>())).Returns<Account>(a => a);
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;

            var actualResult = accountController.Register(email, password);

            Assert.AreEqual(email, actualResult);
            Assert.AreEqual(password, actualResult);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
            accountServiceMock.Verify(m => m.CreateAccount(actualResult), Times.Once);
        }
        [Test,
            TestCase("irf@uni-corvinus", "Abcd1234"),
            TestCase("irf.uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "abcd1234"),
            TestCase("irf@uni-corvinus.hu", "ABCD1234"),
            TestCase("irf@uni-corvinus.hu", "abcdABCD"),
            TestCase("irf@uni-corvinus.hu", "Ab1234"),
]
        public void TestRegisterValidateException(string email, string password)
        {
            var accountController = new AccountController();

            try
            {
                var actualResult = accountController.Register(email, password);
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.IsInstanceOf<ValidationException>(ex);
            }
        }
        [Test,
            TestCase("irf@uni-corvinus.hu","Abcd1234")]
        public void TestRegisterApplicationException(string newEmail, string newPassword)
        {
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock.Setup(m => m.CreateAccount(It.IsAny<Account>())).Throws<ApplicationException>();
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;

            try
            {
                var actualResult = accountController.Register(newEmail, newPassword);
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.IsInstanceOf<ApplicationException>(ex);
            }
        }
    }
}
