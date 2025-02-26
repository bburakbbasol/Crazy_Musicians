using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crazy_Musicians.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection.Metadata.Ecma335;

namespace Crazy_Musicians.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Musicians : ControllerBase
	{
		// Müzisyenlerin listesi (örnek veri)
		private static List<Musician> _musicians = new List<Musician>()
		{
			new Musician{Id=1, Name="Ahmet Çalgı", Job="Ünlü Çalgı Çalar", Description="Her zaman yanlış nota çalar, ama çok eğlenceli"},
			new Musician{Id=2, Name="Zeynep Melodi", Job="Popüler Melodi Yazarı", Description="Şarkılar yanlış anlaşılır ama çok popüler"},
			new Musician{Id=3, Name="Cemil Akor", Job="Çılgın Akorist", Description="Akorları sık değiştirir, ama şaşırtıcı derecede yetenekli"},
			new Musician{Id=4, Name="Fatma Nota", Job="Sürpriz Nota Üreticisi", Description="Nota üretirken sürekli sürprizler hazırlar"},
			new Musician{Id=5, Name="Hasan Ritim", Job="Ritim Canavarı", Description="Her ritmi kendi tarzında yapar, hiç uymaz ama komiktir"},
			new Musician{Id=6, Name="Elif Armoni", Job="Armoni Ustası", Description="Armonileri bazen yanlış çalar, ama çok yaratıcıdır"},
			new Musician{Id=7, Name="Ali Perde", Job="Perde Uygulayıcı", Description="Her perdeyi farklı şekilde çalar, her zaman sürprizdir"},
			new Musician{Id=8, Name="Ayşe Rezonans", Job="Rezonans Uzmanı", Description="Rezonans konusunda uzman, ama bazen çok fazla gürültü çıkarıyor"},
			new Musician{Id=9, Name="Murat Ton", Job="Tonlama Meraklısı", Description="Tonlamadaki farklılıklar bazen komik, ama oldukça ilginç"},
			new Musician{Id=10, Name="Selin Akor", Job="Akor Sihirbazı", Description="Akorları değiştirdiğinde bazen sihirli bir hava yaratır"}
		};

		// Tüm müzisyenleri getirir
		[HttpGet]
		public IEnumerable<Musician> GetAll()
		{
			return _musicians;
		}

		// ID'ye göre müzisyeni getirir
		[HttpGet("{id:int:min(1)}")]
		public ActionResult<Musician> GetMusician(int id)
		{
			var musician = _musicians.FirstOrDefault(m => m.Id == id);
			if (musician == null)
			{
				return NotFound($"{id} is not a musician");
			}
			return Ok(musician);
		}

		// İsme göre müzisyeni getirir
	
		[HttpGet("Name/{name:alpha}")]
		public ActionResult<Musician> GetMusicanByName(string name)
		{
			// İsme veya ilk isme göre müzisyeni bul
			var musician = _musicians.FirstOrDefault(m =>
				m.Name.Contains(name, StringComparison.OrdinalIgnoreCase) ||   // Tam isim eşleşmesi veya isminin bir parçası geçiyorsa
				m.Name.Split(' ')[0].Equals(name, StringComparison.OrdinalIgnoreCase)); // İlk isim ile eşleşme

			// Müzisyen bulunamazsa 404 döndür
			if (musician == null)
			{
				return NotFound($"{name} is not a musician");
			}

			// Müzisyeni döndür
			return musician;
		}


		// Yeni müzisyen ekleme işlemi
		[HttpPost]
		public ActionResult<Musician> Create([FromBody] Musician musician)
		{
			var id = _musicians.Max(m => m.Id) + 1;
			musician.Id = id;
			_musicians.Add(musician);
			return CreatedAtAction(nameof(GetMusician), new { id = musician.Id }, musician);
		}

		// Müzisyen bilgilerini JSON Patch ile güncelleme işlemi
		[HttpPatch("reschedule/{id:int:min(1)}")]
		public IActionResult Reschedule(int id, [FromBody] JsonPatchDocument<Musician> patchDocument)
		{
			var musician = _musicians.FirstOrDefault(m => m.Id == id);
			if (musician is null)
			{
				return NotFound(new { message = "User Not Found" });
			}

			patchDocument.ApplyTo(musician);
			return NoContent();
		}

		// Tüm müzisyen bilgilerini güncelleme işlemi
		[HttpPut("/update")]
		public IActionResult Put(int id, [FromBody] Musician musician)
		{
			if (musician is null)
			{
				return BadRequest();
			}

			var existingMusician = _musicians.FirstOrDefault(m => m.Id == id);
			if (existingMusician is null)
			{
				return NotFound();
			}

			existingMusician.Id = musician.Id;
			existingMusician.Name = musician.Name;
			existingMusician.Job = musician.Job;
			existingMusician.Description = musician.Description;

			return Ok(existingMusician);
		}

		// Belirtilen ID'ye sahip müzisyeni silme işlemi
		[HttpDelete("delete/{id:int:min(1)}")]
		public IActionResult Delete(int id)
		{
			var deleteMusician = _musicians.FirstOrDefault(m => m.Id == id);
			if (deleteMusician is null)
			{
				return NotFound();
			}

			_musicians.Remove(deleteMusician);
			return NoContent();
		}

		// Müzisyenleri isim veya meslek alanına göre arama işlemi
		[HttpGet("Search")]
		public IActionResult Search([FromQuery] string keyword, [FromQuery] int? page = 1, [FromQuery] int pageSize = 3)
		{
			// Boş anahtar kelime kontrolü
			if (string.IsNullOrWhiteSpace(keyword))
			{
				return BadRequest(new { message = "Keyword parameter is required." });
			}

			// İsme, soyisme veya meslek alanına göre arama yapar
			var results = _musicians
				.Where(m =>
					m.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||   // Tam isim içinde arar
					m.Name.Split(' ')[0].Equals(keyword, StringComparison.OrdinalIgnoreCase) ||  // İlk isim ile eşleşme
					m.Job.Contains(keyword, StringComparison.OrdinalIgnoreCase))  // Meslek içinde arama
				.ToList();

			if (results.Count == 0)
			{
				return NotFound(new { message = "No musicians found matching the keyword." });
			}

			// Sayfalama işlemi
			var pagedResults = results.Skip((page.Value - 1) * pageSize).Take(pageSize).ToList();

			return Ok(new
			{
				keyword,
				page,
				totalResults = results.Count,
				results = pagedResults
			});
		}


	}
}
