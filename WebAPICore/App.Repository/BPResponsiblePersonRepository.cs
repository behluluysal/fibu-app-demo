using App.Repository.ApiClient;
using Core.AutoMapperDtos;
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
    public class BPResponsiblePersonRepository : IBPResponsiblePersonRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public BPResponsiblePersonRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<BPResponsiblePersonDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/bpresponsiblepeople?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.bprp);
        }

        public async Task<BPResponsiblePersonDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/bpresponsiblepeople/{id}");
            BPResponsiblePersonDto responsiblePerson = JsonSerializer.Deserialize<BPResponsiblePersonDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePerson;
        }

        public async Task<BPResponsiblePersonDto> CreateAsync(BPResponsiblePersonDto responsiblePerson)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/bpresponsiblepeople", responsiblePerson);
            BPResponsiblePersonDto responsiblePersonDto = JsonSerializer.Deserialize<BPResponsiblePersonDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePersonDto;
        }

        public async Task UpdateAsync(int id, BPResponsiblePersonDto responsiblePerson)
        {
            await _webApiExecuter.InvokePut($"api/bpresponsiblepeople/{id}", responsiblePerson);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/bpresponsiblepeople/{id}");
        }

        #endregion


        #region BPResponsiblePerson PhoneNumber Methods

        public async Task<BPResponsiblePersonWithPhoneNumberDto> GetByIdWithGsmAsync(int id, QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/bpresponsiblepeople/{id}/phonenumbers?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            BPResponsiblePersonWithPhoneNumberDto responsiblePerson = JsonSerializer.Deserialize<BPResponsiblePersonWithPhoneNumberDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePerson;
        }

        public async Task<BPRPPhoneNumberDto> CreateGsmAsync(BPRPPhoneNumberDto gsm)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>($"api/bpresponsiblepeople/{gsm.ResponsiblePersonId}/phonenumbers", gsm);
            BPRPPhoneNumberDto responsiblePersonDto = JsonSerializer.Deserialize<BPRPPhoneNumberDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePersonDto;
        }

        public async Task UpdateGsmAsync(int phoneNumberId, BPRPPhoneNumberDto gsm)
        {
            await _webApiExecuter.InvokePut($"api/bpresponsiblepeople/{gsm.ResponsiblePersonId}/phonenumbers/{phoneNumberId}", gsm);
        }
        public async Task DeleteGsmAsync(int BPResponsiblePersonId, int GsmId)
        {
            await _webApiExecuter.InvokeDelete($"api/bpresponsiblepeople/{BPResponsiblePersonId}/phonenumbers/{GsmId}");
        }

        #endregion

        #region BPResponsiblePerson Email Methods

        public async Task<BPResponsiblePersonWithEmailDto> GetByIdWithEmailAsync(int id, QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/bpresponsiblepeople/{id}/emails?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            BPResponsiblePersonWithEmailDto responsiblePerson = JsonSerializer.Deserialize<BPResponsiblePersonWithEmailDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePerson;
        }
        public async Task<BPRPEmailDto> CreateEmailAsync(BPRPEmailDto email)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>($"api/bpresponsiblepeople/{email.ResponsiblePersonId}/emails", email);
            BPRPEmailDto responsiblePersonDto = JsonSerializer.Deserialize<BPRPEmailDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return responsiblePersonDto;
        }

        public async Task UpdateEmailAsync(int emailId, BPRPEmailDto email)
        {
            await _webApiExecuter.InvokePut($"api/bpresponsiblepeople/{email.ResponsiblePersonId}/emails/{emailId}", email);
        }

        public async Task DeleteEmailAsync(int BPResponsiblePersonId, int EmailId)
        {
            await _webApiExecuter.InvokeDelete($"api/bpresponsiblepeople/{BPResponsiblePersonId}/emails/{EmailId}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("bprp")]
            public IEnumerable<BPResponsiblePersonDto> bprp { get; set; }
        }
    }
}
