using App.Repository.ApiClient;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Repository
{
    public class ResponsiblePersonRepository : IResponsiblePersonRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public ResponsiblePersonRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<ResponsiblePersonDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/responsiblepeople?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.rp);
        }

        public async Task<ResponsiblePersonDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/responsiblepeople/{id}");
            ResponsiblePersonDto responsiblePerson = JsonSerializer.Deserialize<ResponsiblePersonDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePerson;
        }

        public async Task<ResponsiblePersonDto> CreateAsync(ResponsiblePersonDto responsiblePerson)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/responsiblepeople", responsiblePerson);
            ResponsiblePersonDto responsiblePersonDto = JsonSerializer.Deserialize<ResponsiblePersonDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePersonDto;
        }

        public async Task UpdateAsync(int id, ResponsiblePersonDto responsiblePerson)
        {
            await _webApiExecuter.InvokePut($"api/responsiblepeople/{id}", responsiblePerson);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/responsiblepeople/{id}");
        }

        #endregion

        #region ResponsiblePerson Contact Methods

        public async Task<(int, IEnumerable<ResponsiblePersonWithContactDto>)> GetWithContactAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/responsiblepeople/contact?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseWithContactIndex parsedResult = JsonSerializer.Deserialize<ApiResponseWithContactIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.rpcontact);
        }

        public async Task<ResponsiblePersonWithContactDto> GetByIdWithContactAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/responsiblepeople/{id}/contact");
            ResponsiblePersonWithContactDto responsiblePerson = JsonSerializer.Deserialize<ResponsiblePersonWithContactDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePerson;
        }

        #endregion

        #region ResponsiblePerson PhoneNumber Methods

        public async Task<(int, ResponsiblePersonWithPhoneNumberDto)> GetByIdWithGsmAsync(int id, QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/responsiblepeople/{id}/phonenumbers?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseWithPhoneNumbersIndex parsedResult = JsonSerializer.Deserialize<ApiResponseWithPhoneNumbersIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.rpphonenumbers);
        }

        public async Task<SCRPPhoneNumberDto> CreateGsmAsync(SCRPPhoneNumberDto gsm)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>($"api/responsiblepeople/{gsm.ResponsiblePersonId}/phonenumbers", gsm);
            SCRPPhoneNumberDto responsiblePersonDto = JsonSerializer.Deserialize<SCRPPhoneNumberDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePersonDto;
        }

        public async Task UpdateGsmAsync(int phoneNumberId, SCRPPhoneNumberDto gsm)
        {
            await _webApiExecuter.InvokePut($"api/responsiblepeople/{gsm.ResponsiblePersonId}/phonenumbers/{phoneNumberId}", gsm);
        }
        public async Task DeleteGsmAsync(int ResponsiblePersonId, int GsmId)
        {
            await _webApiExecuter.InvokeDelete($"api/responsiblepeople/{ResponsiblePersonId}/phonenumbers/{GsmId}");
        }

        #endregion

        #region ResponsiblePerson Email Methods

        public async Task<(int, ResponsiblePersonWithEmailDto)> GetByIdWithEmailAsync(int id, QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/responsiblepeople/{id}/emails?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseWithEmailsIndex parsedResult = JsonSerializer.Deserialize<ApiResponseWithEmailsIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.rpemails);
        }
        public async Task<SCRPEmailDto> CreateEmailAsync(SCRPEmailDto email)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>($"api/responsiblepeople/{email.ResponsiblePersonId}/emails", email);
            SCRPEmailDto responsiblePersonDto = JsonSerializer.Deserialize<SCRPEmailDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePersonDto;
        }

        public async Task UpdateEmailAsync(int emailId, SCRPEmailDto email)
        {
            await _webApiExecuter.InvokePut($"api/responsiblepeople/{email.ResponsiblePersonId}/emails/{emailId}", email);
        }

        public async Task DeleteEmailAsync(int ResponsiblePersonId, int EmailId)
        {
            await _webApiExecuter.InvokeDelete($"api/responsiblepeople/{ResponsiblePersonId}/emails/{EmailId}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("rp")]
            public IEnumerable<ResponsiblePersonDto> rp { get; set; }
        }

        public class ApiResponseWithContactIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("rpcontact")]
            public IEnumerable<ResponsiblePersonWithContactDto> rpcontact { get; set; }
        }
        public class ApiResponseWithPhoneNumbersIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("rpphonenumbers")]
            public ResponsiblePersonWithPhoneNumberDto rpphonenumbers { get; set; }
        }
        public class ApiResponseWithEmailsIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("rpemails")]
            public ResponsiblePersonWithEmailDto rpemails { get; set; }
        }
    }
}
