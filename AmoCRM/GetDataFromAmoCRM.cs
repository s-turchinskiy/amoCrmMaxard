using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AmoCRM.Models;
using AmoCRM.Classes;
using Newtonsoft.Json;

namespace AmoCRM
{
    public class GetDataFromAmoCRM
    {
        private readonly CookieContainer CookieContainerToAmoCRM;
        private readonly String HostAmoCRM;

        public GetDataFromAmoCRM(String hostAmoCRM, String ClientId, String ClientSecret)
        {
            var url = hostAmoCRM + "/private/api/auth.php?type=json&USER_LOGIN=" + ClientId + "&USER_HASH=" + ClientSecret;
            CookieContainerToAmoCRM = Provider.GetCookieContainer(url);
            HostAmoCRM = hostAmoCRM;
        }

        public List<UserResponse> GetUsers()
        {
            var accountsResponseJson = Provider.SendGetResponse(HostAmoCRM + "/private/api/v2/json/accounts/current", CookieContainerToAmoCRM);
            var accountsResponse = JsonConvert.DeserializeObject<AccountResponseCurrentRoot>(accountsResponseJson);
            return accountsResponse.response.account.users;
        }

        public List<Call> GetCalls()
        {
            var answerAmo = new List<CallNote>();
            var limit_offset = 1;
            while (true)
            {

                var notesResponseJson = Provider.SendGetResponse(HostAmoCRM +
                        "/api/v2/notes?type=contact&note_type=10&limit_rows=500&limit_offset=" + limit_offset.ToString(), CookieContainerToAmoCRM);
                var notesResponse = JsonConvert.DeserializeObject<CallsNoteResponceRoot>(notesResponseJson);
                if (notesResponse == null)
                {
                    break;
                }

                foreach (var item in notesResponse._embedded.items)
                {
                    answerAmo.Add(item);
                }
                limit_offset += 500;
            }

            //повторятеся. вынести
            limit_offset = 1;
            while (true)
            {

                var notesResponseJson = Provider.SendGetResponse(HostAmoCRM +
                        "/api/v2/notes?type=contact&note_type=11&limit_rows=500&limit_offset=" + limit_offset.ToString(), CookieContainerToAmoCRM);
                var notesResponse = JsonConvert.DeserializeObject<CallsNoteResponceRoot>(notesResponseJson);
                if (notesResponse == null)
                {
                    break;
                }

                foreach (var item in notesResponse._embedded.items)
                {
                    answerAmo.Add(item);
                }
                limit_offset += 500;
            }

            var answer = (from item in answerAmo
                          where item.@params != null
                          orderby item.id
                          select new Call
                          {
                              created_at = item.created_at,
                              element_id = item.element_id,
                              id = item.id,
                              income = item.note_type == 10,
                              responsible_user_id = item.responsible_user_id,
                              Phone = item.@params.PHONE,
                              Duration = item.@params.DURATION,
                          }).ToList();
            //answer = answer.Where(item => item.note_type!=2).ToList();
            return answer;
        }

        public List<Email> GetEmails()
        {
            var answerAmo = new List<EmailNote>();
            var limit_offset = 1;
            while (true)
            {

                var notesResponseJson = Provider.SendGetResponse(HostAmoCRM +
                        "/api/v2/notes?type=contact&note_type=15&limit_rows=500&limit_offset=" + limit_offset.ToString(), CookieContainerToAmoCRM);
                var notesResponse = JsonConvert.DeserializeObject<EmailsNoteResponceRoot>(notesResponseJson);
                if (notesResponse == null)
                {
                    break;
                }

                foreach (var item in notesResponse._embedded.items)
                {
                    answerAmo.Add(item);
                }
                limit_offset += 500;
            }

            var answer = (from item in answerAmo
                          where item.@params != null
                          select new Email
                          {
                              created_at = item.created_at,
                              element_id = item.element_id,
                              id = item.id,
                              responsible_user_id = item.responsible_user_id,
                              FromEmail = item.@params.@from.email,
                              ToEmail = item.@params.to.email,
                              income = item.@params.income,
                          }).ToList();
            //answer = answer.Where(item => item.note_type!=2).ToList();
            return answer;
        }

        public List<taskType> GetTaskTypes()
        {
            var taskTypesResponseJson = Provider.SendGetResponse(HostAmoCRM +
                    "/api/v2/account?with=task_types", CookieContainerToAmoCRM);
            var obj = Newtonsoft.Json.Linq.JObject.Parse(taskTypesResponseJson)["_embedded"]["task_types"]
                .Children().Select(item => item.First()).ToList();
            var taskTypes = obj.Select(item => new taskType
            {
                id = (int)item["id"],
                name = (string)item["name"],
            }).ToList();

            return taskTypes;
        }

        public List<noteType> GetNoteTypes()
        {
            var noteTypesResponseJson = Provider.SendGetResponse(HostAmoCRM +
                    "/api/v2/account?with=note_types", CookieContainerToAmoCRM);
            var obj = Newtonsoft.Json.Linq.JObject.Parse(noteTypesResponseJson)["_embedded"]["note_types"]
                .Children().Select(item => item.First()).ToList();
            var noteTypes = obj.Select(item => new noteType
            {
                id = (int)item["id"],
                code = (string)item["code"],
            }).ToList();

            return noteTypes;
        }

        public List<Pipeline> GetPipelines(List<LeadResponse> leads)
        {
            var pipelinesResponseJson = Provider.SendGetResponse(HostAmoCRM +
                    "/private/api/v2/json/pipelines/list", CookieContainerToAmoCRM);
            var obj = Newtonsoft.Json.Linq.JObject.Parse(pipelinesResponseJson)["response"]["pipelines"]
                .Children().Select(item => item.First()).ToList();
            var pipelines = obj.Select(item => new Pipeline
            {
                id = (int)item["id"],
                name = (string)item["name"],
                is_main = (bool)item["is_main"],
                label = (string)item["label"],
                sort = (int)item["sort"],
                value = (int)item["value"],
            }).ToList();

            return pipelines;
        }

        public List<Link> GetLeadsAndContacts(string leadNumber)
        {
            var leadsAndContactsResponseJson = Provider.SendGetResponse(HostAmoCRM + "/private/api/v2/json/contacts/links?deals_link=" + leadNumber, CookieContainerToAmoCRM);
            var leadsAndContactsResponse = JsonConvert.DeserializeObject<LeadsAndContactsRoot>(leadsAndContactsResponseJson);
            if (leadsAndContactsResponse == null)
            {
                return new List<Link>();
            }

            return leadsAndContactsResponse.response.links;
        }

        public List<LeadResponse> GetLeads()
        {
            var leadRequest = new LeadsRequestRoot();
            leadRequest.SetRequest();


            string leadRequestJson = JsonConvert.SerializeObject(leadRequest);
            var leadResponseJson = Provider.SendPOSTResponse(HostAmoCRM + "/private/api/v2/json/leads/list?status=142", leadRequestJson, CookieContainerToAmoCRM);
            var leadResponse = JsonConvert.DeserializeObject<LeadResponseRoot>(leadResponseJson);
            return leadResponse.response.leads;
        }

        public List<ContactResponse> GetContacts(object contact_id, string linked_company_id)
        {
            var contactIsComplete = true;
            try
            {
                contactIsComplete = (bool)contact_id;
            }
            catch
            {

            }

            var companyIsComplete = !(linked_company_id == "" || linked_company_id == "0");


            if (!companyIsComplete && !contactIsComplete)
            {
                return null;
            }

            var contactsResponseJson = "";
            if (contactIsComplete)
            {
                contactsResponseJson = Provider.SendGetResponse(HostAmoCRM + "/private/api/v2/json/contacts/list?id=" + contact_id, CookieContainerToAmoCRM);
            }
            else
            {
                contactsResponseJson = Provider.SendGetResponse(HostAmoCRM + "/private/api/v2/json/company/list?id=" + linked_company_id, CookieContainerToAmoCRM);
            }


            var contactsResponse = JsonConvert.DeserializeObject<ContactResponseRoot>(contactsResponseJson);

            if (contactsResponse == null)
            {
                return null;
            }

            return contactsResponse.response.contacts;
        }

    }
}
