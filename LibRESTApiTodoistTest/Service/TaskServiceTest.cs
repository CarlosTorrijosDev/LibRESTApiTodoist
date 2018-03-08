using LibRESTApiTodoIst.Model;
using LibRESTApiTodoIst.Service;
using LibRESTApiTodoIst.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace LibRESTApiTodoIstTest.Service
{
    [TestClass]
    public class TaskServiceTest
    {
        private static TaskService _sut = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string baseURLRESTApi = ConfigurationManager.AppSettings.Get("BaseURLRESTApi");
            string authenticationToken = ConfigurationManager.AppSettings.Get("AuthenticationToken");
            int retryCount = int.Parse(ConfigurationManager.AppSettings.Get("RetryCount"));
            double delayInSecondsForRetry = double.Parse(ConfigurationManager.AppSettings.Get("DelayInSecondsForRetry"));

            var callerRestApiTodoist = new CallerRestApiTodoist(baseURLRESTApi, authenticationToken, retryCount, delayInSecondsForRetry);

            _sut = new TaskService(callerRestApiTodoist);
        }


        [TestMethod]
        public async Task TestGetAllTasks()
        {
            List<TaskModel> tasks = await _sut.GetAllTasksAsync();

            Assert.IsTrue(tasks.Count > 0);
            Assert.IsTrue(tasks.FirstOrDefault().ID > 0);
            Assert.AreEqual("First task", tasks.FirstOrDefault().Content);
        }

        [TestMethod]
        public async Task TestCreateTaskOnlyContent()
        {
            TaskModel task = await _sut.CreateTaskAsync("Task created by TestCreateTaskOnlyContent");

            Assert.IsNotNull(task);
            Assert.IsTrue(task.ID > 0);
            Assert.IsTrue(task.ProjectID > 0);
            Assert.AreEqual("Task created by TestCreateTaskOnlyContent", task.Content);
            Assert.IsFalse(task.Completed);
            Assert.IsNull(task.LabelIDs);
            Assert.IsTrue(task.Order > 0);
            Assert.AreEqual(1, task.Indent);
            Assert.AreEqual(TaskModel.PriorityType.p4, task.Priority);
            Assert.IsNull(task.Due);
            Assert.IsNull(task.Url);
            Assert.AreEqual(0, task.CommentCount);

            await _sut.DeleteTaskAsync(task.ID);
        }

        [TestMethod]
        public async Task TestCreateTaskAllFieldsWithoutLabelsDueString()
        {
            DateTime baseDate = DateTime.Now;
            DateTime referencedDateTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 9, 0, 0);

            TaskModel task = await _sut.CreateTaskAsync("Task created by TestCreateTaskAllFieldsWithoutLabelsDueString", 2178114772, 1, null, TaskModel.PriorityType.p1, "hoy 09:00", "es");

            Assert.IsNotNull(task);
            Assert.IsTrue(task.ID > 0);
            Assert.AreEqual(2178114772, task.ProjectID);
            Assert.AreEqual("Task created by TestCreateTaskAllFieldsWithoutLabelsDueString", task.Content);
            Assert.IsFalse(task.Completed);
            Assert.IsNull(task.LabelIDs);
            Assert.IsTrue(task.Order > 0);
            Assert.AreEqual(1, task.Indent);
            Assert.AreEqual(TaskModel.PriorityType.p1, task.Priority);
            Assert.IsNotNull(task.Due);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-dd"), task.Due.Date);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-ddThh:mm:ssZ"), task.Due.Datetime);
            Assert.AreEqual("hoy 09:00", task.Due.String);
            Assert.AreEqual("Europe/Madrid", task.Due.Timezone);
            Assert.IsNull(task.Url);
            Assert.AreEqual(0, task.CommentCount);

            await _sut.DeleteTaskAsync(task.ID);
        }

        [TestMethod]
        public async Task TestCreateTaskAllFieldsWithoutLabelsDueDate()
        {
            DateTime baseDate = DateTime.Now.AddDays(1);
            DateTime referencedDateTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 0, 0, 0);

            TaskModel task = await _sut.CreateTaskAsync("Task created by TestCreateTaskAllFieldsWithoutLabelsDueDate", 2178114772, 1, null, TaskModel.PriorityType.p1, referencedDateTime);

            Assert.IsNotNull(task);
            Assert.IsTrue(task.ID > 0);
            Assert.AreEqual(2178114772, task.ProjectID);
            Assert.AreEqual("Task created by TestCreateTaskAllFieldsWithoutLabelsDueDate", task.Content);
            Assert.IsFalse(task.Completed);
            Assert.IsNull(task.LabelIDs);
            Assert.IsTrue(task.Order > 0);
            Assert.AreEqual(1, task.Indent);
            Assert.AreEqual(TaskModel.PriorityType.p1, task.Priority);
            Assert.IsNotNull(task.Due);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-dd"), task.Due.Date);
            Assert.IsNull(task.Due.Datetime);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-dd"), task.Due.String);
            Assert.IsNull(task.Due.Timezone);
            Assert.IsNull(task.Url);
            Assert.AreEqual(0, task.CommentCount);

            await _sut.DeleteTaskAsync(task.ID);
        }

        [TestMethod]
        public async Task TestCreateTaskAllFieldsWithoutLabelsDueDateTime()
        {
            DateTime baseDate = DateTime.Now.AddDays(1);
            DateTime referencedDateTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 9, 0, 0).ToUniversalTime();

            TaskModel task = await _sut.CreateTaskAsync("Task created by TestCreateTaskAllFieldsWithoutLabelsDueDateTime", 2178114772, 1, null, TaskModel.PriorityType.p1, referencedDateTime);

            Assert.IsNotNull(task);
            Assert.IsTrue(task.ID > 0);
            Assert.AreEqual(2178114772, task.ProjectID);
            Assert.AreEqual("Task created by TestCreateTaskAllFieldsWithoutLabelsDueDateTime", task.Content);
            Assert.IsFalse(task.Completed);
            Assert.IsNull(task.LabelIDs);
            Assert.IsTrue(task.Order > 0);
            Assert.AreEqual(1, task.Indent);
            Assert.AreEqual(TaskModel.PriorityType.p1, task.Priority);
            Assert.IsNotNull(task.Due);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-dd"), task.Due.Date);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-ddThh:mm:ssZ"), task.Due.Datetime);
            Assert.AreEqual(referencedDateTime.ToLocalTime().ToString("yyyy-MM-dd hh:mm"), task.Due.String);
            Assert.AreEqual("Europe/Madrid", task.Due.Timezone);
            Assert.IsNull(task.Url);
            Assert.AreEqual(0, task.CommentCount);

            await _sut.DeleteTaskAsync(task.ID);
        }

        [TestMethod]
        public async Task TestGetTaskExists()
        {
            TaskModel task = await _sut.GetTaskAsync(2525441182);

            Assert.IsNotNull(task.ID);
            Assert.IsTrue(task.ID > 0);
            Assert.AreEqual("First task", task.Content);
        }

        [TestMethod]
        public async Task TestGetTaskNotExists()
        {
            TaskModel task = await _sut.GetTaskAsync(1);

            Assert.IsNull(task);
        }

        [TestMethod]
        public async Task TestUpdateTaskOnlyContent()
        {
            TaskModel taskCreated = await _sut.CreateTaskAsync("Task created by TestUpdateTaskOnlyContent");
            var result = await _sut.UpdateTaskAsync(taskCreated.ID, "Task updated by TestUpdateTaskOnlyContent");
            TaskModel taskLoaded = await _sut.GetTaskAsync(taskCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNotNull(taskLoaded);
            Assert.AreEqual(taskCreated.ID, taskLoaded.ID);
            Assert.AreEqual(taskCreated.ProjectID, taskLoaded.ProjectID);
            Assert.AreEqual("Task updated by TestUpdateTaskOnlyContent", taskLoaded.Content);
            Assert.AreEqual(taskCreated.Completed, taskLoaded.Completed);
            Assert.IsNull(taskLoaded.LabelIDs);
            Assert.AreEqual(taskCreated.Order, taskLoaded.Order);
            Assert.AreEqual(taskCreated.Indent, taskLoaded.Indent);
            Assert.AreEqual(taskCreated.Priority, taskLoaded.Priority);
            Assert.IsNull(taskLoaded.Due);
            Assert.IsNull(taskLoaded.Url);
            Assert.AreEqual(taskCreated.CommentCount, taskLoaded.CommentCount);

            await _sut.DeleteTaskAsync(taskCreated.ID);
        }

        [TestMethod]
        public async Task TestUpdateTaskAllFieldsWithoutLabelsDueString()
        {
            DateTime baseDate = DateTime.Now;
            DateTime referencedDateTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 9, 0, 0);

            TaskModel taskCreated = await _sut.CreateTaskAsync("Task created by TestUpdateTaskAllFieldsWithoutLabelsDueString");
            var result = await _sut.UpdateTaskAsync(taskCreated.ID, "Task updated by TestUpdateTaskAllFieldsWithoutLabelsDueString", 2178114772, null, TaskModel.PriorityType.p1, "hoy 09:00", "es");
            TaskModel taskLoaded = await _sut.GetTaskAsync(taskCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNotNull(taskLoaded);
            Assert.AreEqual(taskCreated.ID, taskLoaded.ID);
            Assert.AreEqual(taskCreated.ProjectID, taskLoaded.ProjectID);
            Assert.AreEqual("Task updated by TestUpdateTaskAllFieldsWithoutLabelsDueString", taskLoaded.Content);
            Assert.AreEqual(taskCreated.Completed, taskLoaded.Completed);
            Assert.IsNull(taskLoaded.LabelIDs);
            Assert.AreEqual(taskCreated.Order, taskLoaded.Order);
            Assert.AreEqual(taskCreated.Indent, taskLoaded.Indent);
            Assert.AreEqual(TaskModel.PriorityType.p1, taskLoaded.Priority);
            Assert.IsNotNull(taskLoaded.Due);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-dd"), taskLoaded.Due.Date);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-ddThh:mm:ssZ"), taskLoaded.Due.Datetime);
            Assert.AreEqual("hoy 09:00", taskLoaded.Due.String);
            Assert.AreEqual("Europe/Madrid", taskLoaded.Due.Timezone);
            Assert.IsNull(taskLoaded.Url);
            Assert.AreEqual(taskCreated.CommentCount, taskLoaded.CommentCount);

            await _sut.DeleteTaskAsync(taskCreated.ID);
        }

        [TestMethod]
        public async Task TestUpdateTaskAllFieldsWithoutLabelsDueDate()
        {
            DateTime baseDate = DateTime.Now.AddDays(1);
            DateTime referencedDateTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 0, 0, 0);

            TaskModel taskCreated = await _sut.CreateTaskAsync("Task created by TestUpdateTaskAllFieldsWithoutLabelsDueDate");
            var result = await _sut.UpdateTaskAsync(taskCreated.ID, "Task updated by TestUpdateTaskAllFieldsWithoutLabelsDueDate", 2178114772, null, TaskModel.PriorityType.p1, referencedDateTime);
            TaskModel taskLoaded = await _sut.GetTaskAsync(taskCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNotNull(taskLoaded);
            Assert.AreEqual(taskCreated.ID, taskLoaded.ID);
            Assert.AreEqual(taskCreated.ProjectID, taskLoaded.ProjectID);
            Assert.AreEqual("Task updated by TestUpdateTaskAllFieldsWithoutLabelsDueDate", taskLoaded.Content);
            Assert.AreEqual(taskCreated.Completed, taskLoaded.Completed);
            Assert.IsNull(taskLoaded.LabelIDs);
            Assert.AreEqual(taskCreated.Order, taskLoaded.Order);
            Assert.AreEqual(taskCreated.Indent, taskLoaded.Indent);
            Assert.AreEqual(TaskModel.PriorityType.p1, taskLoaded.Priority);
            Assert.IsNotNull(taskLoaded.Due);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-dd"), taskLoaded.Due.Date);
            Assert.IsNull(taskLoaded.Due.Datetime);
            Assert.AreEqual(referencedDateTime.ToLocalTime().ToString("yyyy-MM-dd"), taskLoaded.Due.String);
            Assert.IsNull(taskLoaded.Due.Timezone);
            Assert.IsNull(taskLoaded.Url);
            Assert.AreEqual(taskCreated.CommentCount, taskLoaded.CommentCount);

            await _sut.DeleteTaskAsync(taskCreated.ID);
        }

        [TestMethod]
        public async Task TestUpdateTaskAllFieldsWithoutLabelsDueDateTime()
        {
            DateTime baseDate = DateTime.Now.AddDays(1);
            DateTime referencedDateTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 9, 0, 0);

            TaskModel taskCreated = await _sut.CreateTaskAsync("Task created by TestUpdateTaskAllFieldsWithoutLabelsDueDateTime");
            var result = await _sut.UpdateTaskAsync(taskCreated.ID, "Task updated by TestUpdateTaskAllFieldsWithoutLabelsDueDateTime", 2178114772, null, TaskModel.PriorityType.p1, referencedDateTime);
            TaskModel taskLoaded = await _sut.GetTaskAsync(taskCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNotNull(taskLoaded);
            Assert.AreEqual(taskCreated.ID, taskLoaded.ID);
            Assert.AreEqual(taskCreated.ProjectID, taskLoaded.ProjectID);
            Assert.AreEqual("Task updated by TestUpdateTaskAllFieldsWithoutLabelsDueDateTime", taskLoaded.Content);
            Assert.AreEqual(taskCreated.Completed, taskLoaded.Completed);
            Assert.IsNull(taskLoaded.LabelIDs);
            Assert.AreEqual(taskCreated.Order, taskLoaded.Order);
            Assert.AreEqual(taskCreated.Indent, taskLoaded.Indent);
            Assert.AreEqual(TaskModel.PriorityType.p1, taskLoaded.Priority);
            Assert.IsNotNull(taskLoaded.Due);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-dd"), taskLoaded.Due.Date);
            Assert.AreEqual(referencedDateTime.ToString("yyyy-MM-ddThh:mm:ssZ"), taskLoaded.Due.Datetime);
            Assert.AreEqual(referencedDateTime.ToLocalTime().ToString("yyyy-MM-dd hh:mm"), taskLoaded.Due.String);
            Assert.AreEqual("Europe/Madrid", taskLoaded.Due.Timezone);
            Assert.IsNull(taskLoaded.Url);
            Assert.AreEqual(taskCreated.CommentCount, taskLoaded.CommentCount);

            await _sut.DeleteTaskAsync(taskCreated.ID);
        }

        [TestMethod]
        public async Task TestCloseTask()
        {
            TaskModel taskCreated = await _sut.CreateTaskAsync("Task created by TestCloseTask");
            var result = await _sut.CloseTaskAsync(taskCreated.ID);
            TaskModel taskLoaded = await _sut.GetTaskAsync(taskCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNull(taskLoaded);
        }

        [TestMethod]
        public async Task TestReopenTask()
        {
            TaskModel taskCreated = await _sut.CreateTaskAsync("Task created by TestReopenTask");
            var resultClose = await _sut.CloseTaskAsync(taskCreated.ID);
            var result = await _sut.ReopenTaskAsync(taskCreated.ID);
            TaskModel taskLoaded = await _sut.GetTaskAsync(taskCreated.ID);

            Assert.IsTrue(result);
            Assert.IsNotNull(taskLoaded);
            Assert.AreEqual("Task created by TestReopenTask", taskLoaded.Content);

            await _sut.DeleteTaskAsync(taskCreated.ID);
        }

        [TestMethod]
        public async Task TestDeleteTask()
        {
            TaskModel task = await _sut.CreateTaskAsync("Task created by TestDeleteTask");
            bool result = await _sut.DeleteTaskAsync(task.ID);

            Assert.IsTrue(result);
        }
    }
}
