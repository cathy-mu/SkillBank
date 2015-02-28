using System;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Assert = NUnit.Framework.Assert;
//using NUnit.Mocks;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class MessageRepositoryTests
    {
        private int _memberAId;
        private int _memberBId;
        private int invalidMemberAId;
        private int invalidMemberBId;
        MessageRepository _repository;
            

        [TestInitialize]
        public void TestInitialize()
        {
            _memberAId = 1;
            _memberBId = 2;
            invalidMemberAId = 99999;
            invalidMemberBId = 99998;
           _repository = new MessageRepository();
        }

        //[TestMethod]
        //public void Should_SaveMessage()
        //{
        //    MessageRepository repository = new MessageRepository();
        //    Message message = new Message();
        //    message.MessageId = 0;
        //    message.From_Id = _memberAId;
        //    message.To_Id = _memberBId;
        //    message.MessageText = "Testing message saving funtion in Unit test";
           
        //    int result = repository.AddMessage(message);

        //    Assert.IsNotNull(result);
            
        //}

        //[TestMethod]
        //public void Should_SaveMessageCorrectly()
        //{
        //    MessageRepository repository = new MessageRepository();
        //    Message message = new Message();
        //    message.From_Id = _memberAId;
        //    message.To_Id = _memberBId;

           　
        //    //TO DO:ｒｅａｄｏｍ　ｉｄ　ｉｎ　ｍｅｓｓａｇｅ
        //    message.MessageText = "Testing message saving funtion in Unit test";
        //    int result = repository.AddMessage(message);
        //    var messagesSaved = repository.GetMessagesByFromToId(message.From_Id, message.To_Id);


        //    Message messageSaved = messagesSaved[0];

        //    Assert.AreEqual(messageSaved.From_Id,message.From_Id);
        //    Assert.AreEqual(messageSaved.To_Id, message.To_Id);
        //    Assert.AreEqual(messageSaved.MessageText, message.MessageText);
        //}

        //[TestMethod]
        //public void Should_UpdateIsLatesTag_WhenSaveMessage()
        //{
        //    MessageRepository repository = new MessageRepository();
        //    Message message = new Message();
        //    message.From_Id = _memberAId;
        //    message.To_Id = _memberBId;
        //    message.MessageText = "Testing message saving funtion in Unit test";
        //    int result = repository.AddMessage(message);

        //    List<Message> messages = repository.GetMessagesByFromToId(_memberAId, _memberBId);
        //    if (messages.Count > 0)
        //    {
        //        Boolean isLatest = messages.Take(1).Where(m => m.IsLatest).Any();
        //        Assert.IsTrue(isLatest);
        //        Assert.AreEqual(messages.Count(m => m.IsLatest), 1);
        //    }
        //}
        
        [TestMethod]
        public void Should_GetLatestMessage_ByMemberId()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            List<MessageListItem> messages = repository.GetLastestMessagesByMemberId(_memberAId, loadBy);

            Assert.IsNotNull(messages);
            messages.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetLatestMessageCorrectly_ByMemberId()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            List<MessageListItem> messages = repository.GetLastestMessagesByMemberId(_memberAId, loadBy);
            foreach (MessageListItem messageItem in messages)
            {
                Assert.IsNotNull(messageItem.MessageText);
                Boolean isFrom = (messageItem.From_Id == _memberAId );
                Boolean isTo = (messageItem.To_Id == _memberAId );
                Assert.IsTrue(isFrom || isTo);
            }
        }

        [TestMethod]
        public void ShouldNot_GetLatestMessage_ByInvalidMemberId()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            var result = repository.GetLastestMessagesByMemberId(invalidMemberAId, loadBy);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Should_GetMessages_ByFromAndToId()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            List<Message> messages = repository.GetMessagesByFromToId(_memberAId, _memberBId, loadBy);

            Assert.IsNotNull(messages);
            messages.Count.AssertIsGreaterThan(0);
        }
        
        [TestMethod]
        public void Should_GetMessagesCorrectly_ByFromAndToId()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            List<Message> messagesA = repository.GetMessagesByFromToId(_memberAId, _memberBId, loadBy);
            List<Message> messagesB = repository.GetMessagesByFromToId(_memberBId, _memberAId, loadBy);

            Assert.IsNotNull(messagesA);
            Assert.IsNotNull(messagesB);
            int messagesANo = messagesA.Count;
            int messagesBNo = messagesB.Count;

            if (messagesANo > messagesBNo)
            {
                messagesA.RemoveRange(messagesBNo, messagesANo - messagesBNo);
            }
            else if (messagesBNo > messagesANo)
            {
                messagesB.RemoveRange(messagesANo, messagesBNo - messagesANo);
            }
            for (int i = 0; i < messagesA.Count; i++)
            {
                Assert.AreEqual(messagesA[i], messagesB[i]);
            }
            
        }

        [TestMethod]
        public void ShouldNot_GetMessages_ByInvalidFromAndToMemberId()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            List<Message> messages = repository.GetMessagesByFromToId(invalidMemberAId, invalidMemberBId, loadBy);

            Assert.IsNull(messages);
        }

        [TestMethod]
        public void Should_GetOneLatestMessagesWithOtherMembers_ByMemberId()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            List<MessageListItem> messages = repository.GetLastestMessagesByMemberId(_memberAId, loadBy);
            int messageNo = messages.Count;
            //messages.Distinct(m => m.From_Id);
            var _memberIds = messages.Select(m => m.From_Id).Union(messages.Select(m => m.To_Id)).Distinct().Count()-1;
            //IEnumerable<int> fromIds = messages.Select(m => m.From_Id);
            //IEnumerable<int> toIds = messages.Select(m => m.To_Id);
            //var _memberNo = fromIds.Union<int>(toIds).Distinct();
            Assert.AreEqual(messageNo,_memberIds);
        }

        [TestMethod]
        public void ShouldNot_GetCorrectMessages_ByFromAndToMemberId()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            List<Message> messages = repository.GetMessagesByFromToId(_memberAId, _memberBId, loadBy);
            foreach (Message messageItem in messages)
            {
                Assert.IsNotNull(messageItem.MessageText);
                Boolean fromMember = (_memberAId == messageItem.From_Id) && messageItem.Show4From;
                Boolean toMember = (_memberAId == messageItem.To_Id) && messageItem.Show4To;
                Assert.IsTrue(fromMember || toMember);
            }
        }

        [TestMethod]
        public void ShouldNot_GetMessages_ByFromAndToMemberId_InCorrectOrder()
        {
            MessageRepository repository = new MessageRepository();
            Byte loadBy = 1;
            List<Message> messages = repository.GetMessagesByFromToId(_memberAId, _memberBId, loadBy);
            List<Message> reorderMessages = messages.OrderByDescending(m => m.CreatedDate).ToList();
            for (int i = 0; i < messages.Count; i++)
            {
                Assert.AreEqual(messages[i], reorderMessages[i]);
            }
        }

        [TestMethod]
        public void ShouldNot_GetUnReadMessageNum_ByMemberId()
        {
            List<MessageUnReadItem> unReadNum = _repository.GetMessageUnReadNum(_memberAId);

            for (int i = 0; i < unReadNum.Count; i++)
            {
                ((int)unReadNum[i].Num).AssertIsGreaterThan(0);;
                unReadNum[i].From_Id.AssertIsGreaterThan(0); ;
            }
        }
        
    }
}
