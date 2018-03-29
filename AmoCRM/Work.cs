using System;
using System.Collections.Generic;
using System.Linq;
using AmoCRM.Classes;
using AmoCRM.Models;
using System.Reflection;
using Newtonsoft.Json;

namespace AmoCRM
{
    public class Work
    {
        public void GetFromAmoCRMSendTo1C(String hostAmoCRM, String ClientId, String ClientSecret, String host1CGet,String host1CPost)
        {
            var getDataFromAmoCRM = new GetDataFromAmoCRM(hostAmoCRM, ClientId, ClientSecret);

            var mm = Provider.SendGetResponse(host1CGet + "GetLastDateSuccess");
            Log.WriteError("11" + mm);

            var users = getDataFromAmoCRM.GetUsers();
            var json = JsonConvert.SerializeObject(users);
            Provider.SendPOSTResponse(host1CPost + "UpdateUsers", json);
            //Provider.GetUrlOfData(host1CGet + "UpdateUser", users[0]);
            //var noteTypes = getDataFromAmoCRM.GetNoteTypes();
            //var taskTypes = getDataFromAmoCRM.GetTaskTypes();

            //foreach (var item in noteTypes)
            //{
            //    var url = Provider.GetUrlOfData(host1C + "UpdateNoteType", item);
            //    Provider.SendGetResponse(url);
            //}

            //foreach (var item in taskTypes)
            //{
            //    var url = Provider.GetUrlOfData(host1C + "UpdateTaskType", item);
            //    Provider.SendGetResponse(url);
            //}

            var emails = getDataFromAmoCRM.GetEmails();
            SendEmails(emails, host1CPost);

            var calls = getDataFromAmoCRM.GetCalls();
            SendCalls(calls,host1CPost);

            //var leads = getDataFromAmoCRM.GetLeads();

            //var pipelines = getDataFromAmoCRM.GetPipelines(leads);
            //foreach (var item in leads)
            //{
            //    Log.WriteInfo(leads.IndexOf(item).ToString());

            //    var dataFor1C = new DataFor1C();
            //    dataFor1C.pipelineName = pipelines.Where(pipeline => pipeline.id == item.pipeline_id).First().name;

            //    var company = new List<ContactResponse>();
            //    if (item.linked_company_id != "" && item.linked_company_id != "0")
            //    {
            //        company = getDataFromAmoCRM.GetContacts(false, item.linked_company_id);
            //    }
            //    else
            //    {
            //        company = getDataFromAmoCRM.GetContacts(item.main_contact_id, "");
            //    }

            //    if (company != null && company.Count() > 0)
            //    {
            //        dataFor1C.nameContact = company[0].name;
            //        dataFor1C.codeContact = company[0].id.ToString();
            //        foreach (var custom_field in company[0].custom_fields)
            //        {
            //            switch (custom_field.name)
            //            {
            //                case "Реквизиты компании":
            //                    dataFor1C.requisites = custom_field.values[0].value; break;
            //                case "Адрес":
            //                    dataFor1C.address = custom_field.values[0].value; break;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        Log.WriteInfo("not found contact for " + item.name);
            //    }

            //    var contacts1 = getDataFromAmoCRM.GetContacts(item.main_contact_id, "");
            //    SetContactsData(dataFor1C, contacts1, "1");


            //    var leadsAndContacts = getDataFromAmoCRM.GetLeadsAndContacts(item.id);
            //    foreach (var itemLinks in leadsAndContacts)
            //    {
            //        if (itemLinks.contact_id == item.main_contact_id.ToString())
            //        {
            //            continue;
            //        }

            //        var contacts2 = getDataFromAmoCRM.GetContacts(itemLinks.contact_id, "");
            //        SetContactsData(dataFor1C, contacts2, "2");
            //    }

            //    var url = Provider.GetUrlOfData(host1CGet + "UpdateCustomer", dataFor1C);
            //    Provider.SendGetResponse(url);
            //}
        }

        private void SendCalls(List<Call> calls,String host1CPost)
        {
            var dataToSend = new List<Call>();
            string json;
            foreach (var item in calls)
            {
                dataToSend.Add(item);

                if (calls.IndexOf(item) != 0 && calls.IndexOf(item) % 300 == 0)
                {
                    json = JsonConvert.SerializeObject(dataToSend);
                    Provider.SendPOSTResponse(host1CPost + "UpdateCalls", json);
                    dataToSend.Clear();

                    Log.WriteInfo("call " + calls.IndexOf(item).ToString() + " of " + calls.Count()
                    + " in " + DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss"));
                }
            }

            json = JsonConvert.SerializeObject(dataToSend);
            Provider.SendPOSTResponse(host1CPost + "UpdateCalls", json);
            Log.WriteInfo("call " + calls.Count().ToString() + " of " + calls.Count()
                   + " in " + DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss"));
        }

        private void SendEmails(List<Email> emails, String host1CPost)
        {
            var dataToSend = new List<Email>();
            string json;
            foreach (var item in emails)
            {
                dataToSend.Add(item);

                if (emails.IndexOf(item)!=0 && emails.IndexOf(item) % 300 == 0)
                {
                    json = JsonConvert.SerializeObject(dataToSend);
                    Provider.SendPOSTResponse(host1CPost + "UpdateEmails", json);
                    dataToSend.Clear();

                    Log.WriteInfo("email " + emails.IndexOf(item).ToString() + " of " + emails.Count()
                    + " in " + DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss"));
                }
            }

            json = JsonConvert.SerializeObject(dataToSend);
            Provider.SendPOSTResponse(host1CPost + "UpdateEmails", json);
            Log.WriteInfo("email " + emails.Count().ToString() + " of " + emails.Count()
                   + " in " + DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss"));
        }

        private void SetContactsData(DataFor1C dataFor1C, List<ContactResponse> contacts, string numer)
        {
            if (contacts == null)
            {
                return;
            }

            var phones = "";
            SetFieldValue(dataFor1C, "nameContact" + numer, contacts[0].name);
            SetFieldValue(dataFor1C, "codeContact" + numer, contacts[0].id.ToString());

            foreach (var custom_field in contacts[0].custom_fields)
            {
                switch (custom_field.name)
                {
                    case "Телефон":
                        foreach (var phone in custom_field.values)
                        {
                            if (phone.value == "")
                            {
                                continue;
                            }

                            if (phones == "")
                            {
                                phones += phone.value;
                            }
                            else
                            {
                                phones += "|" + phone.value;
                            }
                        }
                        SetFieldValue(dataFor1C, "phonesContact" + numer, phones);
                        break;
                    case "Email":
                        SetFieldValue(dataFor1C, "emailContact" + numer, custom_field.values[0].value); break;
                }
            }
        }

        private void SetFieldValue(DataFor1C dataFor1C, string fieldName, string value)
        {
            var property = dataFor1C.GetType().GetProperty(fieldName);
            property.SetValue(dataFor1C, value);
        }

        private void SetFieldFromCustomField(DataFor1C dataFor1C, LeadResponse item)
        {

            //foreach (var custom_field in item.custom_fields)
            //{
            //	switch (custom_field.name)
            //	{
            //		case "Адрес объекта": dataFor1C.address = custom_field.values[0].value; break;

            //	}
            //}
        }
    }
}
