using AIN.FAAS.Repository.Models;
using AIN.FAAS.ViewModel.Models.Request;
using AIN.FAAS.ViewModel.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIN.FAAS.Services.IServices
{
    public interface IInventoryAPIServices
    {
        Task<List<HospitalsResponse>> GetHospital();
        Task<SitesResponse> GetSites(string siteId);
        Task<LabsResponse> GetLabs(string labid);
        Task<LocationsResponse> GetLocations(string locationid);
        Task<StoragesResponse> GetStorage(string storageid);
        Task<InventoryResponse> GetInventory(string id);
        Task<List<InventoryResponse>> GetInventoryList(string availablestatus);
        Task<CheckInResponse> InventoryCheckIn(HttpRequest req);
        Task<InventoryItem> InventoryCheckOut(HttpRequest req);
        Task<TransferRequestData> InventoryTransfer(HttpRequest req);
    }
}
