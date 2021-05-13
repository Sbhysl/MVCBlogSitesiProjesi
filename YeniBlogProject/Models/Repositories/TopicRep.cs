using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models.Repositories
{
    public class TopicRep
    {
        YeniBlogDbContext ctx; 
        public TopicRep(YeniBlogDbContext context)
        {
            ctx = context;
        }
        public bool AddTopic(Topic newTopic)
        {
            newTopic.IsActive = true;
            newTopic.CreatedDate = DateTime.Now;
            ctx.Topics.Add(newTopic);
            return ctx.SaveChanges() > 0;
        }

        public bool UpdateTopic(Topic topic)
        {
            Topic selected = ctx.Topics.Where(a => a.TopicID == topic.TopicID).FirstOrDefault();
            selected.TopicName = topic.TopicName;
            selected.Description = topic.Description;
            selected.ModifiedDate = DateTime.Now;
            selected.IsActive = topic.IsActive;
            return ctx.SaveChanges() > 0;
        }
        
        public bool DeleteTopic(string name)
        {
            Topic topic = ctx.Topics.Where(a => a.TopicName == name && a.IsActive).FirstOrDefault();
            topic.IsActive = false;
            return ctx.SaveChanges() > 0;
        }
        public List<Topic> GetTopics()
        {
            return ctx.Topics.ToList();
        }

        public Topic GetTopicByTopicName(string name)
        {
            return ctx.Topics.Where(a => a.TopicName == name).FirstOrDefault();
        }

        public List<Topic> GetFirstFiveTopics()
        {
            return ctx.Topics.Where(a => a.IsActive).OrderByDescending(a=>a.CreatedDate).Take(5).ToList();
        }

        public bool IsTopicRegistered(string topicName)
        {
            if (ctx.Topics.Where(a=>a.TopicName==topicName).FirstOrDefault()!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
