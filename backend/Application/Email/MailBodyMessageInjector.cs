using backend.Domain;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace backend.Application
{
    public class MailBodyMessageInjector
    {
        protected MailBodyMessagesModel GetBodyMessage(string pathToConfiguration
                                                     , string file
                                                     , Func<List<MailBodyMessagesModel>
                                                        , IEnumerable<MailBodyMessagesModel>> filterMethod)
        {
            string path = Path.Combine(Environment.CurrentDirectory, pathToConfiguration, file);
            List<MailBodyMessagesModel> messages;

            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                messages = JsonConvert.DeserializeObject<List<MailBodyMessagesModel>>(json);
            }

            return ApplyFilter(messages, filterMethod);
        }


        protected MailBodyMessagesModel ApplyFilter(List<MailBodyMessagesModel> messages
                                                  , Func<List<MailBodyMessagesModel>
                                                     , IEnumerable<MailBodyMessagesModel>> filterMethod)
        { 
            if (messages == null || messages.Count == 0)
            {
                throw new Exception("Invalid email reason in security body builder.");
            }
            var filteredMessages = filterMethod.Invoke(messages);
            if (filteredMessages.Count() > 1)
            {
                throw new Exception("Too many matches for the provided email reason in security body builder");
            }
            return filteredMessages.First();
        }


        protected void InjectBodyMessage(MailBodyMessagesModel message, HtmlDocument html)
        {
            HtmlNode titleNode = html.GetElementbyId("Title");
            titleNode.InnerHtml = message.Title;

            HtmlNode middleMessage = html.GetElementbyId("MiddleMessage");
            middleMessage.InnerHtml = message.MiddleMessage;
        }
    }
}
