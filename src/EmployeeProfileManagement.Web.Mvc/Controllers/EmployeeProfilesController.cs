using EmployeeProfileManagement.Core.Model;
using EmployeeProfileManagement.Core.Repositories.Interfaces;
using EmployeeProfileManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace EmployeeProfileManagement.Web.Mvc.Controllers
{
    public class EmployeeProfilesController : Controller
    {
        private static readonly List<EmployeeProfile> _employees = new List<EmployeeProfile>();
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployeeProfilesController> _logger;
        private readonly IBlobStorageRepository _blobStorageRepository;
        public EmployeeProfilesController(HttpClient httpClient, IConfiguration configuration, ILogger<EmployeeProfilesController> logger, IBlobStorageRepository blobStorageRepository)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _blobStorageRepository = blobStorageRepository;
        }

        // GET: EmployeeProfiles
        public async Task<IActionResult> Index()
        {
            IEnumerable<EmployeeProfile> employees = new List<EmployeeProfile>();
            try
            {
                var uri = new Uri(_configuration.GetSection("EmployeeProfileApiBaseUrl").Value + "profiles");
                var response = await _httpClient.GetAsync(uri);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!String.IsNullOrEmpty(json))
                    {
                        var result = JsonConvert.DeserializeObject<ResultObject<IEnumerable<EmployeeProfile>>>(json);
                        if (result != null)
                        {
                            employees = result.Result;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error getting profiles from the api, exception is {ex.Message}");
            }

            return View(employees.ToList());
        }

        // GET: EmployeeProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            EmployeeProfile employeeProfile = null;
            try
            {
                var uri = new Uri(_configuration.GetSection("EmployeeProfileApiBaseUrl").Value + "profiles" + $"/{id}");
                var response = await _httpClient.GetAsync(uri);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!String.IsNullOrEmpty(json))
                    {
                        var result = JsonConvert.DeserializeObject<ResultObject<EmployeeProfile>>(json);
                        if (result != null)
                        {
                            employeeProfile = result.Result;
                            //var blobresponse = await _blobStorageRepository.DownloadAsync(employeeProfile.PhotoUrl);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error getting profiles from the api, exception is {ex.Message}");
            }

            return View(employeeProfile);
        }

        // GET: EmployeeProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,DateofBirth,Designation,HireDate,PhotoUrl,Id")] EmployeeProfile employeeProfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var uri = new Uri(_configuration.GetSection("EmployeeProfileApiBaseUrl").Value + "profiles");
                    using StringContent jsonContent = new(JsonConvert.SerializeObject(employeeProfile),
        Encoding.UTF8,
        "application/json");
                    
                    var response = await _httpClient.PostAsync(uri, jsonContent );
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        if (!String.IsNullOrEmpty(json))
                        {
                            var result = JsonConvert.DeserializeObject<ResultObject<EmployeeProfile>>(json);
                            if (result != null)
                            {
                                employeeProfile = result.Result as EmployeeProfile;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error getting profiles from the api, exception is {ex.Message}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeProfile);
        }

        // GET: EmployeeProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _employees == null)
            {
                return NotFound();
            }

            var uri = new Uri(_configuration.GetSection("EmployeeProfileApiBaseUrl").Value + "profiles" + $"/{id}");
            var response = await _httpClient.GetAsync(uri);
            if (response != null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(json))
                {
                    var result = JsonConvert.DeserializeObject<ResultObject<EmployeeProfile>>(json);
                    if (result != null)
                    {
                        var employeeProfile = result.Result;
                        return View(employeeProfile);

                        //var blobresponse = await _blobStorageRepository.DownloadAsync(employeeProfile.PhotoUrl);
                    }
                    else
                        return NotFound();
                }
                else
                    return NotFound();
            }
            else
            {
                return NotFound();
            }


        }

        // POST: EmployeeProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,DateofBirth,Designation,HireDate,PhotoUrl,Id")] EmployeeProfile employeeProfile)
        {
            if (id != employeeProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var uri = new Uri(_configuration.GetSection("EmployeeProfileApiBaseUrl").Value + "profiles");
                    using StringContent jsonContent = new(JsonConvert.SerializeObject(employeeProfile),
        Encoding.UTF8,
        "application/json");

                    var response = await _httpClient.PutAsync(uri, jsonContent);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        if (!String.IsNullOrEmpty(json))
                        {
                            var result = JsonConvert.DeserializeObject<ResultObject<EmployeeProfile>>(json);
                            if (result != null)
                            {
                                employeeProfile = result.Result as EmployeeProfile;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error getting profiles from the api, exception is {ex.Message}");
                }
                return RedirectToAction(nameof(Index));

            }
            return View(employeeProfile);
        }

        // GET: EmployeeProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var uri = new Uri(_configuration.GetSection("EmployeeProfileApiBaseUrl").Value + "profiles" + $"/{id}");
                var response = await _httpClient.GetAsync(uri);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!String.IsNullOrEmpty(json))
                    {
                        var result = JsonConvert.DeserializeObject<ResultObject<EmployeeProfile>>(json);
                        if (result != null)
                        {
                           var employeeProfile = result.Result;
                            return View(employeeProfile);
                            //var blobresponse = await _blobStorageRepository.DownloadAsync(employeeProfile.PhotoUrl);
                        }
                        else
                            return NotFound();
                    }
                    else
                        return NotFound();
                }
                else
                    return NotFound() ;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting profiles from the api, exception is {ex.Message}");
                return NotFound();
            }
        }

        // POST: EmployeeProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var uri = new Uri(_configuration.GetSection("EmployeeProfileApiBaseUrl").Value + "profiles" + $"/{id}");
                var response = await _httpClient.DeleteAsync(uri);
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting profiles from the api, exception is {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
