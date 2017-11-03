﻿//
// Copyright © 2017 Ranorex All rights reserved
//

using System;
using System.IO;
using NUnit.Framework;
using Ranorex;
using Ranorex.AutomationHelpers.UserCodeCollections;

namespace RanorexAutomationHelpers.Test
{
    [TestFixture]
    public sealed class WebLibraryTests
    {
        [Test]
        public void WebLibraryTest_NoUri_Fail()
        {
            //Arrange
            string address = null;
            string fileName = null;
            var logger = new TestReportLogger();
            Report.AttachLogger(logger);

            //Act
            WebLibrary.DownloadFile(address, fileName);

            //Assert
            Report.DetachLogger(logger);
            Assert.AreEqual($"Downloading a file from: {address} failed for the following reason:\r\nValue cannot be null.\r\nParameter name: {nameof(address)}", logger.LastLogMessage);
        }

        [Test]
        public void WebLibraryTest_NoLocalPath_Fail()
        {
            //Arrange
            string address = "https://www.ranorex.com/release-notes.html";
            string fileName = null;
            var logger = new TestReportLogger();
            Report.AttachLogger(logger);

            //Act
            WebLibrary.DownloadFile(address, fileName);

            //Assert
            Report.DetachLogger(logger);
            Assert.AreEqual($"Downloading a file from: {address} failed for the following reason:\r\nValue cannot be null.\r\nParameter name: {nameof(fileName)}", logger.LastLogMessage);
        }

        [Test]
        public void WebLibraryTest_DownloadHtmlToCurrentDir_Success()
        {
            //Arrange
            string address = "https://www.ranorex.com/release-notes.html";
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "release-notes.html");
            File.Delete(fileName);
            var logger = new TestReportLogger();
            Report.AttachLogger(logger);

            //Act
            WebLibrary.DownloadFile(address, fileName);

            //Assert
            Report.DetachLogger(logger);
            Assert.AreEqual($"File successfully downloaded to {fileName}", logger.LastLogMessage);
            Assert.IsTrue(File.Exists(fileName));
            File.Delete(fileName);
        }

        [Test]
        public void WebLibraryTest_DownloadExeToSystemDir_Fail()
        {
            //Arrange
            string address = "https://www.ranorex.com/download/Ranorex-7.2.0.exe";
            string fileName = Path.Combine(Environment.SystemDirectory, "Ranorex-7.2.0.exe");
            var logger = new TestReportLogger();
            Report.AttachLogger(logger);

            //Act
            WebLibrary.DownloadFile(address, fileName);

            //Assert
            Report.DetachLogger(logger);
            Assert.AreEqual($"Downloading a file from: {address} failed for the following reason:\r\nAn exception occurred during a WebClient request.\r\nAccess to the path '{fileName}' is denied.", logger.LastLogMessage);
            Assert.IsFalse(File.Exists(fileName));
        }
    }
}
