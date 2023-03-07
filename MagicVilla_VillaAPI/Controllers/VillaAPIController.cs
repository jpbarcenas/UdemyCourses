using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		private readonly ApplicationDbContext _dbo;
		public VillaAPIController(ApplicationDbContext dbo)
		{
			_dbo = dbo;
		}

		[HttpGet]
		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
		{
			return Ok(_dbo.Villas.ToList());
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<VillaDTO> GetVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = _dbo.Villas.FirstOrDefault(u => u.Id == id);
			if (villa == null)
			{
				return NotFound();
			}
			return Ok(villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<VillaDTO> CreateVilla([FromBody] VillaCreateDTO villaDTO)
		{
			//if (!ModelState.IsValid)
			//{
			//	return BadRequest(ModelState);
			//}
			if (_dbo.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
			{
				ModelState.AddModelError("DuplicateError", "Villa already Exists!");
				return BadRequest(ModelState);

			}
			if (villaDTO == null)
			{
				return BadRequest(villaDTO);
			}
			//if (villaDTO.Id > 0)
			//{
			//	return StatusCode(StatusCodes.Status500InternalServerError);
			//}

			Villa newVilla = new()
			{
				Amenity = villaDTO.Amenity,
				Details = villaDTO.Details,
				ImageUrl = villaDTO.ImageUrl,
				Name = villaDTO.Name,
				Occupancy = villaDTO.Occupancy,
				Rate = villaDTO.Rate,
				Sqft = villaDTO.Sqft
			};

			_dbo.Villas.Add(newVilla);
			_dbo.SaveChanges();

			return CreatedAtRoute("GetVilla", new { id = newVilla.Id }, newVilla);
		}

		[HttpDelete("{id:int}", Name = "DeleteVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeleteVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = _dbo.Villas.FirstOrDefault(u => u.Id == id);
			if (villa == null)
			{
				return NotFound();
			}
			_dbo.Villas.Remove(villa);
			_dbo.SaveChanges();

			return NoContent();
		}

		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
		{
			if (villaDTO == null || id != villaDTO.Id)
			{
				return BadRequest();
			}
			//var villa = _dbo.Villas.FirstOrDefault(u => u.Id == id);
			//villa.Name = villaDTO.Name;
			//villa.Sqft = villaDTO.Sqft;
			//villa.Occupancy = villaDTO.Occupancy;

			Villa myVilla = new()
			{
				Amenity = villaDTO.Amenity,
				Details = villaDTO.Details,
				Id = villaDTO.Id,
				ImageUrl = villaDTO.ImageUrl,
				Name = villaDTO.Name,
				Occupancy = villaDTO.Occupancy,
				Rate = villaDTO.Rate,
				Sqft = villaDTO.Sqft
			};

			_dbo.Villas.Update(myVilla);
			_dbo.SaveChanges();

			return NoContent();
		}

		// Check jsonpatch.com
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchVillaDTO)
		{
			
			if (patchVillaDTO == null || id == 0)
			{
				return BadRequest();
			}
			var villa = _dbo.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);

			VillaUpdateDTO myVillaDTO = new()
			{
				Amenity = villa.Amenity,
				Details = villa.Details,
				Id = villa.Id,
				ImageUrl = villa.ImageUrl,
				Name = villa.Name,
				Occupancy = villa.Occupancy,
				Rate = villa.Rate,
				Sqft = villa.Sqft
			};

			if (villa == null)
			{
				return BadRequest();
			}

			patchVillaDTO.ApplyTo(myVillaDTO, ModelState);
			Villa myVilla = new()
			{
				Amenity = myVillaDTO.Amenity,
				Details = myVillaDTO.Details,
				Id = myVillaDTO.Id,
				ImageUrl = myVillaDTO.ImageUrl,
				Name = myVillaDTO.Name,
				Occupancy = myVillaDTO.Occupancy,
				Rate = myVillaDTO.Rate,
				Sqft = myVillaDTO.Sqft
			};

			_dbo.Villas.Update(myVilla);
			_dbo.SaveChanges();

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			return NoContent();
		}
	}
}
