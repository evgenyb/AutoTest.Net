using System.Threading;
using AutoTest.Core.FileSystem;
using AutoTest.Core.Messaging;
using Rhino.Mocks;
using NUnit.Framework;
using System;
using System.IO;

namespace AutoTest.Test.Core
{
    [TestFixture]
    public class DirectoryWatcherTests
    {
        private string _file;
        private IMessageBus _messageBus;
        private IWatchValidator _validator;
        private DirectoryWatcher _watcher;

        [SetUp]
        public void SetUp()
        {
            _messageBus = MockRepository.GenerateMock<IMessageBus>();
            _validator = MockRepository.GenerateMock<IWatchValidator>();
            _watcher = new DirectoryWatcher(_messageBus, _validator);
            _file = Path.GetFullPath("watcher_test.txt");
            _watcher.Watch(Path.GetDirectoryName(_file));
        }

        [TearDown]
        public void TearDown()
        {
            _watcher.Dispose();
            File.Delete(_file);
        }

        [Test]
        public void Should_not_start_watch_when_folder_is_invalid()
        {
            var bus = MockRepository.GenerateMock<IMessageBus>();
            var watcher = new DirectoryWatcher(bus, null);
            watcher.Watch("");
            bus.AssertWasNotCalled(m => m.Publish<InformationMessage>(null), m => m.IgnoreArguments());
        }

        [Test]
        public void Should_send_message_when_file_changes_once()
        {
            _validator.Stub(v => v.ShouldPublish(null)).IgnoreArguments().Return(true);
            // Write twice
            File.Create(_file).Dispose();
            using (var writer = new StreamWriter(_file, true)) { writer.WriteLine("some text"); }
			// Set this to 1 sec since on linux in worst case scenario mono will poll every
			// 750 millisecond
            Thread.Sleep(1000);
            
            _messageBus.AssertWasCalled(
                m => m.Publish<FileChangeMessage>(
                         Arg<FileChangeMessage>.Matches(
                             f => f.Files.Length >  0 &&
                                  f.Files[0].Extension.Equals(Path.GetExtension(_file)) &&
                                  f.Files[0].FullName.Equals(_file) &&
                                  f.Files[0].Name.Equals(Path.GetFileName(_file)))),
                m => m.Repeat.Once());
        }
        
        [Test]
        public void Should_not_publish_event_when_validator_invalidates_change()
        {
            _validator.Stub(v => v.ShouldPublish(null)).IgnoreArguments().Return(false);
            File.Create(_file).Dispose();
            Thread.Sleep(100);
            _messageBus.AssertWasNotCalled(m => m.Publish<FileChangeMessage>(null), m => m.IgnoreArguments());
        }
    }
}