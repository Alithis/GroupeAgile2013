using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteTaLoc.Controllers;

namespace NoteTaLoc.Tests.Controllers
{
    /// <summary>
    /// Description résumée pour AccountControllerTest
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {
        public AccountController accountController = null;
        
        public AccountControllerTest()
        {
            //
            // TODO: ajoutez ici la logique du constructeur
            //
        }

        [TestInitialize]
        public void Initialize()
        {
            accountController = new AccountController();
        }

        [TestCleanup]
        public void cleanUp()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Attributs de tests supplémentaires
        //
        // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
        //
        // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestDoesUserNameExist()
        {
            bool actual = this.accountController.DoesUserNameExist("something");
            Assert.IsFalse(actual);

            actual = this.accountController.DoesUserNameExist("shum");
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestValidateUser_Password()
        {
            bool actual = this.accountController.ValidateUser_Password("shum", "321654");
            Assert.IsFalse(actual);

            actual = this.accountController.ValidateUser_Password("shum", "123456");
            Assert.IsTrue(actual);
        }
    }
}
