using LibRESTApiTodoIst.Model;
using LibRESTApiTodoIst.Service;
using LibRESTApiTodoIst.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace LibRESTApiTodoIstTest.Service
{
    [TestClass]
    public class LabelServiceTest
    {
        private static LabelService _sut = null;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string baseURLRESTApi = ConfigurationManager.AppSettings.Get("BaseURLRESTApi");
            string authenticationToken = ConfigurationManager.AppSettings.Get("AuthenticationToken");
            int retryCount = int.Parse(ConfigurationManager.AppSettings.Get("RetryCount"));
            double delayInSecondsForRetry = double.Parse(ConfigurationManager.AppSettings.Get("DelayInSecondsForRetry"));

            var callerRestApiTodoist = new CallerRestApiTodoist(baseURLRESTApi, authenticationToken, retryCount, delayInSecondsForRetry);

            _sut = new LabelService(callerRestApiTodoist);
        }


        [TestMethod]
        public async Task TestGetAllLabels()
        {
            List<LabelModel> labels = await _sut.GetAllLabelsAsync();

            Assert.IsTrue(labels.Count > 0);
            Assert.IsTrue(labels.FirstOrDefault().ID > 0);
            Assert.AreEqual("FirstLabel", labels.FirstOrDefault().Name);
        }

        [TestMethod]
        public async Task TestCreateLabel()
        {
            LabelModel label = await _sut.CreateLabelAsync("Label_created_by_TestCreateLabel");

            Assert.IsNotNull(label);
            Assert.IsTrue(label.ID > 0);
            Assert.AreEqual("Label_created_by_TestCreateLabel", label.Name);
            Assert.IsTrue(label.Order > 0);

            await _sut.DeleteLabelAsync(label.ID);
        }

        [TestMethod]
        public async Task TestGetLabelExists()
        {
            LabelModel label = await _sut.GetLabelAsync(2149617762);

            Assert.IsNotNull(label.ID);
            Assert.IsTrue(label.ID > 0);
            Assert.AreEqual("FirstLabel", label.Name);
        }

        [TestMethod]
        public async Task TestGetLabelNotExists()
        {
            LabelModel label = await _sut.GetLabelAsync(1);

            Assert.IsNull(label);
        }

        [TestMethod]
        public async Task TestUpdateLabelExists()
        {
            LabelModel labelCreated = await _sut.CreateLabelAsync("Label_created_by_TestCreateLabel");
            bool result = await _sut.UpdateLabelAsync(labelCreated.ID, "Label_updated_by_TestCreateLabel");
            LabelModel labelLoaded = await _sut.GetLabelAsync(labelCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNotNull(labelLoaded);
            Assert.AreEqual(labelCreated.ID, labelLoaded.ID);
            Assert.AreEqual("Label_updated_by_TestCreateLabel", labelLoaded.Name);
            Assert.AreEqual(labelCreated.Order, labelLoaded.Order);

            await _sut.DeleteLabelAsync(labelCreated.ID);
        }

        [TestMethod]
        public async Task TestUpdateLabelNotExists()
        {
            bool result = await _sut.UpdateLabelAsync(1, "Label_updated");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task TestDeleteLabel()
        {
            LabelModel label = await _sut.CreateLabelAsync("Label_created_by_TestDeleteLabel");
            bool result = await _sut.DeleteLabelAsync(label.ID);

            Assert.IsTrue(result);
        }
    }
}
