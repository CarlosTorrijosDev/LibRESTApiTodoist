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
    public class CommentServiceTest
    {
        private static CallerRestApiTodoist _callerRestApiTodoist = null;
        private static CommentService _sut = null;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string baseURLRESTApi = ConfigurationManager.AppSettings.Get("BaseURLRESTApi");
            string authenticationToken = ConfigurationManager.AppSettings.Get("AuthenticationToken");
            int retryCount = int.Parse(ConfigurationManager.AppSettings.Get("RetryCount"));
            double delayInSecondsForRetry = double.Parse(ConfigurationManager.AppSettings.Get("DelayInSecondsForRetry"));

            _callerRestApiTodoist = new CallerRestApiTodoist(baseURLRESTApi, authenticationToken, retryCount, delayInSecondsForRetry);

            _sut = new CommentService(_callerRestApiTodoist);
        }


        [TestMethod]
        public async Task TestGetCommentsByTask()
        {
            List<CommentModel> comments = await _sut.GetCommentsByTaskAsync(2525441182);

            Assert.IsTrue(comments.Count > 0);
            Assert.IsTrue(comments.FirstOrDefault().ID > 0);
            Assert.AreEqual("Task comment", comments.FirstOrDefault().Content);
        }

        [TestMethod]
        public async Task TestGetCommentsByProject()
        {
            List<CommentModel> comments = await _sut.GetCommentsByProjectAsync(2178116667);

            Assert.IsTrue(comments.Count > 0);
            Assert.IsTrue(comments.FirstOrDefault().ID > 0);
            Assert.AreEqual("Project comment", comments.FirstOrDefault().Content);
        }

        [TestMethod]
        public async Task TestCreateCommentForTask()
        {
            TaskService taskService = new TaskService(_callerRestApiTodoist);

            TaskModel taskCreated = await taskService.CreateTaskAsync("Task created by TestCreateCommentForTask");
            CommentModel comment = await _sut.CreateCommentForTaskAsync("Comment created by TestCreateCommentForTask", taskCreated.ID);

            Assert.IsNotNull(comment);
            Assert.IsTrue(comment.ID > 0);
            Assert.AreEqual("Comment created by TestCreateCommentForTask", comment.Content);
            Assert.AreEqual(taskCreated.ID, comment.TaskID);
            Assert.IsNull(comment.ProjectID);
            Assert.IsNotNull(comment.Posted);
            Assert.IsNull(comment.Attachment);

            await _sut.DeleteCommentAsync(comment.ID);
            await taskService.DeleteTaskAsync(taskCreated.ID);
        }

        [TestMethod]
        public async Task TestCreateCommentForProject()
        {
            ProjectService projectService = new ProjectService(_callerRestApiTodoist);

            ProjectModel projectCreated = await projectService.CreateProjectAsync("Project created by TestCreateCommentForProject");
            CommentModel comment = await _sut.CreateCommentForProjectAsync("Comment created by TestCreateCommentForProject", projectCreated.ID);

            Assert.IsNotNull(comment);
            Assert.IsTrue(comment.ID > 0);
            Assert.AreEqual("Comment created by TestCreateCommentForProject", comment.Content);
            Assert.AreEqual(projectCreated.ID, comment.ProjectID);
            Assert.IsNull(comment.TaskID);
            Assert.IsNotNull(comment.Posted);
            Assert.IsNull(comment.Attachment);

            await _sut.DeleteCommentAsync(comment.ID);
            await projectService.DeleteProjectAsync(projectCreated.ID);
        }

        [TestMethod]
        public async Task TestGetCommentExists()
        {
            CommentModel comment = await _sut.GetCommentAsync(2249754253);

            Assert.IsNotNull(comment.ID);
            Assert.IsTrue(comment.ID > 0);
            Assert.AreEqual("Task comment", comment.Content);
        }

        [TestMethod]
        public async Task TestGetCommentNotExists()
        {
            CommentModel comment = await _sut.GetCommentAsync(1);

            Assert.IsNull(comment);
        }

        [TestMethod]
        public async Task TestUpdateCommentExists()
        {
            TaskService taskService = new TaskService(_callerRestApiTodoist);

            TaskModel taskCreated = await taskService.CreateTaskAsync("Task created by TestUpdateCommentExists");
            CommentModel commentCreated = await _sut.CreateCommentForTaskAsync("Comment created by TestUpdateCommentExists", taskCreated.ID);
            bool result = await _sut.UpdateCommentAsync(commentCreated.ID, "Comment updated by TestUpdateCommentExists");
            CommentModel commentLoaded = await _sut.GetCommentAsync(commentCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNotNull(commentLoaded);
            Assert.AreEqual(commentCreated.ID, commentLoaded.ID);
            Assert.AreEqual("Comment updated by TestUpdateCommentExists", commentLoaded.Content);
            Assert.AreEqual(taskCreated.ID, commentLoaded.TaskID);
            Assert.IsNull(commentLoaded.ProjectID);
            Assert.AreEqual(commentCreated.Posted, commentLoaded.Posted);
            Assert.IsNull(commentLoaded.Attachment);

            await _sut.DeleteCommentAsync(commentCreated.ID);
            await taskService.DeleteTaskAsync(taskCreated.ID);
        }

        [TestMethod]
        public async Task TestUpdateCommentNotExists()
        {
            bool result = await _sut.UpdateCommentAsync(1, "Comment updated");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task TestDeleteComment()
        {
            TaskService taskService = new TaskService(_callerRestApiTodoist);

            TaskModel taskCreated = await taskService.CreateTaskAsync("Task created by TestDeleteCommentAsync");
            CommentModel comment = await _sut.CreateCommentForTaskAsync("Comment created by TestDeleteCommentAsync", taskCreated.ID);
            bool result = await _sut.DeleteCommentAsync(comment.ID);

            Assert.IsTrue(result);

            await taskService.DeleteTaskAsync(taskCreated.ID);
        }
    }
}
