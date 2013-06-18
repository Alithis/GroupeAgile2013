using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteTaLoc.Utilitary;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace NoteTaLoc.Tests.Utilitary
{
    /// <summary>
    /// Description résumée pour UnitTest1
    /// </summary>
    [TestClass]
    public class CategoryStorageTest
    {
        public CategoryStorageTest()
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

        private CategoryStorage initCategoryStorage()
        {
            // recup d'internet http://stackoverflow.com/questions/15133379/how-to-test-custom-configuration-section-in-web-config
            var configurationFileInfo = new FileInfo("../../../NoteTaLoc/Web.config");
            var vdm = new VirtualDirectoryMapping(configurationFileInfo.DirectoryName, true, configurationFileInfo.Name);
            var wcfm = new WebConfigurationFileMap();
            wcfm.VirtualDirectories.Add("/", vdm);
            Configuration conf = WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/");

            TwitterError error = new TwitterError(conf);
            CategoryStorage tmpCat = new CategoryStorage(error, conf);
            return tmpCat;
        }

        //[TestMethod]
        public void ConstructorTest()
        {
            CategoryStorage tmpCat = initCategoryStorage();
            Assert.AreEqual(String.Empty, tmpCat.errorString.ToString());
            
        }

        [TestMethod]
        public void TestGetCategory() {
            CategoryStorage tmpCat = initCategoryStorage();
            Category cat = tmpCat.getCategoryLangId("eng", 10);
            Assert.AreEqual("libelle test 10 eng", cat.libelle);
        }

        [TestMethod]
        public void TestGetCriteria()
        {
            CategoryStorage tmpCat = initCategoryStorage();
            Criteria cat = tmpCat.getCriteriaLangId("eng", 10);
            Assert.AreEqual("criteria libelle 1", cat.libelle);
        }

        [TestMethod]
        public void TestGetCategoryListJSON()
        {
            CategoryStorage tmpCat = initCategoryStorage();
            String test = tmpCat.getCategoryListJSON("eng");
            Assert.AreEqual("[{\"ID\":10,\"LIB\":\"libelle test 10 eng\"},{\"ID\":20,\"LIB\":\"libelle test 20 eng\"}]",test);
        }

        [TestMethod]
        public void TestGetCriteriaListJSON()
        {
            CategoryStorage tmpCat = initCategoryStorage();
            String test = tmpCat.getCriteriaListJSON("fr");
            Assert.AreEqual("[{\"ID\":40,\"LIB\":\"criteria libelle 2\"},{\"ID\":30,\"LIB\":\"criteria libelle 1\"},{\"ID\":20,\"LIB\":\"criteria libelle 2\"},{\"ID\":10,\"LIB\":\"criteria libelle 1\"}]", test);
        }

        //[TestMethod]
        public void DefautSetupTest()
        {
            Categories cats = new Categories();
            cats.language = "eng";
            Category cat = new Category();
            cat.active = true;
            cat.categoryId = 10;
            cat.libelle = "libelle test";
            Category cat2 = new Category();
            cat2.active = true;
            cat2.categoryId = 20;
            cat2.libelle = "libelle test 2";
            Criteria crit1 = new Criteria();
            crit1.active = true;
            crit1.criteriaId = 10;
            crit1.libelle = "criteria libelle 1";
            crit1.weight = 1;
            Criteria crit2 = new Criteria();
            crit2.active = true;
            crit2.criteriaId = 10;
            crit2.libelle = "criteria libelle 2";
            crit2.weight = 1;
            cat.criteriasList = new List<Criteria>();
            cat.criteriasList.Add(crit1);
            cat.criteriasList.Add(crit2);
            cat2.criteriasList = new List<Criteria>();
            cat2.criteriasList.Add(crit1);
            cat2.criteriasList.Add(crit2);
            cats.categoriesList = new List<Category>();
            cats.categoriesList.Add(cat);
            cats.categoriesList.Add(cat2);
            //StreamWriter myWriter = new StreamWriter("../../../NoteTaLoc/test.xml", false);
            //XmlSerializer mySerializer = new XmlSerializer(typeof(Categories));
            //mySerializer.Serialize(myWriter, cats);

        }
    }
}
