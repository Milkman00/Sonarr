﻿using System;
using System.IO;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NzbDrone.Common;
using NzbDrone.Test.Common;
using NzbDrone.Update.UpdateEngine;

namespace NzbDrone.Update.Test
{
    [TestFixture]
    public class StartNzbDroneServiceFixture : TestBase<StartNzbDrone>
    {
        [Test]
        public void should_start_service_if_app_type_was_serivce()
        {
            string targetFolder = "c:\\NzbDrone\\";

            Subject.Start(AppType.Service, targetFolder);

            Mocker.GetMock<Common.IServiceProvider>().Verify(c => c.Start(ServiceProvider.NZBDRONE_SERVICE_NAME), Times.Once());
        }


        [Test]
        public void should_start_console_if_app_type_was_serivce_but_start_failed_because_of_permissions()
        {
            string targetFolder = "c:\\NzbDrone\\";

            Mocker.GetMock<Common.IServiceProvider>().Setup(c => c.Start(ServiceProvider.NZBDRONE_SERVICE_NAME)).Throws(new InvalidOperationException());

            Subject.Start(AppType.Service, targetFolder);

            Mocker.GetMock<IProcessProvider>().Verify(c => c.Start("c:\\NzbDrone\\NzbDrone.Console.exe"), Times.Once());

            ExceptionVerification.ExpectedWarns(1);
        }
    }
}