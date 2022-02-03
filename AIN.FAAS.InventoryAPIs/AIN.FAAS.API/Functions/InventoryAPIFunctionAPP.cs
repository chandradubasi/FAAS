using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AIN.FAAS.ViewModel.Models.Response;
using AIN.FAAS.Services.IServices;

namespace AIN.FAAS.API.Functions
{
    public class InventoryAPIFunctionAPP
    {
       
        private IInventoryAPIServices _inventoryAPIServices;
        
        public InventoryAPIFunctionAPP( IInventoryAPIServices inventoryAPIServices)
        {
            _inventoryAPIServices = inventoryAPIServices;
        }

        [FunctionName("GetHospitals")]
        public async Task<IActionResult> GetHospitals([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hospitals")] HttpRequest req, ILogger log)
        {
            List<HospitalsResponse> hospitals = new List<HospitalsResponse>();
            try
            {
                hospitals = await _inventoryAPIServices.GetHospital();
                return new OkObjectResult(hospitals);
            }
            catch (Exception e)
            {                
                log.LogError(e.ToString());
                throw e;
            }
        }

        [FunctionName("GetSites")]
        public async Task<IActionResult> GetSites([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sites/{siteId}")] HttpRequest req, ILogger log, string siteId)
        {
            try
            {
                var sites = await _inventoryAPIServices.GetSites(siteId);

                if (sites == null)
                {
                    log.LogInformation($"Item {siteId} not found");
                    return new NotFoundResult();
                }
                else
                {
                    return new OkObjectResult(sites);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }
        }

        [FunctionName("GetLabs")]
        public async Task<IActionResult> GetLabs([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "labs/{labid}")] HttpRequest req, ILogger log, string labid)
        {
            try
            {
                var labslist = await _inventoryAPIServices.GetLabs(labid);

                if (labslist == null)
                {
                    log.LogInformation($"Item {labid} not found");
                    return new NotFoundResult();
                }
                else
                {
                    return new OkObjectResult(labslist);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }
        }

        [FunctionName("GetLocations")]
        public async Task<IActionResult> GetLocations([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "locations/{locationid}")] HttpRequest req, ILogger log, string locationid)
        {
            try
            {
                var locations = await _inventoryAPIServices.GetLocations(locationid);

                if (locations == null)
                {
                    log.LogInformation($"Item {locationid} not found");
                    return new NotFoundResult();
                }
                else
                {
                    return new OkObjectResult(locations);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }
        }

        [FunctionName("GetStorage")]
        public async Task<IActionResult> GetStorage([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "storages/{storageid}")] HttpRequest req, ILogger log, string storageid)
        {
            try
            {
                log.LogInformation("Getting storage item by id");
                var storages = await _inventoryAPIServices.GetStorage(storageid);
                if (storages == null)
                {
                    log.LogInformation($"Item {storageid} not found");
                    return new NotFoundResult();
                }
                else
                {
                    return new OkObjectResult(storages);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }
        }

        [FunctionName("GetInventoryById")]
        public async Task<IActionResult> GetInventory([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "inventory/{id}")] HttpRequest req, ILogger log, string id)
        {
            try
            {
                log.LogInformation("Getting locations item by id");
                var inventoryItem = await _inventoryAPIServices.GetInventory(id);
                if (inventoryItem == null)
                {
                    log.LogInformation($"Item {id} not found");
                    return new NotFoundResult();
                }
                else
                {
                    return new OkObjectResult(inventoryItem);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }
        }

        [FunctionName("GetInventoryListByStatus")]
        public async Task<IActionResult> GetInventoryList([HttpTrigger(AuthorizationLevel.Function, "get", Route = "inventorylist/{availablestatus}")] HttpRequest req, ILogger log, string availablestatus)
        {
            try
            {
                log.LogInformation("Getting locations item by id");
                var inventoryItems = await _inventoryAPIServices.GetInventoryList(availablestatus);
                if (inventoryItems == null)
                {
                    log.LogInformation($"Item {availablestatus} not found");
                    return new NotFoundResult();
                }
                return new OkObjectResult(inventoryItems);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }

        }

        [FunctionName("InventoryCheckIn")]
        public async Task<ActionResult> InventoryCheckIn([HttpTrigger(AuthorizationLevel.Function, "post", Route = "inventory/workflows/CheckIn")] HttpRequest req, ILogger log)
        {
            try
            {
                var checkinResponse = await _inventoryAPIServices.InventoryCheckIn(req);
                if (checkinResponse == null)
                {
                    return new BadRequestResult();
                }
                else
                {
                    return new OkObjectResult(checkinResponse);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }
        }

        [FunctionName("InventoryCheckOut")]
        public async Task<IActionResult> InventoryCheckOut([HttpTrigger(AuthorizationLevel.Function, "put", Route = "inventory/workflows/Checkout")] HttpRequest req, ILogger log)
        {
            try
            {
                var inventoryItem = await _inventoryAPIServices.InventoryCheckOut(req);
                if (inventoryItem == null)
                {
                    return new NotFoundResult();
                }
                else
                {
                    return new OkObjectResult(StatusCodes.Status200OK);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }
        }

        [FunctionName("InventoryTransfer")]
        public async Task<IActionResult> InventoryTransfer([HttpTrigger(AuthorizationLevel.Function, "put", Route = "inventory/workflows/transfer")] HttpRequest req, ILogger log)
        {
            var transferResponse = await _inventoryAPIServices.InventoryTransfer(req);
            try
            {
                if (transferResponse == null)
                {
                    return new BadRequestResult();
                }
                else
                {
                    return new OkObjectResult(transferResponse);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                throw e;
            }
        }
    }
}
