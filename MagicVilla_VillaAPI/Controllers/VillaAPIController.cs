using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
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
		private readonly IVillaRepository _villaRepo;
		private readonly IMapper _mapper;
		public VillaAPIController(IVillaRepository villaRepo, IMapper mapper)
		{
			_villaRepo = villaRepo;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
		{
			IEnumerable<Villa> villaList = await _villaRepo.ReadAllAsync();
			return Ok(_mapper.Map<List<VillaDTO>>(villaList));
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<VillaDTO>> GetVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = await _villaRepo.ReadAsync(u => u.Id == id);
			if (villa == null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<VillaDTO>(villa));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
		{

			if (await _villaRepo.ReadAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
			{
				ModelState.AddModelError("DuplicateError", "Villa already Exists!");
				return BadRequest(ModelState);

			}
			if (createDTO == null)
			{
				return BadRequest(createDTO);
			}

			Villa newVilla = _mapper.Map<Villa>(createDTO);

			await _villaRepo.CreateAsync(newVilla);

			return CreatedAtRoute("GetVilla", new { id = newVilla.Id }, newVilla);
		}

		[HttpDelete("{id:int}", Name = "DeleteVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = await _villaRepo.ReadAsync(u => u.Id == id);
			if (villa == null)
			{
				return NotFound();
			}
			
			await _villaRepo.DeleteAsync(villa);

			return NoContent();
		}

		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
		{
			if (updateDTO == null || id != updateDTO.Id)
			{
				return BadRequest();
			}

			Villa myVilla = _mapper.Map<Villa>(updateDTO);

			await _villaRepo.UpdateAsync(myVilla);

			return NoContent();
		}

		// Check jsonpatch.com
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchVillaDTO)
		{
			
			if (patchVillaDTO == null || id == 0)
			{
				return BadRequest();
			}
			var villa = await _villaRepo.ReadAsync(u => u.Id == id, tracked:false);

			VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

			if (villa == null)
			{
				return BadRequest();
			}

			patchVillaDTO.ApplyTo(villaDTO, ModelState);
			Villa myVilla = _mapper.Map<Villa>(villaDTO);

			await _villaRepo.UpdateAsync(myVilla);

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			return NoContent();
		}
	}
}
