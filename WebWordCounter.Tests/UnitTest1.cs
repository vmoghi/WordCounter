using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebWordCounter.Controllers;
using WebWordCounter.Common;
using WebWordCounter.Models;
using WebWordCounter.WebServices;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using Microsoft.QualityTools.Testing.Fakes;
namespace WebWordCounter.Tests
{
    [TestClass]
    public class UnitTest1
    {
        HomeController _homeController = null;

        [TestInitialize]
        public void SetUp()
        {
            IEnumerable<WordModel> enumList = null;
            var u = new WebWordCounter.Common.Fakes.StubIBuildWordCloud()
            {
                GetListTopWordsString = (url) => { return enumList; }
            };
            _homeController = new HomeController(u);
        }
        [TestMethod]
        public void Main_Without_Parameters_Action_Should_Return_Main_View_For_Default_HomePage()
        {
           
            ViewResult viewResult = _homeController.Main() as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Main", viewResult.ViewName); 
        }

        [TestMethod]
        public void Main_With_Parameters_UrlEmpty_ReturnsEmptyResult()
        {
          
            var collection = new FormCollection();
            collection.Add("urlPath", "");
            ActionResult result = _homeController.Main(collection);
            Assert.IsInstanceOfType(result, typeof(EmptyResult));

        }
        [TestMethod]
        public void Main_With_Parameters_Url_Not_Sarting_HTML_ReturnsEmptyResult()
        {
            var collection = new FormCollection();
            collection.Add("urlPath", "test");
            ActionResult result = _homeController.Main(collection);
            Assert.IsInstanceOfType(result, typeof(EmptyResult));

        }
        [TestMethod]
        public void Main_With_Parameters_UrlCorrect_Returns_EmptyResult_With_Null_List()
        {
            var collection = new FormCollection();
            collection.Add("urlPath", "http://www.bbc.com");

            ActionResult result = _homeController.Main(collection);
            Assert.IsInstanceOfType(result, typeof(EmptyResult));
        }

        [TestMethod]
        public void Main_With_Parameters_Returns_PartialViewResult_When_Some_Words_are_Avaialble()
        {
            
            var collection = new FormCollection();
            collection.Add("urlPath", "http://www.bbc.com");

            var list = new List<WordModel>();
            list.Add(new WordModel());
            var enumList = list as IEnumerable<WordModel>;
            var u = new WebWordCounter.Common.Fakes.StubIBuildWordCloud()
            {
                GetListTopWordsString = (url) => { return enumList; }
            };
            _homeController = new HomeController(u);
            ActionResult result = _homeController.Main(collection);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public void BuildWordCloud_Empy_url_Returns_Null_List()
        {
            IBuildWordCloud buidCloud = new BuildWordCloud();
            var result=buidCloud.GetListTopWords("");

            Assert.AreEqual(null, result); 
        }

        [TestMethod]
        public void BuildWordCloud_Ruturn_Nell_when_Empty_file_from_Website()
        {

            using (ShimsContext.Create())
            {
                WebWordCounter.WebServices.Fakes.ShimProcessWebService.APIHttpRequestString = (url) =>
                {
                    return "";
                };

                BuildWordCloud b = new BuildWordCloud();
                var counterWords = b.GetListTopWords("http://www.bbc.com");
                Assert.AreEqual(null, counterWords);
            }
         
        }

        [TestMethod]
        public void BuildWordCloud_Ruturn_five_words()
        {

            using (ShimsContext.Create())
            {
                WebWordCounter.WebServices.Fakes.ShimProcessWebService.APIHttpRequestString = (url) =>
                {
                    return "ciao hello hi hola salve";
                };

                BuildWordCloud build = new BuildWordCloud();
                var counter = 1;
                var counterWords = build.GetListTopWords("http://www.bbc.com");
                foreach (var word in counterWords)
                {
                    counter++;
                }
                Assert.AreEqual(5, counter);
            }
         
        }
        
    }
}
