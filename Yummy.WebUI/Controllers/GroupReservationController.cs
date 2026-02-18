using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;
public class GroupReservationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GroupReservationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult ReservationDetailModal()
    {
        return PartialView("_ReservationDetailModal");
    }

    [HttpGet]
    public async Task<IActionResult> GetReservationDetail(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");
        var response = await client.GetAsync($"GroupReservations/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return Json(new { success = false, message = "Rezervasyon bulunamadı" });
        }

        var responseData = await response.Content.ReadAsStringAsync();
        var reservation = JsonConvert.DeserializeObject<GetByIdGroupReservationDto>(responseData);

        return Json(new { success = true, data = reservation });
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("YummyApi");
            var response = await client.GetAsync($"GroupReservations/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Rezervasyon bulunamadı";
                return RedirectToAction("Index", "Dashboard");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var reservation = JsonConvert.DeserializeObject<GetByIdGroupReservationDto>(responseData);

            return View(reservation);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Hata: {ex.Message}";
            return RedirectToAction("Index", "Dashboard");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateGroupReservationDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm zorunlu alanları doldurun";
            return View(updateDto);
        }

        try
        {
            var client = _httpClientFactory.CreateClient("YummyApi");

            var jsonContent = JsonConvert.SerializeObject(updateDto);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"GroupReservations", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Güncelleme başarısız: {errorContent}";
                return View(updateDto);
            }

            TempData["SuccessMessage"] = "Rezervasyon başarıyla güncellendi";
            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Hata: {ex.Message}";
            return View(updateDto);
        }
    }
}
