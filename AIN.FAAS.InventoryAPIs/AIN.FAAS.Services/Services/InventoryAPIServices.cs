using AIN.FAAS.Repository.IRepository;
using AIN.FAAS.Repository.Models;
using AIN.FAAS.Services.IServices;
using AIN.FAAS.ViewModel.Models.Request;
using AIN.FAAS.ViewModel.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIN.FAAS.Services.Services
{
    public class InventoryAPIServices : IInventoryAPIServices
    {
        IInventoryAPIRepository inventoryAPIRepository;
        public InventoryAPIServices(IInventoryAPIRepository _inventoryAPIRepository)
        {
            inventoryAPIRepository = _inventoryAPIRepository;
        }
        public async Task<List<HospitalsResponse>> GetHospital()
        {
            return await inventoryAPIRepository.GetHospital();
        }
        public async Task<SitesResponse> GetSites(string siteId)
        {
            return await inventoryAPIRepository.GetSites(siteId);
        }
        public async Task<LabsResponse> GetLabs(string labid)
        {
            return await inventoryAPIRepository.GetLabs(labid);
        }
        public async Task<LocationsResponse> GetLocations(string locationid)
        {
            return await inventoryAPIRepository.GetLocations(locationid);
        }
        public async Task<StoragesResponse> GetStorage(string storageid)
        {
            return await inventoryAPIRepository.GetStorage(storageid);
        }
        public async Task<InventoryResponse> GetInventory(string id)
        {
            return await inventoryAPIRepository.GetInventory(id);
        }
        public async Task<List<InventoryResponse>> GetInventoryList(string availablestatus)
        {
            return await inventoryAPIRepository.GetInventoryList(availablestatus);
        }
        public async Task<CheckInResponse> InventoryCheckIn(HttpRequest req)
        {
            return await inventoryAPIRepository.InventoryCheckIn(req);
        }
        public async Task<InventoryItem> InventoryCheckOut(HttpRequest req)
        {
            return await inventoryAPIRepository.InventoryCheckOut(req);
        }
        public async Task<TransferRequestData> InventoryTransfer(HttpRequest req)
        {
            return await inventoryAPIRepository.InventoryTransfer(req);
        }
    }
}
