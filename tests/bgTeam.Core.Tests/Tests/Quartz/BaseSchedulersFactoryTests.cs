using bgTeam.Impl.Quartz;
using bgTeam.Quartz;
using Moq;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Quartz
{
    public class BaseSchedulersFactoryTests
    {
        [Fact]
        public void CreateJob()
        {
            var factory = new Mock<ISchedulerFactory>();
            var scheduler = new Mock<IScheduler>();
            factory.Setup(x => x.GetScheduler(It.IsAny<CancellationToken>()))
                .ReturnsAsync(scheduler.Object);
            var schedulerFactory = new Factory(null, factory.Object);
            schedulerFactory.Create<Job>(new JobTriggerInfo { Name = "Job1", DateStart = "* 0 0 ? * * *" });
            scheduler.Verify(x => x.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>(), It.IsAny<CancellationToken>()));
            scheduler.Verify(x => x.Start(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void Dispose()
        {
            var factory = new Mock<ISchedulerFactory>();
            var scheduler = new Mock<IScheduler>();

            factory.Setup(x => x.GetScheduler(It.IsAny<CancellationToken>()))
                .ReturnsAsync(scheduler.Object);
            factory.Setup(x => x.GetAllSchedulers(It.IsAny<CancellationToken>()))
                .ReturnsAsync((new List<IScheduler> { scheduler.Object }).AsReadOnly());

            var schedulerFactory = new Factory(null, factory.Object);
            schedulerFactory.Create<Job>(new JobTriggerInfo { Name = "Job1", DateStart = "* 0 0 ? * * *" });

            schedulerFactory.Dispose();

            scheduler.Verify(x => x.Shutdown(false, It.IsAny<CancellationToken>()));
        }
    }

    class Factory : BaseSchedulersFactory
    {
        public Factory(IServiceProvider container, ISchedulerFactory schedulerFactory)
            : base(container, schedulerFactory)
        {
        }

        protected override IDictionary<string, object> CreateCommonMap(IJobTriggerInfo config)
        {
            return new Dictionary<string, object>();
        }
    }

    class JobTriggerInfo : IJobTriggerInfo
    {
        public string Name { get; set; }
        public string DateStart { get; set; }
    }

    class Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
