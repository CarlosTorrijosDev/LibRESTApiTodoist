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
    public class ProjectServiceTest
    {
        private static ProjectService _sut = null;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string baseURLRESTApi = ConfigurationManager.AppSettings.Get("BaseURLRESTApi");
            string authenticationToken = ConfigurationManager.AppSettings.Get("AuthenticationToken");
            int retryCount = int.Parse(ConfigurationManager.AppSettings.Get("RetryCount"));
            double delayInSecondsForRetry = double.Parse(ConfigurationManager.AppSettings.Get("DelayInSecondsForRetry"));

            var callerRestApiTodoist = new CallerRestApiTodoist(baseURLRESTApi, authenticationToken, retryCount, delayInSecondsForRetry);

            _sut = new ProjectService(callerRestApiTodoist);
        }


        [TestMethod]
        public async Task TestGetAllProjects()
        {
            List<ProjectModel> projects = await _sut.GetAllProjectsAsync();

            Assert.IsTrue(projects.Count > 0);
            Assert.IsTrue(projects.FirstOrDefault().ID > 0);
            Assert.AreEqual("Inbox", projects.FirstOrDefault().Name);
        }

        [TestMethod]
        public async Task TestCreateProject()
        {
            ProjectModel project = await _sut.CreateProjectAsync("Project created by TestCreateProject");

            Assert.IsNotNull(project);
            Assert.IsTrue(project.ID > 0);
            Assert.AreEqual("Project created by TestCreateProject", project.Name);
            Assert.AreEqual(0, project.CommentCount);
            Assert.IsTrue(project.Order > 0);
            Assert.AreEqual(1, project.Indent);

            await _sut.DeleteProjectAsync(project.ID);
        }

        [TestMethod]
        public async Task TestGetProjectExists()
        {
            ProjectModel project = await _sut.GetProjectAsync(2178114772);

            Assert.IsNotNull(project.ID);
            Assert.IsTrue(project.ID > 0);
            Assert.AreEqual("Inbox", project.Name);
        }

        [TestMethod]
        public async Task TestGetProjectNotExists()
        {
            ProjectModel project = await _sut.GetProjectAsync(1);

            Assert.IsNull(project);
        }

        [TestMethod]
        public async Task TestUpdateProjectExists()
        {
            ProjectModel projectCreated = await _sut.CreateProjectAsync("Project created by TestUpdateProjectExists");
            bool result = await _sut.UpdateProjectAsync(projectCreated.ID, "Project updated by TestUpdateProjectExists");
            ProjectModel projectLoaded = await _sut.GetProjectAsync(projectCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNotNull(projectLoaded);
            Assert.AreEqual(projectCreated.ID, projectLoaded.ID);
            Assert.AreEqual("Project updated by TestUpdateProjectExists", projectLoaded.Name);
            Assert.AreEqual(projectCreated.CommentCount, projectLoaded.CommentCount);
            Assert.AreEqual(projectCreated.Order, projectLoaded.Order);
            Assert.AreEqual(projectCreated.Indent, projectLoaded.Indent);

            await _sut.DeleteProjectAsync(projectCreated.ID);
        }

        [TestMethod]
        public async Task TestUpdateProjectNotExists()
        {
            bool result = await _sut.UpdateProjectAsync(1, "Project updated");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task TestDeleteProject()
        {
            ProjectModel project = await _sut.CreateProjectAsync("Project created by TestDeleteProject");
            bool result = await _sut.DeleteProjectAsync(project.ID);

            Assert.IsTrue(result);
        }
    }
}
