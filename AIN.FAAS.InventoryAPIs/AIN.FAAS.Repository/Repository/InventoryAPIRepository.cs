using AIN.FAAS.Repository.IRepository;
using AIN.FAAS.Repository.Models;
using AIN.FAAS.ViewModel.Models.Request;
using AIN.FAAS.ViewModel.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AIN.FAAS.Repository.Repository
{
    public class InventoryAPIRepository : IInventoryAPIRepository
    {
        private readonly AINDatabaseContext _aINDatabaseContext;
        private static Random random = new Random();
        public enum AvailableStatus
        {
            InTransit,
            Available,
            UnAvailable
        }
        public InventoryAPIRepository(AINDatabaseContext aINDatabaseContext)
        {
            _aINDatabaseContext = aINDatabaseContext;
        }
        public async Task<List<HospitalsResponse>> GetHospital()
        {
            List<HospitalsResponse> hospitals = new List<HospitalsResponse>();
            try
            {
                var hospitalsList = await _aINDatabaseContext.Hospital.Include(h => h.Site).ToListAsync();
                if (hospitalsList != null)
                {
                    foreach (var hospital in hospitalsList)
                    {
                        HospitalsResponse hospitalsResponse = new HospitalsResponse()
                        {
                            HospitalId = hospital.Id.ToString(),
                            Name = hospital.Name,
                            Sites = hospital.Site.Select(e => Environment.GetEnvironmentVariable("APIDomain") + "/api/sites/" + e.Id).ToArray()
                        };
                        hospitals.Add(hospitalsResponse);
                    }
                }
                return hospitals;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<SitesResponse> GetSites(string siteId)
        {
            try
            {
                var sitelist = await _aINDatabaseContext.Site.Where(s => s.Id == Guid.Parse(siteId)).Include(h => h.Lab).FirstOrDefaultAsync();

                if (sitelist == null)
                {
                    return null;
                }
                else
                {
                    SitesResponse sitesResponse = new SitesResponse()
                    {
                        SiteId = sitelist.Id.ToString(),
                        Name = sitelist.Name,
                        Labs = sitelist.Lab.Select(e => Environment.GetEnvironmentVariable("APIDomain") + "/api/labs/" + e.Id).ToArray(),
                    };
                    return sitesResponse;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<LabsResponse> GetLabs(string labid)
        {
            try
            {
                var labs = await _aINDatabaseContext.Lab.Where(l => l.Id == Guid.Parse(labid)).Include(loc => loc.Location).FirstOrDefaultAsync();

                if (labs == null)
                {
                    return null;
                }
                else
                {
                    LabsResponse labsResponse = new LabsResponse()
                    {
                        LabId = labs.Id.ToString(),
                        Name = labs.Name,
                        Locations = labs.Location.Select(loc => Environment.GetEnvironmentVariable("APIDomain") + "/api/locations/" + loc.Id).ToArray(),
                    };
                    return labsResponse;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<LocationsResponse> GetLocations(string locationid)
        {
            try
            {
                var locations = await _aINDatabaseContext.Location.Where(loc => loc.Id == Guid.Parse(locationid)).Include(s => s.Storage).FirstOrDefaultAsync();

                if (locations == null)
                {
                    return null;
                }
                else
                {
                    LocationsResponse locationsResponse = new LocationsResponse()
                    {
                        LocationId = locations.Id.ToString(),
                        Name = locations.Name,
                        Storages = locations.Storage.Select(s => Environment.GetEnvironmentVariable("APIDomain") + "/api/storages/" + s.Id).ToArray(),
                    };
                    return locationsResponse;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<StoragesResponse> GetStorage(string storageid)
        {
            try
            {
                var storages = await _aINDatabaseContext.Storage.Where(s => s.Id == Guid.Parse(storageid)).Include(s => s.InventoryItem).FirstOrDefaultAsync();
                if (storages == null)
                {
                    return null;
                }
                else
                {
                    StoragesResponse storagesResponse = new StoragesResponse()
                    {
                        StorageId = storages.Id.ToString(),
                        Name = storages.Name,
                        Inventory = storages.InventoryItem.Select(I => Environment.GetEnvironmentVariable("APIDomain") + "/api/inventory/" + I.Id).ToArray(),
                    };
                    return storagesResponse;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<InventoryResponse> GetInventory(string id)
        {
            try
            {
                var inventoryItem = await _aINDatabaseContext.InventoryItem.Where(i => i.Id == Guid.Parse(id)).FirstOrDefaultAsync();
                if (inventoryItem == null)
                {
                    return null;
                }
                InventoryResponse inventoryResponse = new InventoryResponse()
                {
                    InventoryId = inventoryItem.Id.ToString(),
                    SGTIN = inventoryItem.Sgtin,
                    ProductId = Environment.GetEnvironmentVariable("APIDomain") + "/api/products/" + inventoryItem.ProductId.ToString(),
                    LotNumber = inventoryItem.LotNumber.ToString(),
                    ExpiryDate = inventoryItem.ExpiryDate,
                    StorageId = Environment.GetEnvironmentVariable("APIDomain") + "/api/storages/" + inventoryItem.StorageId.ToString(),

                };
                return inventoryResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<InventoryResponse>> GetInventoryList(string availablestatus)
        {
            List<InventoryResponse> inventoryResponses = new List<InventoryResponse>();
            try
            {
                var inventoryItems = await _aINDatabaseContext.InventoryItem.Where(i => i.Status == availablestatus).ToListAsync();
                if (inventoryItems == null)
                {
                    return null;
                }
                foreach (var inventoryItem in inventoryItems)
                {
                    InventoryResponse inventoryResponse = new InventoryResponse()
                    {
                        InventoryId = inventoryItem.Id.ToString(),
                        SGTIN = inventoryItem.Sgtin,
                        ProductId = Environment.GetEnvironmentVariable("APIDomain") + "/api/products/" + inventoryItem.ProductId.ToString(),
                        LotNumber = inventoryItem.LotNumber.ToString(),
                        ExpiryDate = inventoryItem.ExpiryDate,
                        StorageId = Environment.GetEnvironmentVariable("APIDomain") + "/api/storages/" + inventoryItem.StorageId.ToString(),

                    };
                    inventoryResponses.Add(inventoryResponse);
                }
                return inventoryResponses;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<CheckInResponse> InventoryCheckIn(HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var checkInRequest = JsonConvert.DeserializeObject<CheckInRequest>(requestBody);

            try
            {
                List<InventoryItem> dboInventoryItems = new List<InventoryItem>();
                if (checkInRequest.AutoId)
                {
                    checkInRequest.SGTIN = new string[checkInRequest.Quantity];
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    for (var i = 0; i < checkInRequest.Quantity; i++)
                    {
                        checkInRequest.SGTIN[i] = new string(Enumerable.Repeat(chars, 24)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                    }
                }
                foreach (var sgtin in checkInRequest.SGTIN)
                {
                    dboInventoryItems.Add(new Models.InventoryItem()
                    {
                        Sgtin = sgtin,
                        StorageId = new Guid(checkInRequest.StorageId),
                        ProductId = new Guid(checkInRequest.ProductId),
                        ExpiryDate = checkInRequest.ExpiryDate,
                        LotNumber = Convert.ToInt32(checkInRequest.LotNumber),
                        Remarks = checkInRequest.Remarks,
                        Status = AvailableStatus.Available.ToString(),
                    });

                }

                _aINDatabaseContext.InventoryItem.AddRange(dboInventoryItems);
                await _aINDatabaseContext.SaveChangesAsync();

                CheckInResponse checkinResponse = new CheckInResponse()
                {
                    Status = "success",
                    SGTIN = checkInRequest.SGTIN,
                    Product = Environment.GetEnvironmentVariable("APIDomain") + "/api/products/" + checkInRequest.ProductId,
                    ExpiryDate = checkInRequest.ExpiryDate,
                    Lot = checkInRequest.LotNumber,
                    Storage = Environment.GetEnvironmentVariable("APIDomain") + "/api/Storages/" + checkInRequest.StorageId
                };

                return checkinResponse;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<InventoryItem> InventoryCheckOut(HttpRequest req)
        {
            InventoryItem inventoryItem = new InventoryItem();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var checkOutRequest = JsonConvert.DeserializeObject<CheckOutRequest>(requestBody);

            try
            {
                for (var j = 0; j < checkOutRequest.SGTIN.Length; j++)
                {

                    inventoryItem = await _aINDatabaseContext.InventoryItem.Where(i => i.Sgtin == checkOutRequest.SGTIN[j].ToString()).FirstOrDefaultAsync();

                    if (inventoryItem == null)
                    {
                        return null;
                    }
                    else
                    {
                        inventoryItem.Status = AvailableStatus.UnAvailable.ToString();
                        await _aINDatabaseContext.SaveChangesAsync();
                    }
                }
                return inventoryItem;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<TransferRequestData> InventoryTransfer(HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var transferRequest = JsonConvert.DeserializeObject<TransferRequestData>(requestBody);

            try
            {
                for (var j = 0; j < transferRequest.Items.Length; j++)
                {
                    var transferItem = await _aINDatabaseContext.InventoryItem.Where(i => i.Id == new Guid(transferRequest.Items[j]) && i.StorageId == new Guid(transferRequest.SourceId)).FirstOrDefaultAsync();
                    if (transferItem != null)
                    {

                        if (!(transferRequest.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase)) && !(transferRequest.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))) // update CheckInStatus with Pending approval
                        {

                            transferItem.Status = AvailableStatus.InTransit.ToString();
                            await _aINDatabaseContext.SaveChangesAsync();

                            TransferRequest dbtransferRequest = new TransferRequest()
                            {
                                Items = new Guid(transferRequest.Items[j]),
                                SourceId = new Guid(transferRequest.SourceId),
                                DestinationId = new Guid(transferRequest.DestinationId),
                                Requester = transferRequest.Requester,
                                RequestedDate = transferRequest.RequestedDate,
                                Approver = transferRequest.Approver,
                                Status = transferRequest.Status,
                                Comments = transferRequest.Comments
                            };
                            _aINDatabaseContext.TransferRequest.AddRange(dbtransferRequest);
                            await _aINDatabaseContext.SaveChangesAsync();
                        }

                        else // approved or Rejected case
                        {
                            if (transferRequest.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                            {
                                transferItem.Status = AvailableStatus.Available.ToString();
                                transferItem.StorageId = new Guid(transferRequest.DestinationId);
                                await _aINDatabaseContext.SaveChangesAsync();

                                TransferRequest dbtransferRequest = new TransferRequest()
                                {
                                    Items = new Guid(transferRequest.Items[j]),
                                    SourceId = new Guid(transferRequest.SourceId),
                                    DestinationId = new Guid(transferRequest.DestinationId),
                                    Requester = transferRequest.Requester,
                                    RequestedDate = transferRequest.RequestedDate,
                                    Approver = transferRequest.Approver,
                                    Status = transferRequest.Status,
                                    Comments = transferRequest.Comments
                                };
                                _aINDatabaseContext.TransferRequest.AddRange(dbtransferRequest);
                                await _aINDatabaseContext.SaveChangesAsync();
                            }

                            else if (transferRequest.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                            {
                                transferItem.Status = AvailableStatus.Available.ToString();
                                await _aINDatabaseContext.SaveChangesAsync();

                                TransferRequest dbtransferRequest = new TransferRequest()
                                {
                                    Items = new Guid(transferRequest.Items[j]),
                                    SourceId = new Guid(transferRequest.SourceId),
                                    DestinationId = new Guid(transferRequest.DestinationId),
                                    Requester = transferRequest.Requester,
                                    RequestedDate = transferRequest.RequestedDate,
                                    Approver = transferRequest.Approver,
                                    Status = transferRequest.Status,
                                    Comments = transferRequest.Comments
                                };

                                _aINDatabaseContext.TransferRequest.AddRange(dbtransferRequest);
                                await _aINDatabaseContext.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return transferRequest;
        }
    }
}
